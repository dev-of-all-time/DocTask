using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Dtos.Authentications;
using DocTask.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocTask.Api.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthenticationController : ControllerBase
{
    private IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var result = await _authenticationService.Login(request);
        return Ok(new ApiResponse<LoginResponseDto>
        {
            Data = result,
            Message = "Login success"
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromHeader] string accessToken, [FromHeader] string refreshToken)
    {
        await _authenticationService.Logout(accessToken, refreshToken);

        return Ok(new ApiResponse<object>
        {
            Message = "Logout success"
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromHeader] string refreshToken)
    {
        var result = await _authenticationService.RefreshToken(refreshToken);
        return Ok(new ApiResponse<RefreshResponseDto>
        {
            Data = result,
            Message = "Refresh token success"
        });
    }
}