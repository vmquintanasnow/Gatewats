using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Models.DTO
{
    /// <summary>
    /// This class allow to select what data show, and simplify validation while create object
    /// </summary>
    public class PeripheralDto
    {
        public string Vendor { get; set; }

        public DateTime? DateCreation { get; set; }

        public bool? Status { get; set; }

        public string GatewayId { get; set; }
       
    }
}
