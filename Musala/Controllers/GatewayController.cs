using System;
using Microsoft.AspNetCore.Mvc;
using Musala.Business.Filters;
using System.Threading.Tasks;
using Musala.Business.Payload;
using Musala.Business.DTO;
using Musala.Business.Services;
using System.Net;
using Musala.Business.Exceptions;

namespace Musala.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewaysController : ControllerBase
    {
        private readonly IGatewayService _service;
        public GatewaysController( IGatewayService service)
        {
            _service = service;
        }

        // GET: gateways/
        [HttpGet]
        public async Task<IActionResult> GetAll([ModelBinder(typeof(QueryModelBinder))] QueryObject query)
        {
            var response = await _service.GetAllGateways(query).ConfigureAwait(false);
            return new PayloadResult(response, HttpStatusCode.OK);
        }

        // POST: gateways/
        [HttpPost]
        public IActionResult Save(SaveGatewayDto saveGatewayDto)
        {
            var gatewayDto = _service.SaveGateway(saveGatewayDto);
            return new PayloadResult(gatewayDto, HttpStatusCode.Created);
        }

        // DELETE: gateways/5
        [HttpDelete("{id}")]
        public IActionResult Del(Guid id)
        {
            _service.Delete(id);
            return new PayloadResult(null, HttpStatusCode.NoContent);            
        }

        // GET: gateways/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {            
            var gatewayDto= await _service.FindById(id).ConfigureAwait(false);         
            return new PayloadResult(gatewayDto, HttpStatusCode.OK);
        }
    }
}