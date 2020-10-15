using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Musala.Business.Payload;
using System.Net;

namespace Musala.Business.Exceptions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            appError.Run(async context =>
           {
               context.Response.ContentType = "application/json";
               var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
               if (contextFeature != null)
               {
                   context.Response.StatusCode = contextFeature is ICustomException ?
                                                    (int)HttpStatusCode.BadRequest :
                                                    (int)HttpStatusCode.InternalServerError;
                   var factory = new ErrorPayloadFactory(new string[] { contextFeature.Error.Message }, "Internal Server Error.");
                   await context.Response.WriteAsync(factory.GetPayload().ToString)
                   .ConfigureAwait(false);
               }
           }));
        }
    }
}
