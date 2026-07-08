using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Logout;
using AIKnowledge.Application.Features.Auth.Me;
using AIKnowledge.Application.Features.Auth.RefreshToken;
using AIKnowledge.Application.Features.Auth.Register;
using AIKnowledge.Application.Interfaces;
using AIKnowledge.Domain.Entities;

namespace AIKnowledge.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IAuditRepository _auditRepository;

    public AuthService(
        IAuthRepository repository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IAuditRepository auditRepository)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _auditRepository = auditRepository;
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

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = userId,
            Action = "REGISTER",
            TableName = "Users",
            RecordId = userId,
            Description = "New user registered.",
            IPAddress = ""
        });

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

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = user.UserId,
            Action = "LOGIN",
            TableName = "Users",
            RecordId = user.UserId,
            Description = "User logged in.",
            IPAddress = ""
        });

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

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var storedToken = await _repository.GetRefreshTokenAsync(request.RefreshToken);

        if (storedToken == null)
            throw new Exception("Invalid refresh token.");

        if (storedToken.IsRevoked)
            throw new Exception("Refresh token has been revoked.");

        if (storedToken.ExpiryDate < DateTime.UtcNow)
            throw new Exception("Refresh token has expired.");

        var user = await _repository.GetUserByIdAsync(storedToken.UserId);

        if (user == null)
            throw new Exception("User not found.");

        string role = await _repository.GetRoleNameAsync(user.RoleId) ?? "User";

        // Old refresh token revoke
        storedToken.IsRevoked = true;
        await _repository.UpdateRefreshTokenAsync(storedToken);

        // New JWT
        string newJwt = _jwtService.GenerateToken(
            user.UserId,
            user.Email,
            role);

        // New Refresh Token
        string newRefreshToken = _jwtService.GenerateRefreshToken();

        await _repository.SaveRefreshTokenAsync(new RefreshToken
        {
            UserId = user.UserId,
            Token = newRefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        });

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = user.UserId,
            Action = "REFRESH_TOKEN",
            TableName = "RefreshTokens",
            RecordId = storedToken.RefreshTokenId,
            Description = "Generated new JWT using refresh token.",
            IPAddress = ""
        });

        return new RefreshTokenResponse
        {
            Token = newJwt,
            RefreshToken = newRefreshToken,
            Expiry = DateTime.UtcNow.AddMinutes(60)
        };
    }
    public async Task<LogoutResponse> LogoutAsync(LogoutRequest request)
    {
        var token = await _repository.GetRefreshTokenAsync(request.RefreshToken);

        if (token == null)
            throw new Exception("Invalid Refresh Token.");

        if (token.IsRevoked)
            throw new Exception("Already Logged Out.");

        token.IsRevoked = true;

        await _repository.UpdateRefreshTokenAsync(token);

        await _auditRepository.AddAsync(new AuditLog
        {
            UserId = token.UserId,
            Action = "LOGOUT",
            TableName = "RefreshTokens",
            RecordId = token.RefreshTokenId,
            Description = "User logged out successfully.",
            IPAddress = ""
        });

        return new LogoutResponse
        {
            Success = true,
            Message = "Logout successful."
        };
    }
}