using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Musala.Resources.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Musala.Resources.Errors
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            appError.Run(async context =>
           {
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
               context.Response.ContentType = "application/json";
               var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
               if (contextFeature != null)
               {
                   var factory = new ErrorPayloadFactory(new string[] { contextFeature.Error.Message }, "Internal Server Error.");
                   await context.Response.WriteAsync(factory.GetPayload().ToString)
                   .ConfigureAwait(false);
               }
           }));
        }
    }
}
