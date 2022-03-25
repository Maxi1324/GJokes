using GStatsFaker.Model;
using GStatsFaker.Repository;
using GStatsFaker.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerShell.Commands;
using System.Net;

namespace GStatsFaker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IJwtAuthenticationManager AuthenticationManager { get; set; }
        public IAccountRepo AccountRepo { get; set; }
        public IConfigRepo Config { get; set; }

        public AccountController(IJwtAuthenticationManager AuthenticationManager, IAccountRepo AccountRepo, IConfigRepo Config)
        {
            this.AuthenticationManager = AuthenticationManager;
            this.AccountRepo = AccountRepo;
            this.Config = Config;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            string? token = AuthenticationManager.Authenticate(userCred.Email, userCred.Password);
            if (token != null) return Ok(token);
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("CreateAccount")]
        public object Registrieren([FromBody] UserCred userCred)
        {
            int r = AccountRepo.CreateAccount(userCred.Email, userCred.Password);
            switch (r)
            {
                case -1:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Email is already in use" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Email is not valid" });
                case -3:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Password is to short, must be longer than 5 chars" });
                case -4:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Body is wrong, Email or Password is null" });
                default:
                    return Ok(new Response() { Code = r, Desc = "Alles ok returned UserID" });
            }
        }

        [AllowAnonymous]
        [HttpPost("SendEmailVerification")]
        public object SendEmailVerification([FromBody] UserInfo UInfo)
        {
            int r = AccountRepo.SendEmailVerfikation(UInfo.UserId);
            switch (r)
            {
                case -1:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "User not found" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "To many requests. Email is blocked" });
                default:
                    return Ok(new Response() { Code = r, Desc = "Email has been sent" });
            }
        }

        [AllowAnonymous]
        [HttpPost("ActivateAccount")]
        public object ActivateAccount([FromBody] ActivateAccountInfo UInfo)
        {
            int r = AccountRepo.ActivateAccount(UInfo.UserId, UInfo.Code);
            switch (r)
            {
                case 1:
                    return Ok(new Response() { Code = r, Desc = "Account has been Activated Repository" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Code invalid" });
                case -3:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Code expired" });
                default:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "User does not exist" });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetUserIdFromMail")]
        public Response GetUserIdFromMail(string Mail)
        {
            int r = AccountRepo.FindUserId(Mail);

            if(r >= 0)
            {
                return new Response() { Code = r, Desc = "User ID" };
            }
            else
            {
                return new Response() { Code = r, Desc = "There is no User with this Email" };
            }
        }

        [HttpPost("DeleteAccount")]
        public object DeleteUser()
        {
            int R = AccountRepo.DeleteAccount(Config.FindUser(User).Id);
            switch (R)
            {
                case 1:
                    return Ok(new Response() { Code = R, Desc = "Account has been Deleted" });
                case -1:
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Nutzer exestiert nicht, was sehr komisch ist!" });
                default: throw new Exception("Internal Server Error");
            }
        }

        //Passwort vergessen
    }
}