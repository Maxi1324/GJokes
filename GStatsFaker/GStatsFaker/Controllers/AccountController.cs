using GStatsFaker.Model;
using GStatsFaker.Repository;
using GStatsFaker.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerShell.Commands;
using System.Net;

namespace GStatsFaker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors()]
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
        public object Authenticate([FromBody] UserCred userCred)
        {
            string? token = AuthenticationManager.Authenticate(userCred.Email, userCred.Password);
            if (token == "lol")
            {
                return UnprocessableEntity(new Response() { Code = -2, Desc = "Your Account has't been Email verified" });
            }
            if(token == "blocked")
            {
                return UnprocessableEntity(new Response() { Code = -3, Desc = "Your Account has been blocked by an Admin" });
            }
            else
            if (token != null)
            {
                return Ok(new Response() { Code = 1, Desc = token });
            }
            else
            {
                return UnprocessableEntity(new Response() { Code = -1, Desc = "Password or Email is wrong" });
            }
        }

        [EnableCors()]
        [AllowAnonymous]
        [HttpPost("CreateAccount")]
        public object Registrieren([FromBody] UserCred userCred)
        {
            int r = AccountRepo.CreateAccount(userCred.Email, userCred.Password);
            switch (r)
            {
                case -1:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Email already in use, but not verified" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Email is not valid" });
                case -3:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Password is too short, must be longer than 4 chars" });
                case -5:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Password ist too long, must be shorter than 21" });
                case -4:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Body is wrong, Email or Password is null" });
                case -6:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "Email is already in use" });
                case -7:
                    return UnprocessableEntity(new Response() { Code = r, Desc = "This Email has been blocked by an Admin" });
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
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
                    return Ok(new Response() { Code = r, Desc = AuthenticationManager.GenToken(AccountRepo.FindUser(UInfo.UserId).Email) });
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
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
        public object DeleteUser(UserCred UC)
        {
            int R = AccountRepo.DeleteAccount(Config.FindUser(User).Id, UC.Password);
            switch (R)
            {
                case 1:
                    return Ok(new Response() { Code = R, Desc = "Account has been Deleted" });
                case -1:
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Nutzer exestiert nicht, was sehr komisch ist!" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Password is wrong" });
                default: throw new Exception("Internal Server Error");
            }
        }

        [HttpPost("ChangePassword")]
        public object ChangePassword(ChangePassword CP)
        {
            int R = AccountRepo.ChangePassword(Config.FindUser(User),CP.OldPassword,CP.NewPassword);
            switch (R)
            {
                case 1:
                    return new Response() { Code = R, Desc = "Password successfully changed" };
                case -1:
                    return new Response() { Code = R, Desc = "The old password is not correct" };
                case -4:
                    return new Response() { Code = R, Desc = "The old password is null" };
                case -3:
                    return new Response() { Code = R, Desc = "The new password is too short, must be longer than 4" };
                case -5:
                    return new Response() { Code = R, Desc = "The new password is too long, must be shorter than 21" };
                default:
                    throw new Exception("internal Server error");
            }
        }

        [HttpGet("Loggedin")]
        public bool Loggedin()
        {
            return true;
        }

        //Passwort vergessen
    }
}