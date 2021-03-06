using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using GStatsFaker.Model;
using GStatsFaker.Repository;
using Microsoft.AspNetCore.Authorization;
using GStatsFaker.DBContexts;
using System;
using System.Threading;
using System.ServiceModel.Channels;
using System.Security.Claims;
using GStatsFaker.Repository.Interfaces;

namespace GStatsFaker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public IConfigRepo Config { get; set; }
        public IStatsFaker Faker;
        public GSFContext Context;

        public ConfigController(IConfigRepo Config, IStatsFaker Faker, GSFContext Context)
        {
            this.Config = Config;
            this.Faker = Faker;
            this.Context = Context;
        }

        [HttpGet("GetUserInfo")]
        public ConfigInfos GetUserInfo()
        {
            ConfigInfos Infos = Config.GetUserConfigData(Config.FindUser(User));
            return Infos;
        }

        [HttpPost("SetConRange")]
        public object SetConRange([FromBody] ConRange UInfo)
        {
            int R = Config.SetConRange(Config.FindUser(User), UInfo.MinCon, UInfo.MaxCon);
            switch (R)
            {
                case (1):
                    return Ok(new Response(R, "All the Changes were successfully saved"));
                case (-1):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Min Jokes or Max Jokes is less than 0 or greater than 50" });

                case (-2):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Min Jokes is greater than Max Jokes" });

                default:throw new Exception("Internal Server Error");
            }
        }

        [HttpPost("SetRepoName")]
        public object SetRepoName([FromBody]RepoName RepoName)
        {
            int R = Config.SetRepoName(Config.FindUser(User), RepoName.Name);
            switch (R)
            {
                case (1):
                    return Ok(new Response(R, "Alles OK"));
                case (-1):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Name bereitsvergeben" });
                default: throw new Exception("Internal Server Error");
            }
        }

       [HttpPost("SetGithubAccountSettings")]
        public object SetGAS([FromBody] GithubAccountSettings GAS)
        {
            //Überprüfen ob der Nutzer so verwendet werden kann.
           
            int R = Config.SetGAS(GAS, Config.FindUser(User));
            switch (R)
            {
                case (1):
                    return Ok(new Response(R, "All the Changes were successfully saved"));
                case (-1):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "The entered Email is not valid" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = R, Desc = "The entered Username is not valid" });
                default: throw new Exception("Internal Server Error");
            }
        }

        [HttpPost("SendInvite")]
        public Response SendInvite()
        {
            int R = Config.Invite(Config.FindUser(User));
            switch (R)
            {
                case (1):
                    return new Response(R, "The Invite was successfully sent");

                case (-1):
                    return new Response(R, "Github Email must be set");

                case (-2):
                    return new Response(R, "Github Username is not set");
                case -3:
                    return new Response(R, "User already in Repo");
                default: throw new Exception("Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpGet("SeedDb")]
        public Response SeedDb(int num = 100)
        {
            Context.Seed(num);
            return new Response() { Code = 1, Desc ="dings" };
        }
    }
}
