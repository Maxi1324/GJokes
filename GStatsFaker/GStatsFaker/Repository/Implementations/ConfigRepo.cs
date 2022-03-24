using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using System.Security.Claims;

namespace GStatsFaker.Repository.Implementations
{
    public class ConfigRepo : IConfigRepo
    {
        public GSFContext Context { get; set; }
        public IContManager ContManager { get; set; }

        public ConfigRepo(GSFContext Context, IContManager ContManager)
        {
            this.Context = Context;
            this.ContManager = ContManager;
        }

        public User FindUser(ClaimsPrincipal User)
        {
            string? Email = User.FindFirst(ClaimTypes.Email)?.Value;
            User u = Context.Users.FirstOrDefault(u => u.Email == Email)??throw new Exception("User not Found");
            return u;
        }

        public ConfigInfos GetUserConfigData(User User)
        {
            ConfigInfos configInfos = new ConfigInfos() { Erstellung = User.Created, MaxCon = User.MaxCon, MinCon = User.MinCon, RepoName = User.RepoName,GithubEmail = User.GithubEmail,GithubUsername = User.GithubUsername };
            return configInfos;
        }

        public int SetConRange(User User,int Min, int Max)
        {
            if(Min >= Max)
            {
                return -2;
            }
            try
            {
                User.MinCon = Min;
                User.MaxCon = Max;
                Context.SaveChanges();
            }
            catch
            {
                return -1;   
            }
            return 1;
        }

        public int SetRepoName(User User,string repoName)
        {
            repoName = NormString(repoName);
            if (Context.Users.Any(u=>u.RepoName == repoName)|| repoName == string.Empty) return -1;
            User.RepoName = repoName;
            IStatsFaker F = ContManager.GetStatsFaker(User);
            F.Rename(repoName);
            F.Delete();
            F.InitRep(repoName);
            F.AddActivity(3);
            Context.SaveChanges();
            return 1;
        }

        public static string NormString(string str1)
        {

            string str = "";
            for (int i = 0; i < str1.Length; i++)
            {
                if (char.IsLetterOrDigit(str1[i]))
                {
                    str += str1[i];
                }
            }
            return str;
        }

        public int SetGAS(GithubAccountSettings GAS, User user)
        {
            if (!AccountRepo.IsValidEmail(GAS.GithubEmail))
            {
                return -1;
            }
            if(GAS.GithubUserName == string.Empty)
            {
                return -2;
            }
            user.GithubEmail = GAS.GithubEmail;
            user.GithubUsername = GAS.GithubUserName;
            Context.SaveChanges();
            return 1;
        }

        public int Invite(User user)
        {
            if(!AccountRepo.IsValidEmail(user.GithubEmail))return -1;
            if(user.GithubUsername == string.Empty)return -2;
            IStatsFaker Faker = ContManager.GetStatsFaker(user);
            Faker.Invite(user.GithubUsername);
            return 1;
        }
    }
}
