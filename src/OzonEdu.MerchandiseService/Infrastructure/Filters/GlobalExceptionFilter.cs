using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext exptnContext)
        {
            var resultObject = new
            {
                ExceptionType = exptnContext.Exception.GetType().FullName,
                Message = exptnContext.Exception.Message
            };

            var jsonResult = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            _logger.LogError(exptnContext.Exception, "Exception caught in Global Exception Filter");

            exptnContext.Result = jsonResult;
        }
    }
}