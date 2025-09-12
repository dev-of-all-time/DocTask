using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DocTask.Core.DTOs.ApiResponses;
using DocTask.Core.Exceptions;
using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Data;

namespace DockTask.Api.Handlers;

public class AuthenticationHandler
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService;
    
    public AuthenticationHandler(RequestDelegate next, IJwtService jwtService)
    {
        _next = next;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        var skipPaths = new[] { "/api/v1/auth"};
        
        if (skipPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await _next(context);
            return;
        }

        try
        {
            // Kiểm tra header Authorization
            if (!context.Request.Headers.ContainsKey("Authorization"))
                throw new UnauthorizedException("Missing Authorization Header");

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedException("Invalid Authorization Header");

            var token = authHeader["Bearer ".Length..].Trim();

            var jwtToken = (JwtSecurityToken)_jwtService.ValidateAccessToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            var foundUser = await userRepository.GetByUserNameAsync(username);
            if (foundUser?.Refreshtoken == null)
                throw new UnauthorizedException("Invalid token");

            var identity = new ClaimsIdentity(jwtToken.Claims, "JwtAuth");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;

            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is BaseException baseException)
                context.Response.StatusCode = baseException.StatusCode;
            else
                context.Response.StatusCode = 500;
            
            await context.Response.WriteAsJsonAsync(new ApiResponse<object>
            {
                Success = false,
                Error = ex.Message,
            });
        }
    }
}

public static class JwtAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationHandler>();
    }
}