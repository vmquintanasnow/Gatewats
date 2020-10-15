using AutoMapper;
using Musala.Business.DTO;
using Musala.Business.Filters;
using Musala.DAL;
using Musala.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Musala.Business.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Musala.Business.Payload;

namespace Musala.Business.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly MusalaContext _context;
        private readonly IMapper _mapper;

        public GatewayService(MusalaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IPayload SaveGateway(SaveGatewayDto saveGatewayDto)
        {
            Gateway gateway = _mapper.Map<Gateway>(saveGatewayDto);

            var isCreated = _context.Gateways.Select(
                    gat =>
                    new
                    {
                        ValueForAvoidDuplicate = gat.Name + gat.Ipv4
                    }
                )
                .Any(keys => keys.ValueForAvoidDuplicate.Equals(saveGatewayDto.ValueToAvoidDuplicate()));

            if (!isCreated)
            {
                var result = _context.Gateways.Add(gateway);
                _context.SaveChanges();

                var factory = new SingleObjectPayloadFactory<GatewayDto>(_mapper.Map<GatewayDto>(result.Entity));
                return factory.GetPayload();

            }
            else
            {
                throw new AlreadyExistException("The same Gateway was created before");
            }
        }

        public void Delete(Guid id)
        {
            Gateway gatewayToDelete;
            try
            {
                gatewayToDelete = _context.Gateways.Include(g => g.Devices).Single(g => g.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Gateways with id: {id.ToString()}, hasn't been found in db.");
            }            

            _context.Gateways.Remove(gatewayToDelete);
            _context.Attach(gatewayToDelete).State = EntityState.Deleted;
            _context.SaveChanges();
            
        }
  
        public async  Task<IPayload> FindById(Guid id)
        {
            Gateway data;
            try
            {
                data = await _context.Gateways
                    .Include(g => g.Devices)
                    .SingleAsync(g => g.Id.Equals(id));
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"The Gateways with id:{id.ToString()}, hasn't been found in db.");
            }

            var factory = new SingleObjectPayloadFactory<GatewayDto>(_mapper.Map<GatewayDto>(data));
            return factory.GetPayload();
        }

        async Task<IPayload> IGatewayService.GetAllGateways(QueryObject queryObject)
        {
            List<GatewayDto> gateways = await _context.Gateways
                    .Include(gateway => gateway.Devices)
                    .ApplyQuery(queryObject, out int totalItems)
                    .Select(gateway => _mapper.Map<GatewayDto>(gateway))
                    .ToListAsync()
                    .ConfigureAwait(false);

            var factory = new CollectionPayloadFactory<GatewayDto>(gateways, queryObject,totalItems);
            return factory.GetPayload();
        }
    }
}
