using AIKnowledge.Application.Features.Auth.ChangePassword;
using AIKnowledge.Application.Features.Auth.Login;
using AIKnowledge.Application.Features.Auth.Logout;
using AIKnowledge.Application.Features.Auth.Me;
using AIKnowledge.Application.Features.Auth.RefreshToken;
using AIKnowledge.Application.Features.Auth.Register;

namespace AIKnowledge.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);

    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<MeResponse> GetCurrentUserAsync(int userId);
    Task<RefreshTokenResponse> RefreshTokenAsync(
    RefreshTokenRequest request);
    Task<LogoutResponse> LogoutAsync(LogoutRequest request);
    Task<ChangePasswordResponse> ChangePasswordAsync(int userId,ChangePasswordRequest request);
}