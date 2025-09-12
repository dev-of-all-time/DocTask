using DocTask.Core.Dtos.Authentications;

namespace DocTask.Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<LoginResponseDto> Login(LoginRequestDto request);
    Task Logout(string accessToken, string refreshToken);
    Task<RefreshResponseDto> RefreshToken(string refreshToken);
}