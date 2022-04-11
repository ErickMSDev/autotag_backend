using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTagBackEnd.Entities;
using AutoTagBackEnd.Helpers;
using AutoTagBackEnd.Models;
using AutoTagBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTagBackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : AppController
    {
        private readonly AutoTagContext _context;
        private IUserService _userService;

        public AuthController(AutoTagContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest body)
        {
            var response = _userService.Authenticate(_context, body);

            if (response == null)
                return Ok(new { error = new[] { new { type = "password", message = "Email o constraseña incorrecta" } } });

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AccessToken()
        {
            var response = _userService.Authenticate(_context, this.CurrentAccount.Id);

            if (response == null)
                return Ok(new { error = new[] { new { type = "password", message = "Email o constraseña incorrecta" } } });

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterRequest body)
        {
            var response = _userService.RequestRegister(_context, body);

            if (response != null)
                return Ok(new { error = new[] { new { type = "email", message = response } } });

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult ConfirmEmail([FromBody] ConfirmEmailRequest body)
        {
            DateTime minDateAllowed = DateTime.Now.AddHours(-3);
            AccountRequest accountRequest = _context.AccountRequests.Where(ar => ar.Code == body.Token && ar.CreationDate >= minDateAllowed).FirstOrDefault();

            if (accountRequest == null)
                return Ok(new { error = new[] { new { type = "email", message = "El link no es válido" } } });

            var response = _userService.Register(_context, accountRequest);

            if (response == null)
                return Ok(new { error = new[] { new { type = "email", message = "La cuenta ya existe" } } });

            return Ok(response);
        }
    }
}