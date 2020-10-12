using Musala.Models;
using Musala.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Resources.Mapper
{
    public class MusalaMapper : IMapper
    {
        public  Gateway SaveGatewayDtoToGateway(SaveGatewayDto saveGatewayDto)
        {
            return new Gateway()
            {
                Ipv4 = saveGatewayDto.Ipv4,
                Name = saveGatewayDto.Name,
                Peripherals = new List<Peripheral>()
            };
        }

        public GatewayDto GatewayToGatewayDto(Gateway gateway)
        {
            return new GatewayDto()
            {
                Ipv4 = gateway.Ipv4,
                Name = gateway.Name,
                Peripherals = (from peripheral in gateway.Peripherals select PeripheralToPeripheralDto(peripheral)).ToList()
            };
        }

        public Peripheral SavePeripheralDtoToPeripheral( SavePeripheralDto savePeripheralDto)
        {
            return new Peripheral()
            {
                Vendor= savePeripheralDto.Vendor,
                Status= savePeripheralDto.Status,
                DateCreation= savePeripheralDto.DateCreation,
                GatewayId= new Guid(savePeripheralDto.GatewayId)
            };
        }

        public PeripheralDto PeripheralToPeripheralDto(Peripheral peripheral)
        {
            return new PeripheralDto()
            {
                Vendor = peripheral.Vendor,
                Status = peripheral.Status,
                DateCreation = peripheral.DateCreation,
                GatewayId = peripheral.GatewayId.ToString(),
            };
        }
    }
}
