namespace Webapi.TokenOperations.Models
{
    public class TokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; }
        public string SecurityKey { get; set; }
    }
}