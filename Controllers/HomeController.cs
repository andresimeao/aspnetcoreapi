using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Api.Services;
using Api.Data;
using Api.Repositories;

namespace Api.Controllers
{
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly ApiContext _context;

        //contrutor 
        public HomeController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var auth = new AuthRepository(_context);
            var user = await auth.Login(model);
            if ( user == null)
            {
                return BadRequest(new { Success = false, Error = "username or password incorrect !" });
            }
            else
            {
                //JWT auth
                //var _token = TokenService.GenerateToken(model);
                //user.Password = "";
                return new
                {
                    Success = true,
                    User = user,
                };
            }
        }
    }
}