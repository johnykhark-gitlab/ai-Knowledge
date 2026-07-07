using AIKnowledge.Application.Interfaces;
using AIKnowledge.Infrastructure.Data.Connection;
using AIKnowledge.Infrastructure.Data.Repositories;
using AIKnowledge.Infrastructure.Security;
using AIKnowledge.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIKnowledge.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddScoped<IAuthRepository, AuthRepository>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}

