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
        private readonly string _key;

        public AuthFilter(ILogger<AuthFilter> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(_key));
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
                var key = Encoding.ASCII.GetBytes(_key);
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
                var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user?.Email != userEmail)
                {
                    var reponseUnnauthorized = JsonSerializer.Serialize(new { message = "Unauthorized" });
                    context.HttpContext.Response.StatusCode = 401;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(reponseUnnauthorized);
                    return;
                }

                var roles = await _userManager.GetRolesAsync(user);

                user.Roles = roles.Select(x => new ApplicationRole { Name = x }).ToList();

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