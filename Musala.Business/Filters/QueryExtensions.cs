using StringToExpression.LanguageDefinitions;
using System;
using System.Linq;

namespace Musala.Business.Filters
{
    public static class QueryExtensions
    {
        /// <summary>
        /// </summary>
        /// <exception cref="FormatException"/>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parentQuery"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> parentQuery, QueryObject query, out int itemAmount)
        {
            

            if (query == null)
            {
                itemAmount = parentQuery.Count();
                return parentQuery;
            }

            var responseQuery = parentQuery
                .ApplyFilter(query.Filter,out int itemAmountAfterFilter)
                .ApplyOrder(query.Sort)
                .ApplySkip(query.Skip, query.Take);

            itemAmount = itemAmountAfterFilter;
            return responseQuery;
        }

        public static IQueryable<T> ApplyOrder<T>(this IQueryable<T> query, string order)
        {
            if (string.IsNullOrEmpty(order))
            {
                return query;
            }

            try
            {
                var compiledOrdering = OrderByParser.Parse(order);
                return compiledOrdering.Apply(query);
            }
            catch (Exception e)
            {
                throw new FormatException($"Provided sort expression '{order}' has incorrect format", e);
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="FormatException"/>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string filter, out int itemAmount)
        {
            
            if (string.IsNullOrEmpty(filter))
            {
                itemAmount = query.Count();
                return query;
            }

            try
            {
                var compiledFilter = new ODataFilterLanguage().Parse<T>(filter);
                var iQueryableResult =query.Where(compiledFilter);
                itemAmount = iQueryableResult.Count();
                return iQueryableResult;
            }
            catch (Exception e)
            {
                throw new FormatException($"Provided filter expression '{filter}' has incorrect format", e);
            }
        }

        public static IQueryable<T> ApplySkip<T>(this IQueryable<T> query, int? skip, int? take)
            => query
                .SkipIf(skip.HasValue, skip.GetValueOrDefault())
                .TakeIf(take.HasValue, take.GetValueOrDefault());

        private static IQueryable<T> SkipIf<T>(this IQueryable<T> query, bool predicate, int skip)
            => predicate ? query.Skip(skip) : query;

        private static IQueryable<T> TakeIf<T>(this IQueryable<T> query, bool predicate, int skip)
            => predicate ? query.Take(skip) : query;
    }
}