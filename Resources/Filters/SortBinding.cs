using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Musala.Resources.Filters
{
    public static class OrderByParser
    {
        public static OrderToken Parse(string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy)) throw new ArgumentNullException(nameof(orderBy));

            var tokens = orderBy.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(segment => new OrderToken(segment)).ToArray();

            var orderToken = tokens[0];
            _ = tokens.Skip(1).Aggregate(orderToken, (token, next) => token.Next = next);

            return orderToken;
        }
    }

    public class OrderToken
    {
        private readonly string _propertyPath;
        private readonly SortOrder _order;
        public OrderToken Next { get; set; }

        public OrderToken(string segment)
        {
            var parts = segment.Trim().Split(' ').Select(x => x.Trim()).ToArray();
            if (parts.Length < 1 || parts.Length > 2)
                throw new ArgumentException($"Segment '{segment}' has incorrect format");

            _propertyPath = parts[0];
            _order = parts.Length == 2 ? (SortOrder)Enum.Parse(typeof(SortOrder), parts[1]) : SortOrder.Asc;
        }

        public IQueryable<T> Apply<T>(IQueryable<T> query, bool firstCall = true)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var memberExpression =
                (MemberExpression)_propertyPath.Split('.').Aggregate((Expression)parameter, Expression.Property);

            var call = Expression.Call(
                typeof(Queryable), ChooseMethod(), new[]
                {
                    typeof(T),
                    ((PropertyInfo) memberExpression.Member).PropertyType
                },
                query.Expression,
                Expression.Lambda(memberExpression, parameter));

            var ordered = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(call);

            return Next?.Apply<T>(ordered, false) ?? ordered;

            string ChooseMethod()
            {
                switch (_order)
                {
                    case SortOrder.Asc: return firstCall ? "OrderBy" : "ThenBy";
                    case SortOrder.Desc: return firstCall ? "OrderByDescending" : "ThenByDescending";
                    default: return firstCall ? "OrderBy" : "ThenBy";
                }
            }
        }

        private enum SortOrder
        {
            Asc,
            Desc
        }
    }
}
