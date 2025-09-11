using DocTask.Core.Dtos.Authentications;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;

namespace DocTask.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthenticationService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto request)
    {
        var foundUser = await _userRepository.GetByUserNameAsync(request.Username);
        if (foundUser == null || !BCrypt.Net.BCrypt.Verify(request.Password, foundUser.Password))
            throw new BadRequestException("Username or password is incorrect");

        foundUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var updatedUser = await _userRepository.UpdateRefreshToken(foundUser,  _jwtService.GenerateRefreshToken(foundUser));
        
        return new LoginResponseDto
        {
            AccessToken = _jwtService.GenerateAccessToken(foundUser),
            RefreshToken =  updatedUser.Refreshtoken
        };
    }
}