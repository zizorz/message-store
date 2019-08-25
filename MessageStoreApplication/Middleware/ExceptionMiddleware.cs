using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageStoreApplication.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MessageStoreApplication.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ForbiddenException)
            {
                context.Response.StatusCode = 403;
            }
            catch (NotFoundException)
            {
                context.Response.StatusCode = 404;
            }
        }
    }
}