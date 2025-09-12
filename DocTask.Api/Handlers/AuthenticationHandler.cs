using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DocTask.Core.Interfaces.Services;

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

    public async Task InvokeAsync(HttpContext context)
    {
        var skipPaths = new[] { "/api/v1/auth"};
        
        if (skipPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
        {
            await _next(context);
            return;
        }
        
        
        // Kiểm tra header Authorization
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Missing Authorization Header");
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid Authorization Header");
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        var jwtToken = (JwtSecurityToken) _jwtService.ValidateAccessToken(token);
        
        var identity = new ClaimsIdentity(jwtToken.Claims, "JwtAuth");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;

        await _next(context);
    }
}

public static class JwtAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationHandler>();
    }
}