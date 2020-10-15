using System;
using System.ComponentModel.DataAnnotations;

namespace Musala.Business.DTO
{
    public class SaveDeviceDto
    {      
        public string Vendor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateCreation { get; set; }

        [Required]
        public bool? Status { get; set; }

        [Required]
        public string GatewayId { get; set; }

        public string ValueToAvoidDuplicate()
        {
            return Vendor + GatewayId;
        }
    }
}
