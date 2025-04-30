using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevTaskTracker.Application.Interfaces;
using DevTaskTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser user)
    {
        // Step 0: Create expiration time for JWT token
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"]));

        

        // Step 1: Create claims - include standard and identity-related claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),

           // Identity claims
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID (standard for NameIdentifier)
            new Claim(ClaimTypes.Name, user.UserName),                // Person Name (used in Identity))                  
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Step 2: Create the symmetric security key
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        // Step 3: Create signing credentials
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Step 4: Create the JWT token
        var jwtToken = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        // Step 5: Create the token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Step 6: Generate the final token string
        var finalToken = tokenHandler.WriteToken(jwtToken);
        return finalToken;
    }

}
