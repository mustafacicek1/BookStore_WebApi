using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Webapi.DbOperations;
using Webapi.TokenOperations;
using Webapi.TokenOperations.Models;

namespace Webapi.Application.UserOperations.Commands.RefreshToken
{
    public class RefreshTokenCommand
    {
        public string RefreshToken { get; set; }
        private readonly IBookStoreDbContext _context;
        private readonly IConfiguration _configuration;

        public RefreshTokenCommand(IBookStoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Token Handle()
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshToken == RefreshToken && u.RefreshTokenExpireDate > DateTime.Now);
            if (user is null)
                throw new InvalidOperationException("Valid bir Refresh Token bulunamadı!");

            TokenHandler handler = new TokenHandler(_configuration);
            Token token = handler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = token.Expiration.AddMinutes(5);
            _context.SaveChanges();
            return token;
        }
    }
}