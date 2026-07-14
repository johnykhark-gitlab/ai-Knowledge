using AIKnowledge.Application.Interfaces;
using AIKnowledge.Infrastructure.AI.Interfaces;
using AIKnowledge.Infrastructure.AI.Ollama;
using AIKnowledge.Infrastructure.Data.Connection;
using AIKnowledge.Infrastructure.Data.Repositories;
using AIKnowledge.Infrastructure.DocumentProcessing.Pdf;
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
        services.AddScoped<IDocumentContentRepository, DocumentContentRepository>();
        services.AddScoped<IDocumentTextExtractor, PdfTextExtractor>();
        services.AddScoped<DocumentProcessorService>();
        services.AddHttpClient<IOllamaService, OllamaService>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}

