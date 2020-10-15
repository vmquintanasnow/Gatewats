using Musala.Business.DTO;
using Musala.Business.Filters;
using Musala.Business.Payload;
using Musala.DAL;
using Musala.Domain.Entity;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Musala.Business.Exceptions;

namespace Musala.Business.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly MusalaContext _context;
        private readonly IMapper _mapper;

        public DeviceService(MusalaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            var device = _context.Devices.Find(id);

            if (device == null)
            {
                throw new DoesNotExistException($"Device with id: { id }, hasn't been found in db.");
            }

            _context.Devices.Remove(device);
            _context.Attach(device).State = EntityState.Deleted;
            _context.SaveChanges();

        }

        public async Task<IPayload> FindById(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                throw new DoesNotExistException($"Device with id: { id }, hasn't been found in db.");
            }
            else
            {
                var factory = new SingleObjectPayloadFactory<DeviceDto>(_mapper.Map<DeviceDto>(device));
                return factory.GetPayload();
            }
        }

        /// <summary> 
        /// </summary>
        /// <exception cref="FormatException"/>
        /// <param name="queryObject"></param>
        /// <returns></returns>
        public async Task<IPayload> GetAllDevices(QueryObject queryObject)
        {
            var devices = await _context.Devices
                            .ApplyQuery(queryObject, out int totalItems)
                            .Select(device => _mapper.Map<DeviceDto>(device))
                            .ToListAsync()
                            .ConfigureAwait(false);

            var factory = new CollectionPayloadFactory<DeviceDto>(devices, queryObject,totalItems);
            return factory.GetPayload();

        }

        public async Task<IPayload> SaveDevice(SaveDeviceDto saveDeviceDto)
        {
            var isCreated = await _context.Devices.Select(
                    p =>
                    new
                    {
                        ValueForAvoidDuplicate = p.Vendor + p.GatewayId.ToString()
                    }
                )
                .AnyAsync(keys => keys.ValueForAvoidDuplicate.Equals(saveDeviceDto.ValueToAvoidDuplicate())).ConfigureAwait(true);

            if (isCreated)
            {
                throw new AlreadyExistException($"The same Device was created before");
            }

            Device device = _mapper.Map<Device>(saveDeviceDto);
            Gateway gateway;
            try
            {
                gateway = await _context.Gateways
                .Include(p => p.Devices)
                .SingleAsync(p => p.Id == device.GatewayId).ConfigureAwait(false);

            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Gateways with id: { device.GatewayId }, hasn't been found in db.");
            }


            if (gateway.Devices.Count >= 10)
            {
                throw new GatewayDeviceLimitExceeded($"Gateways {gateway.Name} cannot support more than 10 devices.");
            }

            var peripheralResult = _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            var factory = new SingleObjectPayloadFactory<DeviceDto>(_mapper.Map<DeviceDto>(peripheralResult.Entity));
            return factory.GetPayload();

        }

    }
}
