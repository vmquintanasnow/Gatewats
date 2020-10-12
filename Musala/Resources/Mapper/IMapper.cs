using Musala.Models;
using Musala.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Resources.Mapper
{
    public interface IMapper
    {
        Gateway SaveGatewayDtoToGateway(SaveGatewayDto saveGatewayDto);

        GatewayDto GatewayToGatewayDto(Gateway gateway);

        Peripheral SavePeripheralDtoToPeripheral(SavePeripheralDto savePeripheralDto);

        PeripheralDto PeripheralToPeripheralDto(Peripheral peripheral);
    }
}
