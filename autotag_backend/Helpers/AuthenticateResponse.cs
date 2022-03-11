using System;
using AutoTagBackEnd.Models;

namespace AutoTagBackEnd.Helpers
{
    public class UserAuthenticateResponse
    {
        public string? Role { get; set; }
        public DataAuthenticateResponse? Data { get; set; }
    }

    public class DataAuthenticateResponse
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }

    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public UserAuthenticateResponse User { get; set; }

        public AuthenticateResponse(Account account, string token)
        {
            Token = token;
            User = new UserAuthenticateResponse
            {
                Role = account.Role.Code,
                Data = new DataAuthenticateResponse
                {
                    DisplayName = account.FirstName + " " + account.LastName,
                    Email = account.Email
                }
            };
        }
    }
}

