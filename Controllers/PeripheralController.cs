using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musala.Models;
using Musala.Resources;
using System.Collections.Generic;
using System.Reflection;
using Musala.Resources.Mapper;
using Musala.Models.DTO;
using System.Threading.Tasks;
using Musala.Resources.Filters;
using Musala.Resources.Payload;

namespace Musala.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeripheralsController : ControllerBase
    {
        private readonly MusalaTestContext _context;
        private readonly IMapper _mapper;

        public PeripheralsController(MusalaTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// This method return a list with all peripherals from DB
        /// </summary>
        /// <returns>IEnumerable</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([ModelBinder(typeof(QueryModelBinder))] QueryObject query)
        {
            try
            {
                //This method convert the data to list two time, beacuse the filter library can't compare string and gui and is used like gatewayId
                var response = _context.Peripheral
                                 .Select(peripheral => _mapper.PeripheralToPeripheralDto(peripheral))
                                 .ToList()
                                 .AsQueryable()
                                 .ApplyQuery(query)
                                 .ToList();

                var factory = new CollectionPayloadFactory<PeripheralDto>(response, query);

                return new SuccessPayloadResult(factory.GetPayload());
            }
            catch (FormatException formatException)
            {
                var factory = new ErrorPayloadFactory(new string[] { formatException.Message }, "Sorry, there is a problem with the input values.");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }
            catch (Exception exception)
            {
                var factory = new ErrorPayloadFactory(new string[] { exception.Message }, "Sorry, there is a problem with the input values.");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }

        }

        /// <summary>
        /// This method create a new Peripheral and return the Peripheral created.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(SavePeripheralDto savePeripheral)
        {
            Peripheral peripheral = _mapper.SavePeripheralDtoToPeripheral(savePeripheral);

            var isCreated = await _context.Peripheral.Select(
                    p =>
                    new
                    {
                        ValueForAvoidDuplicate = p.Vendor + p.GatewayId.ToString()
                    }
                )
                .AnyAsync(keys => keys.ValueForAvoidDuplicate.Equals(savePeripheral.ValueForAvoidDuplicity()))
                .ConfigureAwait(false);

            if (isCreated)
            {
                var factory = new ErrorPayloadFactory(new string[] { "Can't create the object." }, "The Peripheral was created before");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }

            var findGateway = await _context.Gateway
                    .Include(p => p.Peripherals)
                    .FirstAsync(p => p.Id == peripheral.GatewayId)
                    .ConfigureAwait(false);

            if (findGateway == null)
            {
                var factory = new ErrorPayloadFactory(new string[] { "Do not exits the gatewayID, please user other ID" }, "Gateway not found.");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }
            else if (findGateway.Peripherals.Count >= 10)
            {
                var factory = new ErrorPayloadFactory(new string[] { "The selected gateway has reach(10 peripheals) the maximum number of peripheals allowed." }, "The Gateway can't contains more peripherals.");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }
            else
            {
                var peripheralResult = _context.Peripheral.Add(peripheral);
                _context.SaveChanges();

                var factory = new SingleObjectPayloadFactory<PeripheralDto>(_mapper.PeripheralToPeripheralDto(peripheralResult.Entity));
                return new CreatedPayloadResult(factory.GetPayload());
            }
        }

        // DELETE: admin/roles/5
        [HttpDelete("{id}")]
        public IActionResult Del(int id)
        {
            var data = _context.Peripheral.Find(id);

            if (data == null)
            {
                var errorFactory = new ErrorPayloadFactory(new string[] { "The peripheral's id selected does not exist." }, "Peripheral not found");
                return new CustomPayloadResult(errorFactory.GetPayload(), 404);

            }
            else
            {
                var removeResult = _context.Peripheral.Remove(data);
                _context.Attach(data).State = EntityState.Deleted;
                _context.SaveChanges();

                var factory = new MessagePayloadFactory("Peripheral deleted successfully");
                return new SuccessPayloadResult(factory.GetPayload());

            }
        }


        // Get: admin/roles/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var data = _context.Peripheral.Find(id);

            if (data == null)
            {

                var errorFactory = new ErrorPayloadFactory(new string[] { "The peripheral's id selected does not exist." }, "Peripheral not found");
                return new CustomPayloadResult(errorFactory.GetPayload(), 404);

            }
            else
            {
                var factory = new SingleObjectPayloadFactory<PeripheralDto>(_mapper.PeripheralToPeripheralDto(data));
                return new SuccessPayloadResult(factory.GetPayload());
            }
        }
    }
}
