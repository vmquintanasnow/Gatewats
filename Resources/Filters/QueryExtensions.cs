using StringToExpression.LanguageDefinitions;
using System;
using System.Linq;

namespace Musala.Resources.Filters
{
    public static class QueryExtensions
    {
        public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> parentQuery, QueryObject query)
        {
            if (query == null)
            {
                return parentQuery;
            }

            return parentQuery
                .ApplyFilter(query.Filter)
                .ApplyOrder(query.Sort)
                .ApplySkip(query.Skip, query.Take);
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

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return query;
            }

            try
            {
                var compiledFilter = new ODataFilterLanguage().Parse<T>(filter);
                return query.Where(compiledFilter);
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