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
using Microsoft.AspNetCore.Cors;

namespace GStatsFaker.Controllers
{
    public class AdminController
    {
        public IAdminRepo AR { get; private set; }

        public AdminController(IAdminRepo AC)
        {
            AR = AC;
        } 

        [HttpPost("BlockPerson")]
        public object BlockPerson(BlockUser UI)
        {
            int r = AR.BlockPerson(UI);
            switch (r)
            {
                case -1:
                    return ( new Response() { Code = r, Desc = "The Admin password is not correct" });
                case -2:
                    return ( new Response() { Code = r, Desc = "There is no User with this Id" });
                case -3:
                    return ( new Response() { Code = r, Desc = "The User is already blocked" });
                case 1:
                    return new Response() { Code = r, Desc = "User has been blocked" };
                default:
                    throw new Exception("Internal Server Error");
            }
        }


        [HttpPost("UnblockPerson")]
        public object UnblockPerson(BlockUser UI)
        {
            int r = AR.UnblockPerson(UI);
            switch (r)
            {
                case -1:
                    return (new Response() { Code = r, Desc = "The Admin password is not correct" });
                case -2:
                    return (new Response() { Code = r, Desc = "There is no User with this Id" });
                case -3:
                    return (new Response() { Code = r, Desc = "The User is not blocked" });
                case 1:
                    return new Response() { Code = r, Desc = "User has been unblocked" };
                default:
                    throw new Exception("Internal Server Error");
            }
        }

        [HttpPost("UnblockPerson")]
        public object GetUsers(int Page, OrderBy OB, Filter F, string password)
        {
            return AR.GetUsers(Page, OB, F, password);
        }
    }
}
