using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.App.Filters
{
    public class RequestDurationMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestDurationMiddleware(RequestDelegate next)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            await _next.Invoke(context);
            watch.Stop();
            var str = string.Format("DURATION: {0} - {1}", context.Request.Path, watch.ElapsedMilliseconds);
            Console.WriteLine(str);
        }
    }

    public static class RequestDurationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestDurationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestDurationMiddleware>();
        }
    }
}
