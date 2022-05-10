using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Webapi.DbOperations;
using Webapi.TokenOperations;
using Webapi.TokenOperations.Models;

namespace Webapi.Application.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommand
    {
        public UserLoginModel Model { get; set; }
        private readonly IBookStoreDbContext _context;
        private readonly IConfiguration _configuration;

        public CreateTokenCommand(IBookStoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Token Handle()
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == Model.Email && u.Password == Model.Password);
            if (user is null)
                throw new InvalidOperationException("Email veya şifre hatalı!");

            TokenHandler handler = new TokenHandler(_configuration);
            Token token = handler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireDate = token.Expiration.AddMinutes(5);
            _context.SaveChanges();
            return token;
        }
    }
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}