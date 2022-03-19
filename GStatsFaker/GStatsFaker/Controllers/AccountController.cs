using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GStatsFaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IJwtAuthenticationManager AuthenticationManager { get; set; }

        public AccountController(IJwtAuthenticationManager AuthenticationManager)
        {
            this.AuthenticationManager = AuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            string? token = AuthenticationManager.Authenticate(userCred.Email, userCred.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }
    }
}
