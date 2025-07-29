using LiveChat.DTOs.auth;

namespace LiveChat.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDTO> Login(LoginDTO login);
        Task Register(RegisterDTO register);
        // Task VerifyEmail(string token);
        // Task ForgotPassword(string email);
        // Task RefreshToken(string token)
        // Task Logout();
    }
}
