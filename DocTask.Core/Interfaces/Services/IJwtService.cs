using DocTask.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace DocTask.Core.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    SecurityToken ValidateAccessToken(string token);
    SecurityToken ValidateRefreshToken(string token);
}