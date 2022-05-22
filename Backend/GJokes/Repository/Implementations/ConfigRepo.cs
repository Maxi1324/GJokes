using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
            User u = Context.Users.Include(u=> u.ConSettings).FirstOrDefault(u => u.Email == Email)??throw new Exception("User not Found");
            return u;
        }

        public ConfigInfos GetUserConfigData(User User)
        {
            ConfigInfos configInfos = new ConfigInfos() { Erstellung = User.Created, MaxCon = User.ConSettings.MaxCon, MinCon = User.ConSettings.MinCon, RepoName = User.ConSettings.RepoName,GithubEmail = User.ConSettings.GithubEmail,GithubUsername = User.ConSettings.GithubUsername };
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
                User.ConSettings.MinCon = Min;
                User.ConSettings.MaxCon = Max;
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
            if (Context.Users.Any(u=>u.ConSettings.RepoName == repoName)|| repoName == string.Empty) return -1;
            User.ConSettings.RepoName = repoName;
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
            user.ConSettings.GithubEmail = GAS.GithubEmail;
            user.ConSettings.GithubUsername = GAS.GithubUserName;
            Context.SaveChanges();
            return 1;
        }

        public int Invite(User user)
        {
            if(!AccountRepo.IsValidEmail(user.ConSettings.GithubEmail))return -1;
            if(user.ConSettings.GithubUsername == string.Empty)return -2;
            IStatsFaker Faker = ContManager.GetStatsFaker(user);
            int R = Faker.Invite(user.ConSettings.GithubUsername);
            if (R == -1) return -3;
            return 1;
        }

        public void CreateCont(int num, User user)
        {
            ContManager.GetStatsFaker(user).AddActivity(num);
        }
    }
}
