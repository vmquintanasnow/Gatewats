using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musala.Resources.Payload.Helpers
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginationLinks
    {
        public string Next { get; set; }
        public string Previous { get; set; }
    }
}
