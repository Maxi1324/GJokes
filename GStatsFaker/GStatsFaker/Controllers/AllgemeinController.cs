using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using GStatsFaker.Model;
using GStatsFaker.Repository;
using Microsoft.AspNetCore.Authorization;
using GStatsFaker.DBContexts;

namespace GStatsFaker.Controllers
{
    [Authorize]
    [Route("Allgemein/[controller]")]
    [ApiController]
    public class AllgemeinController : ControllerBase
    {
        private readonly ILogger<AllgemeinController> _logger;
        private readonly IStatsFaker _SFaker;
        private GSFContext Context;

        public AllgemeinController(ILogger<AllgemeinController> logger, IStatsFaker StatsFaker, GSFContext Context)
        {
            _logger = logger;
            _SFaker = StatsFaker;
            this.Context = Context;
            _SFaker.InitRep("Dingsi1", "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", "Dummy");
        }
        [HttpGet(Name = "AddEdit")]
        public string Get()
        {
            Context.Users.Add(new User() { Email = "maxi123456780fischer@gmail.com1", Password = "Hallo1" });
            Context.SaveChanges();
            //_SFaker.SetUpCredentials("maxi1234567890fischer@gmail.com", "Maxi1324");
            // _SFaker.AddActivity(20);
            return "";
        }
    }
}
