using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Models.DTO
{
    public class SavePeripheralDto
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
