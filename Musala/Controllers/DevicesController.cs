using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Musala.Business.Filters;
using Musala.Business.Payload;
using Musala.Business.Services;
using Musala.Business.DTO;
using Musala.Domain.Entity;
using Musala.DAL;
using AutoMapper;
using System.Net;


namespace Musala.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly MusalaContext _context;
        private readonly IMapper _mapper;
        private readonly IDeviceService _service;

        public DevicesController(MusalaContext context, IMapper mapper, IDeviceService service)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// This method return a list with all peripherals from DB
        /// </summary>
        /// <returns>IEnumerable</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([ModelBinder(typeof(QueryModelBinder))] QueryObject query)
        {
            var devicesPayload = await _service.GetAllDevices(query).ConfigureAwait(false);
            return new PayloadResult(devicesPayload, HttpStatusCode.OK);        
        }

        /// <summary>
        /// This method create a new Device and return the Device created.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveDevice(SaveDeviceDto saveDeviceDto)
        {
            var deviceResultPayload = await _service.SaveDevice(saveDeviceDto).ConfigureAwait(true);
            return new PayloadResult(deviceResultPayload, HttpStatusCode.OK);
        }

        // DELETE: admin/roles/5
        [HttpDelete("{id}")]
        public IActionResult Del(int id)
        {
            _service.Delete(id);
            return new PayloadResult(null, HttpStatusCode.NoContent);
        }


        // Get: admin/roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var devicePayload = await _service.FindById(id).ConfigureAwait(false);
            return new PayloadResult(devicePayload, HttpStatusCode.OK);
        }
    }
}
