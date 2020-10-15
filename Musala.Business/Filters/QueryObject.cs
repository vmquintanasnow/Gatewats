using System;

namespace Musala.Business.Filters
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
            queryString = queryString.EndsWith("&", StringComparison.Ordinal) ? queryString.Substring(0, queryString.Length - 1) : queryString;
            return queryString;
        }

        public QueryObject CopyWithBounds(int? skip, int? take)
        {
            return new QueryObject()
            {
                Filter = Filter,
                Sort = Sort,
                Skip = skip ?? Skip,
                Take = take ?? Take,

            };
        }
        public string GetNext(int total)
        {
            if (Skip.HasValue && Take.HasValue)
            {
                if (Skip + Take < total)
                {
                    return CopyWithBounds(skip: Skip + Take, take: Take).ToString();
                }
                return "";
            }
            else if (Take.HasValue && Take < total)
            {
                return CopyWithBounds(skip: Take, take: Take).ToString();
            }
            else
            {
                return "";
            }
        }
        public string GetPrevious()
        {
            if (Skip.HasValue && Take.HasValue && Skip - Take >= 0)
            {
                return CopyWithBounds(skip: Skip - Take, take: Take).ToString();
            }
            else if (Skip.HasValue && Skip > 0)
            {
                return CopyWithBounds(skip: 0, null).ToString();
            }
            else
            {
                return "";
            }
        }
    }
}