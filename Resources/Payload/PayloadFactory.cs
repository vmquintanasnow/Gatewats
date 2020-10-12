using Musala.Resources.Filters;
using Musala.Resources.Payload.Helpers;
using System;
using System.Collections.Generic;

namespace Musala.Resources.Payload
{
    public interface IPayloadFactory
    {
        IPayload GetPayload();
    }

    public class CollectionPayloadFactory<T> : IPayloadFactory
    {
        public QueryObject Query { get; set; }
        public List<T> Data { get; }

        public CollectionPayloadFactory(List<T> data, QueryObject queryObject)
        {
            Data = data;
            Query = queryObject;
        }

        public IPayload GetPayload()
        {
            Pagination pagination = new Pagination();
            PaginationLinks links = new PaginationLinks();
            if (Query != null)
            {
                pagination = new Pagination()
                {
                    CurrentPage = Query.Skip ?? 1 / Query.Take ?? 1,
                    PageSize = Query.Take ?? Data.Count
                };

                links = new PaginationLinks()
                {
                    Next = Query.ToString(),
                    Previous = Query.ToString()
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

    public class MessagePayloadFactory : IPayloadFactory
    {
        private string Data { get; }

        public MessagePayloadFactory(string data)
        {
            Data = data;
        }

        public IPayload GetPayload()
        {
            return new MessagePayload(data: Data);

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
