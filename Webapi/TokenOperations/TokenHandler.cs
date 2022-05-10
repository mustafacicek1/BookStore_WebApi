using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Webapi.Entities;
using Webapi.TokenOperations.Models;

namespace Webapi.TokenOperations
{
    public class TokenHandler
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public Token CreateAccessToken(User user)
        {
            Token tokenModel = new Token();

            tokenModel.Expiration = DateTime.Now.AddMinutes(_tokenOptions.Expiration);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: tokenModel.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenModel.AccessToken = tokenHandler.WriteToken(jwt);
            tokenModel.RefreshToken = CreateRefreshToken();

            return tokenModel;
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}