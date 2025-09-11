using DocTask.Core.Dtos.Authentications;

namespace DocTask.Core.Interfaces.Services;

public interface IAuthenticationService
{
    Task<LoginResponseDto> Login(LoginRequestDto request);
}