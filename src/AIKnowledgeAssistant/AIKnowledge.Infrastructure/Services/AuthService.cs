using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Register;
using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IAuthRepository repository,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        bool exists =
            await _repository.EmailExistsAsync(request.Email);

        if (exists)
            throw new Exception("Email already exists.");

        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber,
            RoleId = 2
        };

        int id =
            await _repository.RegisterUserAsync(user);

        return new RegisterResponse
        {
            UserId = id,
            Message = "Registration Successful"
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}