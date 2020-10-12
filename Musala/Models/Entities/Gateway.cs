using Musala.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Musala.Models
{
    public partial class Gateway
    {
        public Gateway()
        {
            Peripherals = new HashSet<Peripheral>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ipv4 { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Peripheral> Peripherals { get; set; }

    }

}
