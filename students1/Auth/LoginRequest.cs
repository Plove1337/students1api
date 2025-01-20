namespace students1.Auth
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string TwoFactorCode { get; set; }
}

    public class Tokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

}
