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
                    return Ok(new Respone(R, "Alles OK"));
                case (-1):
                    return UnprocessableEntity(new Respone() { Code = R, Desc = "MinCon oder MaxCon sind kleiner als 0" });

                case (-2):
                    return UnprocessableEntity(new Respone() { Code = R, Desc = "Min Con ist größergleich MaxCon" });

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
                    return Ok(new Respone(R, "Alles OK"));
                case (-1):
                    return UnprocessableEntity(new Respone() { Code = R, Desc = "Name bereitsvergeben" });
                default: throw new Exception("Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpGet("Create Repo")]
        public string CreateRepo()
        {
            Faker.InitRep("HAAAAAAALLO");
            Faker.CheckIfInvited("Maxi13254");
            return "asd";
        }
    }
    //SetRepoName
}
