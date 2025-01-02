using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Authentication_Client.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

public class JwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(Client client)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, client.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", client.role.ToString()),
            new Claim("fullname", client.Prenom + " " + client.Nom)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwtSettings.ExpirationDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    

    public ClaimsPrincipal? ExtractClaimsFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false // Set to true if you want to validate the expiration
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            // Handle exceptions if the token is invalid or cannot be parsed
            return null;
        }
    }

}


