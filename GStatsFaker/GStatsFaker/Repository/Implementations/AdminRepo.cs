using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.Repository.Implementations
{
    public class AdminRepo:IAdminRepo
    {
        public GSFContext Context { get; private set; }
        public IAccountRepo CRepo { get; private set; }
        public IConfigRepo ConfigRepo { get; private set; }

        public AdminRepo(GSFContext Context, IAccountRepo CP, IConfigRepo Config)
        {
            this.Context = Context;
            CRepo = CP;
            ConfigRepo = Config;
        }

        public int BlockPerson(BlockUser UI)
        {
            if (CheckAdminPassword(UI.Password)) return -1;

            User? u = Context.Users.SingleOrDefault((u) => u.Id == UI.UserId);
            if (u != null)
            {
                if (Context.BlockList.Any(b => b.Email == u.Email))
                {
                    return -3;
                }
                Blocked b = new Blocked() { BlockTime = DateTime.Now, Email = u.Email };
                Context.BlockList.Add(b);
                Context.SaveChanges();
                return 1;
            }
            return -2;
        }

        public int UnblockPerson(BlockUser UI)
        {
            if (CheckAdminPassword(UI.Password)) return -1;
            User? u = Context.Users.SingleOrDefault((u) => u.Id == UI.UserId);
            if (u != null)
            {
                Blocked? b = Context.BlockList.SingleOrDefault(b => b.Email == u.Email) ?? default!;
                if (b == null)
                {
                    return -3;
                }
                Context.BlockList.Remove(b);
                Context.SaveChanges();
                return 1;
            }
            return -2;
        }

        public List<HoleUserInfos> GetUsers(int Page, OrderBy OB, Filter F, string password)
        {
            if (CheckAdminPassword(password)) return null;

            List<User> u = Context.Users.Include(u => u.EmalVerifikations).Include(u=>u.ConSettings).ToList();

            switch (OB)
            {
                case OrderBy.Joined:
                    u = u.OrderBy(u => u.Created).ToList();
                    break;
                case OrderBy.JoinedDesc:
                    u.OrderByDescending(u => u.Created).ToList();
                    break;
                case OrderBy.Email:
                    u.OrderBy(u => u.Email).ToList();
                    break;
                case OrderBy.Id:
                    u.OrderBy(u => u.Id).ToList();
                    break;
            }

            List<Blocked> b = Context.BlockList.ToList();

            int iStart = Page * Config.PageSize;

            List<HoleUserInfos> uList = new List<HoleUserInfos>();
            int APS = 0;
            for (int i = iStart; i < u.Count && i < iStart + Config.PageSize + APS; i++)
            {

                User u1 = u[i];
                HoleUserInfos HUI = new HoleUserInfos();

                bool Blocked = b.Any(b => b.Email == u1.Email);
                bool Verified = u1.EmalVerifikations.Any(e => e.IsVerifiziert);

                if (F == Filter.All || (F == Filter.Authenticated && Verified) || (F == Filter.NotAuthenticated && !Verified) || (F == Filter.Blocked && Blocked) || (F == Filter.NotBlocked && !Blocked))
                {
                    HUI.UserId = u1.Id;
                    HUI.RealEmail = u1.Email;
                    HUI.Blocked = Blocked;
                    HUI.Verified = Verified;
                    HUI.ConfigInfos = ConfigRepo.GetUserConfigData(u1);
                    uList.Add(HUI);
                }
                else
                {
                    APS++;
                }

            }
            return uList;
        }

        public bool CheckAdminPassword(string password)
        {
            return password != Config.AdminPassword;
        }

    }
}
