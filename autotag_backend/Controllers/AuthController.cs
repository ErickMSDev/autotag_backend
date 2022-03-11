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
        public IActionResult Login([FromBody] AuthenticateRequest body)
        {
            var response = _userService.Authenticate(_context, body);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AccessToken()
        {
            var response = _userService.Authenticate(_context, this.CurrentAccount.Id);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] AuthenticateRequest body)
        {
            var response = _userService.Authenticate(_context, body);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}