using Serilog;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Art.Models;
using System.Text.Json;

namespace Art.Extensions
{
    public static class GlobalxtensionHandler
    {
        public static void Exception(this IApplicationBuilder app)//core builder verir bunu bize
            //application-nin request pipeline ni configure eden mechanism
        {
            app.UseExceptionHandler(appError =>//pipeline a exceptionu tutan, loglayan middleware artirir
            {
                appError.Run(async context =>
                {
                    await Console.Out.WriteLineAsync("dandandnadna");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";//response header ucun value-nu get/set edir
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {//errorlari ayirmaq lazimdir
                        Log.Error($"Something went wrong: {contextFeature.RouteValues}");
                        Log.Information($"Something went wrong: {contextFeature.Path}");
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = $"Internal Server Error: {contextFeature.Error}"
                        }));
                    }
                });
            });
        }
    }
}
