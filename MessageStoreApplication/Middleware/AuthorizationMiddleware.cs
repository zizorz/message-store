using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.Extensions.Primitives;

namespace MessageStoreApplication.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly string AuthorizationString = "Authorization"; 
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(AuthorizationString))
            {
                context.Response.StatusCode = 401;
                return;
            }
            var authHeader = context.Request.Headers[AuthorizationString];

            if (authHeader == StringValues.Empty)
            {
                context.Response.StatusCode = 401;
                return;
            } 
            
            await _next.Invoke(context);
        }
    }
}