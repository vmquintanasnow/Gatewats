using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Musala.Business.Payload
{
    public class PayloadResult : IActionResult
    {
        public IPayload Payload { get; set; }
        public HttpStatusCode StatusCode {get;set;}

        public PayloadResult(IPayload payload, HttpStatusCode statusCode)
        {
            Payload = payload;
            StatusCode = statusCode;
        }

        public IPayload GetPayloadForTest()
        {
            return Payload;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {

            var objectResult = new ObjectResult(Payload)
            {
                StatusCode = (int)StatusCode,
            };

            await objectResult.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
}