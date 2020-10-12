using System;

namespace Musala.Resources.Filters
{
    public class QueryObject
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

        public override string ToString()
        {
            string queryString = "";
            queryString += Skip.HasValue ? $"$skip={Skip}&" : "";
            queryString += Take.HasValue ? $"$limit={Take}&" : "";
            queryString += string.IsNullOrEmpty(Sort) ? "" : $"$orderby={Sort}&";
            queryString += string.IsNullOrEmpty(Filter) ? "" : $"$filter={Filter}";
            queryString = queryString.EndsWith("&",StringComparison.Ordinal) ? queryString[0..^1] : queryString;
            return queryString;
        }
    }
}