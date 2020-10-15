using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Musala.Domain.Entity
{
    public class Gateway
    {
        public Gateway()
        {
            Devices = new HashSet<Device>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ipv4 { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Device> Devices { get; set; }

    }
}
