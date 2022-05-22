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

        public List<HoleUserInfos> GetUsers(int Page, OrderBy OB, Filter F,string search, string password)
        {
            if (CheckAdminPassword(password)) return null;

            var Abfrage = Context.Users.Include(u => u.EmalVerifikations)
                .Include(u => u.ConSettings)
                .Include(u => u.EmalVerifikations)
                .Where(u =>
                F == Filter.All || (
                F == Filter.Authenticated && u.EmalVerifikations.Any(e => e.IsVerifiziert)) || (
                F == Filter.NotAuthenticated && !u.EmalVerifikations.Any(e => e.IsVerifiziert)) || (
                F == Filter.Blocked && Context.BlockList.Any(b1 => b1.Email == u.Email)) || (
                F == Filter.NotBlocked && !Context.BlockList.Any(b1 => b1.Email == u.Email)))
                ;

            //var Search = Abfrage.OrderBy(u => search == "-"?0 :Context.LevenshteinDistance(search,u.Email));

            List<User> u = Abfrage.ToList();
            var Search = u.OrderBy(u => search == "-" ? 0 : LevenshteinDistance(search, u.Email));
                 
            switch (OB)
            {
                case OrderBy.Joined:
                    Search = Search.ThenBy(u => u.Created);
                    break;
                case OrderBy.JoinedDesc:
                    Search = Search.ThenByDescending(u => u.Created);
                    break;
                case OrderBy.Email:
                    Search = Search.ThenBy(u => u.Email);
                    break;
                case OrderBy.Id:
                    Search = Search.ThenBy(u => u.Id);
                    break;
            }

            u = Search
                .Skip(Config.PageSize * Page)
                .Take(Config.PageSize)
                .ToList();

            List<HoleUserInfos> holeUsers = new List<HoleUserInfos>();
            foreach(User u1 in u)
            {
                HoleUserInfos HUI = new HoleUserInfos();

                bool Blocked = Context.BlockList.Any(b => b.Email == u1.Email);
                bool Verified = u1.EmalVerifikations.Any(e => e.IsVerifiziert);

                HUI.UserId = u1.Id;
                HUI.RealEmail = u1.Email;
                HUI.Blocked = Blocked;
                HUI.Verified = Verified;
                HUI.ConfigInfos = ConfigRepo.GetUserConfigData(u1);

                holeUsers.Add(HUI);
            }
            return holeUsers;
        }

        /// <summary>
        /// copyd from https://gist.github.com/Davidblkx/e12ab0bb2aff7fd8072632b396538560
        /// </summary>
        [DbFunction("CodeFirstDatabaseSchema", "LevenshteinDistance")]
        public int LevenshteinDistance(string source1, string source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[source1Length, source2Length];
        }

        public bool CheckAdminPassword(string password)
        {
            return password != Config.AdminPassword;
        }
    }
}
