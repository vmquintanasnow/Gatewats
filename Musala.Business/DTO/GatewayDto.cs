using System.Collections.Generic;

namespace Musala.Business.DTO
{

    /// <summary>
    /// This class allow to select what data show, and simplify validation while create object
    /// </summary>
    public class GatewayDto
    {
        public GatewayDto()
        {
            Devices = new HashSet<DeviceDto>();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public string Ipv4 { get; set; }

        public ICollection<DeviceDto> Devices { get; set; }

    }
}
