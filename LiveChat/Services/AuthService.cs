using LiveChat.DTOs.auth;
using LiveChat.Entities;
using LiveChat.Services.Interfaces;

namespace LiveChat.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserService userService, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenDTO> Login(LoginDTO login)
        {
            User? user = await _userService.GetByEmail(login.Email);
            if (user == null || !_passwordHasher.VerifyPassword(login.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }
            string accessToken = await _tokenService.GenerateAccessToken(user);
            RefreshToken refreshToken = _tokenService.GenerateRefreshToken(user);
            TokenDTO token = new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
            await _tokenService.SaveRefreshToken(refreshToken);
            return token;

        }

        public async Task Register(RegisterDTO register)
        {
            User? user = await _userService.GetByEmail(register.Email);
            if (user != null) throw new InvalidOperationException("El email ya está en uso.");
            string hashedPassword = _passwordHasher.HashPassword(register.Password);
            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = register.Email,
                Username = register.Username,
                Password = hashedPassword,
                Role = "User"
            };
            await _userService.Create(newUser);
        }
    }
}
