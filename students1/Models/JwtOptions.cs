namespace students1.Models
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
        public int ExpirationSeconds { get; set; }
        public string Secret { get; set; }
        public string AdminSecret { get; set; }
    }


}
