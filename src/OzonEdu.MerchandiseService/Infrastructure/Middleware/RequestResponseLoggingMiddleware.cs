using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace OzonEdu.MerchandiseService.Infrastructure.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType == "application/grpc")
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.ToString().Contains("/swagger/"))
            {
                await _next(context);
                return;
            }

            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();

                await using var requestStream = _recyclableMemoryStreamManager.GetStream();
                await context.Request.Body.CopyToAsync(requestStream);

                _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                       $"Schema: {context.Request.Scheme}{Environment.NewLine}" +
                                       $"Host: {context.Request.Host}{Environment.NewLine}" +
                                       $"Path: {context.Request.Path}{Environment.NewLine}" +
                                       $"QueryString: {context.Request.QueryString}{Environment.NewLine}" +
                                       $"Request Headers: {ConvertHeadersToString(context.Response.Headers)}{Environment.NewLine}" +
                                       $"Request Body: {ReadStreamInChunks(requestStream)}");
                context.Request.Body.Position = 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Logger ERROR! Could not log HTTP request");
            }
        }
        private async Task LogResponse(HttpContext context)
        {
            try
            {
                var originalBodyStream = context.Response.Body;

                await using var responseBody = _recyclableMemoryStreamManager.GetStream();
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"Http Response Information:{Environment.NewLine}"+
                                       $"Schema: {context.Request.Scheme}{Environment.NewLine}" +
                                       $"Host: {context.Request.Host}{Environment.NewLine}" +
                                       $"Path: {context.Request.Path}{Environment.NewLine}" +
                                       $"QueryString: {context.Request.QueryString}{Environment.NewLine}" +
                                       $"Response Headers: {ConvertHeadersToString(context.Response.Headers)}{Environment.NewLine}" +
                                       $"Response Body: {text}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Logger ERROR! Could not log HTTP response");
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private static string ConvertHeadersToString(IHeaderDictionary headers)
        {
            var resultHeadersStr = string.Empty;
            foreach (var hkv in headers)
            {
                if(string.IsNullOrWhiteSpace(hkv.Value) )
                    continue;
                resultHeadersStr += hkv.Key + " = " + hkv.Value + "; ";
            }
            return resultHeadersStr;
        }
    }
}
