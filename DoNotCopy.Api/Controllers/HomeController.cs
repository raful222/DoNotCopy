using DoNotCopy.Api.Dto;
using DoNotCopy.Api.Services;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly EncryptionService _hashService;

        public HomeController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            EncryptionService hashService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _hashService = hashService;
        }


        // התחברות
        [HttpPost("LoginIsValid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login(LoginModel loginModel)
        {
            var password = _hashService.HashPassword(loginModel.Password);
            var anyLucturer = _context.Lecturers.Where(x => x.Email == loginModel.UserName && x.PasswordHash == password).FirstOrDefault();
            if (anyLucturer != null)
            {
                return Ok(anyLucturer.Id);
            }
            return Ok(0);
        }

        [HttpGet("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult test()
        {
            var password = _hashService.HashPassword("123456");
            var passwordhash = _hashService.VerifyHashedPassword(password,"123456");

            return Ok();
        }



    }
}
