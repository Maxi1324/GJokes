using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GStatsFaker.Controllers
{
    public class AdminController
    {

        public GSFContext Context { get; private set; }
        public AdminController(GSFContext Context)
        {
            this.Context = Context;
        } 

        [EnableCors()]
        [HttpPost("BlockPerson")]
        public object BlockPerson(UserInfo UI)
        {
            User? u = Context.Users.SingleOrDefault((u)=> u.Id == UI.UserId);
            if(u != null)
            {
            }
            return  -1;
        }
    }
}
