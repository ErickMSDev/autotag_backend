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
        AuthenticateResponse Authenticate(AutoTagContext _context, LoginRequest model);
        AuthenticateResponse Authenticate(AutoTagContext _context, int accountId);
        string RequestRegister(AutoTagContext _context, RegisterRequest model);
        AuthenticateResponse Register(AutoTagContext _context, AccountRequest model);
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

        public AuthenticateResponse Authenticate(AutoTagContext _context, LoginRequest model)
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

        public string RequestRegister(AutoTagContext _context, RegisterRequest model)
        {
            Account account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            // return null if user not found
            if (account != null) return "La cuenta ya existe";

            Role role = _context.Roles.SingleOrDefault(x => x.Code == "user");

            // return null if role not found
            if (role == null) return "No se encontró el rol user";

            string domain = model.Email.Split('@')[1];

            bool domainIsInBlacklist = _context.DomainBlacklists.Any(d => d.DomainName == domain);

            if (domainIsInBlacklist) return "No está permitido usar el dominio " + domain;

            string code = Guid.NewGuid().ToString().Replace("-", string.Empty) + Guid.NewGuid().ToString().Replace("-", string.Empty);

            // Crear AccountRequest
            AccountRequest accountRequest = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Date = DateTime.Today,
                Code = code
            };
            _context.AccountRequests.Add(accountRequest);
            _context.SaveChanges();

            // Enviar mail de confirmación de mail
            EmailService.SendEmail(new UserEmailOptions() {
                ToEmails = new List<string>() { accountRequest.Email },
                Subject = "Confirmación de Correo",
                Template = "confirmacion_email",
                Params = new Dictionary<string, string>()
                {
                    ["accountFirstName"] = accountRequest.FirstName,
                    ["token"] = accountRequest.Code
                }
            });

            return null;
        }

        public AuthenticateResponse Register(AutoTagContext _context, AccountRequest model)
        {
            Account account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            // return null if user not found
            if (account != null) return null;

            Role role = _context.Roles.SingleOrDefault(x => x.Code == "user");

            // return null if role not found
            if (role == null) throw new Exception("No se encontró el rol user");


            Account newAccount = new Account()
            {
                RoleId = role.Id,
                Email = model.Email.ToLower(),
                Password = model.Password,
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Enabled = true
            };
            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            // authentication successful so generate jwt token
            var token = generateJwtToken(newAccount);

            // eliminar AccountRequests
            var accountRequets = _context.AccountRequests.Where(ar => ar.Email == newAccount.Email).ToList();
            _context.AccountRequests.RemoveRange(accountRequets);
            _context.SaveChanges();

            return new AuthenticateResponse(newAccount, token);
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