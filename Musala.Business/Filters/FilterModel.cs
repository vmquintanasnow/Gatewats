using System;
using System.ComponentModel.DataAnnotations;

namespace Musala.Business.Filters
{
    public class FilterModel
    {
        [Required]
        [Range(1, int.MaxValue)]
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
