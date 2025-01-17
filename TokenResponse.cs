   public class TokenResponse
   {
       public string AccessToken { get; set; }
       public string RefreshToken { get; set; }
   }

   public class RefreshTokenRequest
   {
       public string RefreshToken { get; set; }
   }
   