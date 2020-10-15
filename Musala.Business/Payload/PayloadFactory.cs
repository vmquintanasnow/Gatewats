using Musala.Business.Filters;
using Musala.Business.Payload.Helpers;
using System.Collections.Generic;

namespace Musala.Business.Payload
{
    public interface IPayloadFactory
    {
        IPayload GetPayload();
    }

    public class CollectionPayloadFactory<T> : IPayloadFactory
    {
        public QueryObject Query { get; set; }
        public List<T> Data { get; }
        public int Total { get; set; }

        public CollectionPayloadFactory(List<T> data, QueryObject queryObject, int total)
        {
            Data = data;
            Query = queryObject;
            Total = total;
        }

        public IPayload GetPayload()
        {
            Pagination pagination = new Pagination();
            PaginationLinks links = new PaginationLinks();
            if (Query != null)
            {
                pagination = new Pagination()
                {
                    Skip = Query.Skip ?? 0,
                    Limit = Query.Take ?? Total,
                    Total = Total
                };

                links = new PaginationLinks()
                {                    
                    Next = Query.GetNext(Total),
                    Previous = Query.GetPrevious()
                };
            }


            CollectionPayload<T> collectionPayload = new CollectionPayload<T>(data: Data)
            {
                Links = links,
                Pagination = pagination
            };

            return collectionPayload;
        }

    }

    public class SingleObjectPayloadFactory<T> : IPayloadFactory
    {
        private T Data { get; }

        public SingleObjectPayloadFactory(T data)
        {
            Data = data;
        }

        public IPayload GetPayload()
        {
            return new SingleObjectPayload<T>(data: Data);

        }

    }

    public class ErrorPayloadFactory : IPayloadFactory
    {
        private object Data { get; }
        private string Message { get; set; }

        public ErrorPayloadFactory(object error, string message)
        {
            Data = error;
            Message = message;
        }

        public IPayload GetPayload()
        {
            return new ErrorPayload(data: Data, Message);
        }

    }
}
