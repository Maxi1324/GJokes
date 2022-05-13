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
    public class ContController : ControllerBase
    {
        public IContRepo contRepo { get; private set; }
        
        public IConfigRepo configRepo { get; private set; }
        public IContManager contManager { get; private set; }
        public ContController(IContRepo CR, IConfigRepo CR1, IContManager contManager)
        {
            contRepo = CR;
            configRepo = CR1;
            this.contManager = contManager;
        }

        [HttpGet("GenerateContToday")]
        public Response GenerateJokesNow(int num)
        {
            User u = configRepo.FindUser(User);
            GenerateJokeResult GJR = contRepo.GenerateJokes(u, contManager, num);
            switch (GJR)
            {
                case GenerateJokeResult.Success:
                    return new Response((int)GJR, num+" Jokes were generated");
                case GenerateJokeResult.ZuVieleConts:
                    return new Response((int)GJR,"You can't generate that many Jokes. The max amout is "+Config.MaxCont);
                case GenerateJokeResult.WenigerAls0Conts:
                    return new Response((int)GJR, "You can't generate <= 0 Jokes");
                default:
                    throw new Exception("Internal Server error");
            }
        }

        [HttpGet("GenerateContsPast")]
        public Response GenerateContsPast(string StartDate = "", string EndDate = "", int MinCont = 0, int MaxCont = 0)
        {
            User u = configRepo.FindUser(User);
            GenerateJokeResult GJR = contRepo.GenerateJokes(u, contManager, StartDate, EndDate, MinCont, MaxCont);
            switch (GJR)
            {
                case GenerateJokeResult.Success:
                    return new Response((int)GJR,"Conts created");
                case GenerateJokeResult.ZuVieleConts:
                    return new Response((int)GJR, "Can't generate more than "+Config.MaxCont+" each day");
                case GenerateJokeResult.WenigerAls0Conts:
                    return new Response((int)GJR, "You can't generate less than zero Jokes each day");
                case GenerateJokeResult.DateRangeNotSet:
                    return new Response((int)GJR, "You must set a Daterange");
                default:
                    throw new Exception("Internal Server error");
            }
        }
    }
}
