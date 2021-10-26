using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);
            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            try
            {
                if (context.Request.ContentLength == null) return;
                if (context.Request.ContentLength <= 0) return;
                
                context.Request.EnableBuffering(); 
                var buffer = new byte[context.Request.ContentLength.Value];
                 
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                _logger.LogInformation("Request logged: {bodyAsText}", bodyAsText);

                context.Request.Body.Position = 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Logger ERROR! Could not log request body");
            }
        }
    }
}
