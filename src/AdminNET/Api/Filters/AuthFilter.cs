using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using AdminNET.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace AdminNET.Api.Filters
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuthFilter> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthFilter(ILogger<AuthFilter> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("This is a secret phrase");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "unique_name").Value;

                // attach user to context on successful jwt validation
                var user = await _userManager.FindByIdAsync(userId);
                context.HttpContext.Items["User"] = user;
            }
            catch
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