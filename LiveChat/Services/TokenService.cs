using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LiveChat.Entities;
using LiveChat.Repositories.Interfaces;
using LiveChat.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LiveChat.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(IConfiguration config, ITokenRepository tokenRepository)
        {
            _config = config;
            _tokenRepository = tokenRepository;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            RefreshToken token = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id,
            };
            return token;
        }

        public async Task<RefreshToken?> GetValidRefreshToken(string token)
        {
            RefreshToken? refreshToken = await _tokenRepository.GetValidToken(token);
            return refreshToken;
        }

        public async Task RevokeRefreshToken(string token)
        {
            await _tokenRepository.Revoke(token);
        }

        public async Task SaveRefreshToken(RefreshToken token)
        {
            await _tokenRepository.Create(token);
        }
    }
}
