namespace LiveChat.DTOs.auth
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
