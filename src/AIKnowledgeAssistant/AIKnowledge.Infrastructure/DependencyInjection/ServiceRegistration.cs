using AIKnowledge.Application.Interfaces;
using AIKnowledge.Infrastructure.Data.Connection;
using AIKnowledge.Infrastructure.Data.Repositories;
using AIKnowledge.Infrastructure.Security;
using AIKnowledge.Infrastructure.Services;
using AIKnowledge.Infrastructure.Storage;
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

        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IStorageService, LocalStorageService>();
        services.AddScoped<IDocumentService, DocumentService>();


        return services;
    }
}

