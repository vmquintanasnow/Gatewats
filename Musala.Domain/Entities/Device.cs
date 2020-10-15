using System;
using System.ComponentModel.DataAnnotations;

namespace Musala.Domain.Entity
{
    public  class Device
    {
        [Key]
        public int Id { get; set; }

        public string Vendor { get; set; }

        public DateTime? DateCreation { get; set; }

        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        //Foreing key
        public Guid GatewayId { get; set; }
        public virtual Gateway Gateway { get; set; }

    }
}
