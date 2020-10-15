using System;

namespace Musala.Business.DTO
{
    /// <summary>
    /// This class allow to select what data show, and simplify validation while create object
    /// </summary>
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Vendor { get; set; }

        public DateTime? DateCreation { get; set; }

        public bool? Status { get; set; }

        public string GatewayId { get; set; }
       
    }
}
