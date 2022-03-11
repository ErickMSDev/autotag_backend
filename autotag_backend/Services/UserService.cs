using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoTagBackEnd.Models;
using AutoTagBackEnd.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using AutoTagBackEnd.Entities;

namespace AutoTagBackEnd.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AutoTagContext _context, AuthenticateRequest model);
        AuthenticateResponse Authenticate(AutoTagContext _context, int accountId);
        IEnumerable<Account> GetAll(AutoTagContext _context);
        Account GetById(AutoTagContext _context, int id);
    }

    public class UserService : IUserService
    {
        private readonly JwtSettings _jwtSettings;

        public UserService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public AuthenticateResponse Authenticate(AutoTagContext _context, AuthenticateRequest model)
        {
            Account account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (account == null) return null;

            Role role = _context.Roles.SingleOrDefault(x => x.Id == account.RoleId);

            // return null if role not found
            if (role == null) return null;

            account.Role = role;

            // authentication successful so generate jwt token
            var token = generateJwtToken(account);

            return new AuthenticateResponse(account, token);
        }

        public AuthenticateResponse Authenticate(AutoTagContext _context, int accountId)
        {
            Account account = _context.Accounts.SingleOrDefault(x => x.Id == accountId);

            // return null if user not found
            if (account == null) return null;

            Role role = _context.Roles.SingleOrDefault(x => x.Id == account.RoleId);

            // return null if role not found
            if (role == null) return null;

            account.Role = role;

            // authentication successful so generate jwt token
            var token = generateJwtToken(account);

            return new AuthenticateResponse(account, token);
        }

        public IEnumerable<Account> GetAll(AutoTagContext _context)
        {
            return _context.Accounts.ToList();
        }

        public Account GetById(AutoTagContext _context, int id)
        {
            return _context.Accounts.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(Account account)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            if(string.IsNullOrEmpty(_jwtSettings.SecretKey))
            {
                throw new Exception("No se ha encontrado el valor de SecretKey");
            }
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}