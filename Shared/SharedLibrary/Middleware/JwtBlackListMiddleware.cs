using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Middleware
{
    public class JwtBlacklistMiddleware
    {
        private readonly IBlackListService _blacklistService;
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next, IBlackListService blacklistService)
        {
            _next = next;
            _blacklistService = blacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null && _blacklistService.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                var message = "У вас нет доступа к этому ресурсу.";
                await context.Response.WriteAsync(message);
                return;
            }

            await _next(context);
        }
    }
}
