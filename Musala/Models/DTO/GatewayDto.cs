using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Models.DTO
{

    /// <summary>
    /// This class allow to select what data show, and simplify validation while create object
    /// </summary>
    public class GatewayDto
    {
        public string Name { get; set; }

        public string Ipv4 { get; set; }

        public ICollection<PeripheralDto> Peripherals { get; set; }

    }
}
