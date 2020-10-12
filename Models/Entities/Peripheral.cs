using Musala.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Musala.Models
{
    public partial class Peripheral
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
