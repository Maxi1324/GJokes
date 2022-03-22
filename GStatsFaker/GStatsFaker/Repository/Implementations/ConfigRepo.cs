using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using System.Security.Claims;

namespace GStatsFaker.Repository.Implementations
{
    public class ConfigRepo : IConfigRepo
    {
        public GSFContext Context { get; set; }

        public ConfigRepo(GSFContext Context)
        {
            this.Context = Context;
        }

        public User FindUser(ClaimsPrincipal User)
        {
            string? Email = User.FindFirst(ClaimTypes.Email)?.Value;
            User u = Context.Users.FirstOrDefault(u => u.Email == Email)??throw new Exception("User not Found");
            return u;
        }

        public ConfigInfos GetUserConfigData(User User)
        {
            ConfigInfos configInfos = new ConfigInfos() { Erstellung = User.Created, MaxCon = User.MaxCon, MinCon = User.MinCon, RepoName = User.RepoName };
            return configInfos;
        }

        public int SetConRange(User User,int Min, int Max)
        {
            if(Min <= Max)
            {
                return -2;
            }
            try
            {
                User.MinCon = Min;
                User.MaxCon = Max;
                Context.SaveChanges();
            }
            catch(ArgumentException ex)
            {
                return -1;   
            }
            return 1;
        }

        public int SetRepoName(User User,string repoName)
        {
            repoName = NormString(repoName);
            if (Context.Users.FirstOrDefault(u=>u.RepoName == repoName) != null || repoName == string.Empty) return -1;
            User.RepoName = repoName;
            Context.SaveChanges();
            return 1;
        }

        public static string NormString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsLetterOrDigit(str[i]))
                {
                    str += str[i];
                }
            }
            return str;
        }
    }
}
