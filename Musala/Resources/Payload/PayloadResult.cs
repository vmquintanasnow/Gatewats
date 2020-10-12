
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Musala.Resources;

namespace Musala.Resources
{
    public abstract class PayloadResult
    {
        protected IPayload Payload { get; set; }

        public IPayload GetPayloadForTest()
        {
            return Payload;
        }
    }
   
    public class SuccessPayloadResult : PayloadResult ,IActionResult   {      

        public SuccessPayloadResult(IPayload payload)
        {
            Payload = payload;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {          
        
            var objectResult = new ObjectResult(Payload)
            {
                StatusCode=200,
            };

            await objectResult.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
    public class CreatedPayloadResult : PayloadResult, IActionResult
    {

        public CreatedPayloadResult(IPayload payload)
        {
            Payload = payload;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {

            var objectResult = new ObjectResult(Payload)
            {
                StatusCode = 201,
            };

            await objectResult.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
    public class CustomPayloadResult : PayloadResult, IActionResult
    {
        int StatusCode { get; set; }
        public CustomPayloadResult(IPayload payload,int statusCode)
        {
            Payload = payload;
            StatusCode = statusCode;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {

            var objectResult = new ObjectResult(Payload)
            {
                StatusCode = StatusCode,
            };

            await objectResult.ExecuteResultAsync(context).ConfigureAwait(false);
        }
    }
}