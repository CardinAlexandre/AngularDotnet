using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class VerifyTokenController : ControllerBase
{

    private readonly IConfiguration Configuration;

    public VerifyTokenController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [HttpGet]
    public bool ValidateToken([FromQuery] string token)
    {

        string secretKey = Configuration["secretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey);

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keys = Encoding.ASCII.GetBytes(secretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keys),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(5)
            }, out var validatedToken);

            return validatedToken != null;
        }
        catch
        {
            return false;
        }
    }
}
