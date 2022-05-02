using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;

namespace GStatsFaker.Repository.Implementations
{
    public class AdminRepo
    {
        public GSFContext Context { get; private set; }
        public IAccountRepo CRepo { get; private set; }

        public AdminRepo(GSFContext Context,IAccountRepo CP)
        {
            this.Context = Context;
            CRepo = CP;
        }

        public object BlockPerson(BlockUser UI)
        {
            if (CheckAdminPassword(UI.Password)) return -1;

            User? u = Context.Users.SingleOrDefault((u) => u.Id == UI.UserId);
            if (u != null)
            {
                CRepo.
            }
            return -2;
        }


        public bool CheckAdminPassword(string password)
        {
            return password == Config.AdminPassword;
        }

    }
}
