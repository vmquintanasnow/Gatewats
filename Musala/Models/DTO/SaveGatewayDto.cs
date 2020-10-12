using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Models.DTO
{
    public class SaveGatewayDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b", ErrorMessage = "Dirección Ipv4 incorrecta")]
        public string Ipv4 { get; set; }

        public string ValueToAvoidDuplicate()
        {
            return Name + Ipv4;
        }
    }
}
