using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseService.Infrastructure.Middleware
{
    public class HealthCheckLiveMiddleware
    {
        public HealthCheckLiveMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //todo: проверить работу HealthCheck`ов
            context.Response.StatusCode = StatusCodes.Status200OK;
        }
    }
}

