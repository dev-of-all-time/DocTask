using System.IdentityModel.Tokens.Jwt;
using DocTask.Core.Dtos.Authentications;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Data.Repositories;

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

    public async Task Logout(string accessToken, string refreshToken)
    {
        var jwtToken = (JwtSecurityToken)_jwtService.ValidateAccessToken(accessToken);
        _jwtService.ValidateRefreshToken(refreshToken);
        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user?.Refreshtoken == null || !user.Refreshtoken.Equals(refreshToken))
            throw new UnauthorizedException("Invalid token");
        await _userRepository.UpdateRefreshToken(user, null);
    }

    public async Task<RefreshResponseDto> RefreshToken(string refreshToken)
    {
        var jwtToken = (JwtSecurityToken)_jwtService.ValidateRefreshToken(refreshToken);
        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
        var user = await _userRepository.GetByUserNameAsync(username);
        if (user?.Refreshtoken == null || !user.Refreshtoken.Equals(refreshToken))
            throw new UnauthorizedException("Invalid token");
        
        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken(user);
        
        var updatedUser = await _userRepository.UpdateRefreshToken(user, newRefreshToken);
        
        return new RefreshResponseDto(newAccessToken, updatedUser.Refreshtoken);
    }
}