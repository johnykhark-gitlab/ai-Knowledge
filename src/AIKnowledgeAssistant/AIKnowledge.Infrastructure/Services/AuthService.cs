using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Me;
using AIKnowledge.Application.Features.Auth.Register;
using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IAuthRepository repository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _repository.EmailExistsAsync(request.Email))
            throw new Exception("Email already exists.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber,
            RoleId = 2
        };

        int userId = await _repository.RegisterUserAsync(user);

        return new RegisterResponse
        {
            UserId = userId,
            Message = "User registered successfully."
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Invalid email or password.");

        bool validPassword = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!validPassword)
            throw new Exception("Invalid email or password.");

        string role = await _repository.GetRoleNameAsync(user.RoleId) ?? "User";

        await _repository.UpdateLastLoginAsync(user.UserId);


        string token = _jwtService.GenerateToken(
            user.UserId,
            user.Email,
            role);

        string refreshToken = _jwtService.GenerateRefreshToken();
        await _repository.SaveRefreshTokenAsync(
            new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

        return new LoginResponse
        {
            UserId = user.UserId,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Role = role,
            Token = token,
            RefreshToken = refreshToken,
            Expiry = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<MeResponse> GetCurrentUserAsync(int userId)
    {
        var user = await _repository.GetUserByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        string role = await _repository.GetRoleNameAsync(user.RoleId) ?? "";

        return new MeResponse
        {
            UserId = user.UserId,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Role = role
        };
    }
}