using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Interfaces.Services;
using DocTask.Data.Repositories;
using DocTask.Service.Services;

namespace DockTask.Api.Configurations;

public static class ApplicationContainer
{
    public static IServiceCollection AddApplicationContainer(this IServiceCollection services)
    {
        // Services
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IJwtService, JwtService>();
        
        //Repositories
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}