using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musala.Domain
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
