using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Management.Automation;
using GStatsFaker.Model;
using GStatsFaker.Repository;

namespace GStatsFaker.Controllers
{
    [Route("Allgemein/[controller]")]
    [ApiController]
    public class AllgemeinController : ControllerBase
    {
        private readonly ILogger<AllgemeinController> _logger;
        private readonly IStatsFaker _SFaker;

        public AllgemeinController(ILogger<AllgemeinController> logger, IStatsFaker StatsFaker)
        {
            _logger = logger;
            _SFaker = StatsFaker;
            _SFaker.InitRep("Dingsi1", "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", "Dummy");
        }

        [HttpGet(Name = "AddEdit")]
        public string Get()
        {
            _SFaker.SetUpCredentials("maxi1234567890fischer@gmail.com", "Maxi1324");
            _SFaker.AddActivity(20);
            return "";
        }
    }
}
