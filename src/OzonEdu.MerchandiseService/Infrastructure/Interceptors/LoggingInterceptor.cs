using System;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogRequest(request);

            var response = base.UnaryServerHandler(request, context, continuation);

            LogResponse(response);

            return response;
        }

        private void LogRequest<TRequest>(TRequest request)
        {
            try
            {
                var requestJson = JsonSerializer.Serialize(request);
                _logger.LogInformation(requestJson);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Logger ERROR! Could not log GRPC request");
            }
        }

        private void LogResponse<TResponse>(TResponse response)
        {
            try
            {
                var responseJson = JsonSerializer.Serialize(response);
                _logger.LogInformation(responseJson);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Logger ERROR! Could not log GRPC response");
            }
        }
    }
}