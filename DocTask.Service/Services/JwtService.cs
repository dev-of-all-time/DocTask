using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Services;
using DocTask.Core.Models;
using DocTask.Service.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocTask.Service.Services;

public class JwtSetting
{
    public string AccessSecretKey { get; set; }
    public string RefreshSecretKey { get; set; }
    public string AccessTokenExpiry {get; set;}
    public string RefreshTokenExpiry {get; set;}
    public string Issuer {get; set;}
    public string Audience {get; set;}
}       

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;

    public JwtService(IOptions<JwtSetting> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
    }

    public string GenerateAccessToken(User user)
    {
        double expiration = TimeConverter.ConvertToMilliseconds(_jwtSetting.AccessTokenExpiry);
        return GenerateToken(user, _jwtSetting.AccessSecretKey, expiration);
    }

    public string GenerateRefreshToken(User user)
    { 
        double expiration = TimeConverter.ConvertToMilliseconds(_jwtSetting.RefreshTokenExpiry);
        return GenerateToken(user, _jwtSetting.RefreshSecretKey, expiration);
    }

    public SecurityToken ValidateAccessToken(string token)
    {
        return ValidateToken(_jwtSetting.AccessSecretKey, token);
    }

    public SecurityToken ValidateRefreshToken(string token)
    {
        return ValidateToken(_jwtSetting.RefreshSecretKey, token);
    }

    public string GenerateRefreshToken(User user, string secretKey, DateTime expiration)
    {
        throw new NotImplementedException();
    }

    public string GenerateToken(User user, string secretKey, double expiration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddMilliseconds(expiration),
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    

    public SecurityToken ValidateToken(string secretKey, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSetting.Issuer,
                ValidAudience = _jwtSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            
            return validatedToken;
        }
        catch
        {
            throw new UnauthorizedException("Invalid token");
        }
    }
}