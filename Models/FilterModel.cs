using System;
using System.ComponentModel.DataAnnotations;

namespace Musala.Models
{
    public class FilterModel
    {
        [Required]
        [Range(1,double.MaxValue )]
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

        public FilterModel()
        {
            this.Limit = 100;
            this.Page = 1;
        }
    }
}
