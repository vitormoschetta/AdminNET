using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminNET.Api.Filters
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuthFilter> _logger;

        public AuthFilter(ILogger<AuthFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                var reponseUnnauthorized = JsonSerializer.Serialize(new { message = "Unauthorized" });
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(reponseUnnauthorized);
                return;
            }

            await next();
        }
    }
}