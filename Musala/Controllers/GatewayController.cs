using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musala.Resources;
using Musala.Models;
using Musala.Models.DTO;
using Musala.Resources.Mapper;
using Musala.Resources.Filters;
using System.Threading.Tasks;
using Musala.Resources.Payload;

namespace Musala.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewaysController : ControllerBase
    {
        private readonly MusalaTestContext _context;
        private readonly IMapper _mapper;

        public GatewaysController(MusalaTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: gateways/
        [HttpGet]
        public async Task<IActionResult> GetAll([ModelBinder(typeof(QueryModelBinder))] QueryObject query)
        {
            try
            {
                var response = await _context.Gateway
                    .Include(gateway => gateway.Peripherals)
                    .ApplyQuery(query)
                    .Select(gateway => _mapper.GatewayToGatewayDto(gateway))
                    .ToListAsync()
                    .ConfigureAwait(false);

                var factory = new CollectionPayloadFactory<GatewayDto>(response, query);

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

        // POST: gateways/
        [HttpPost]
        public IActionResult Post(SaveGatewayDto saveGatewayDto)
        {
            Gateway data = _mapper.SaveGatewayDtoToGateway(saveGatewayDto);

            var isCreated = _context.Gateway.Select(
                    gateway =>
                    new
                    {
                        ValueForAvoidDuplicate = gateway.Name + gateway.Ipv4
                    }
                )
                .Any(keys => keys.ValueForAvoidDuplicate.Equals(saveGatewayDto.ValueToAvoidDuplicate()));

            if (isCreated)
            {
                var factory = new ErrorPayloadFactory(new string[] { "Can't create the object." }, "The Gateway was created before");
                return new CustomPayloadResult(factory.GetPayload(), 400);
            }
            else
            {
                var result = _context.Gateway.Add(data);
                _context.SaveChanges();

                var factory = new SingleObjectPayloadFactory<Gateway>(result.Entity);
                return new CreatedPayloadResult(factory.GetPayload());
            }
        }

        // DELETE: gateways/5
        [HttpDelete("{id}")]
        public IActionResult Del(string id)
        {
            var data = _context.Gateway.Find(new Guid(id));

            if (data == null)
            {
                var factory = new ErrorPayloadFactory(new string[] { "The Gateway's id selected does not exist." }, "Gateway not found");
                return new CustomPayloadResult(factory.GetPayload(), 404);
            }
            else
            {
                try
                {
                    var removeResult = _context.Gateway.Remove(data);
                    _context.Attach(data).State = EntityState.Deleted;
                    _context.SaveChanges();

                    var factory = new MessagePayloadFactory("Gateway deleted successfully");
                    return new SuccessPayloadResult(factory.GetPayload());
                }
                catch (Exception ex)
                {
                    var factory = new ErrorPayloadFactory(new string[] { ex.Message }, "Sorry, an error ocurred");
                    return new CustomPayloadResult(factory.GetPayload(), 500);
                }
            }
        }

        // GET: gateways/5
        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            Gateway data;
            try
            {
                data = _context.Gateway
                    .Include(g => g.Peripherals)
                    .Single(g => g.Id.Equals(new Guid(id)));
            }
            catch (InvalidOperationException ex)
            {
                var errorFactory = new ErrorPayloadFactory(new string[] { "The Gateway doesn't exist.", ex.Message }, message: "Sorry, an error ocurred.");
                return new CustomPayloadResult(errorFactory.GetPayload(), 404);
            }

            var factory = new SingleObjectPayloadFactory<GatewayDto>(_mapper.GatewayToGatewayDto(data));
            return new SuccessPayloadResult(factory.GetPayload());

        }
    }
}