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

        public ConfigController(IConfigRepo Config, IStatsFaker Faker)
        {
            this.Config = Config;
            this.Faker = Faker;
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
                    return Ok(new Response(R, "Alles OK"));
                case (-1):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "MinCon oder MaxCon sind kleiner als 0 oder grpßer als Max Contributions" });

                case (-2):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "Min Con ist größergleich MaxCon" });

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
                    return Ok(new Response(R, "Alles OK"));
                case (-1):
                    return UnprocessableEntity(new Response() { Code = R, Desc = "EmailNotValid" });
                case -2:
                    return UnprocessableEntity(new Response() { Code = R, Desc = "No Username was entered" });
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
                    return new Response(R, "AllesOK");

                case (-1):
                    return new Response(R, "Github Email must be set");

                case (-2):
                    return new Response(R, "Github Username must is not set");
                case -3:
                    return new Response(R, "User already in Repo");
                default: throw new Exception("Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpGet("GenCons")]
        public Response GenerateConts()
        {
            Faker.InitRep("Dummy");
           //Faker.SetUpCredentials("adrian.schauer@aon.at", "LuckForce");
           Faker.SetUpCredentials("maxi1234567890fischer@gmail.com", "Maxi1324");
            Faker.AddActivity(20);
           // Config.CreateCont(5, Config.FindUser(User));
            return new Response(1, "Alles OK");
        }
    }
    //SetRepoName mehr testen
    //Sichtbarmachen

}
