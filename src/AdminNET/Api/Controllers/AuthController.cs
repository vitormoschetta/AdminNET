using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminNET.Api.Models;
using AdminNET.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdminNET.Api.Controllers;

[ApiController]
[Route("api/authenticate")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _key;

    public AuthController(ILogger<AuthController> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _userManager = userManager;
        _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(_key));
    }


    [HttpPost]
    public async Task<ActionResult<string>> Authenticate([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        var result = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!result)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            Id = user.Id,
            Username = user.UserName,
            Token = tokenString
        });
    }
}
