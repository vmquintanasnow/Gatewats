using Musala.Business.Payload.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Musala.Business.Payload
{
    public interface IPayload
    {
        public object Data { get; }

        public string ToString => JsonConvert.SerializeObject(this);
    }

    public class CollectionPayload<T> : IPayload
    {
        public object Data { get => _data; }

        private readonly List<T> _data;

        public Pagination Pagination { get; set; }
        public PaginationLinks Links { get; set; }

        public CollectionPayload(List<T> data)
        {
            _data = data;
        }
    }

    public class SingleObjectPayload<T> : IPayload
    {
        public object Data { get => _data; }

        private readonly T _data;

        public SingleObjectPayload(T data)
        {
            _data = data;
        }
    }

    public class ErrorPayload : IPayload
    {
        [JsonProperty(PropertyName ="error")]
        public object Data { get => _data; }

        private readonly object _data;

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public ErrorPayload(object data,string message)
        {
            _data = data;
            Message = message;
        }
    }

}