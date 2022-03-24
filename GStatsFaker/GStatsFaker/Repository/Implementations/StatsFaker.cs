using System.Management.Automation;

namespace GStatsFaker.Repository
{
    public class StatsFaker: IStatsFaker
    {
        public const string Repos = "Repos";
        public PowerShell PS { get; private set; }

        public string HomePath { get => ""+Directory.GetCurrentDirectory()+"\\Repos\\"+Username+"\\"+RepoName; }

        public string Username { get; private set; } = "";
        public string RepoName { get; private set; } = "";
        public string Token { get; private set; } = "";

        public StatsFaker(string RepoName)
        {
            PS = PowerShell.Create();
            InitRep(RepoName);
        }

        public StatsFaker()
        {
            PS = PowerShell.Create();
        }

        public void InitRep(string RepoName)
        {
            this.Username = Config.GAccountName;
            this.RepoName = RepoName;
            this.Token = Config.GToken;

            PS.AddScript($"cd {HomePath};.\\gh.exe repo create {RepoName} --private");
            PS.Invoke();

            string s = "cd " + Directory.GetCurrentDirectory();
            PS.AddScript("(cd " + Directory.GetCurrentDirectory() + $");(mkdir Repos)");
            PS.AddScript("(cd " + Directory.GetCurrentDirectory()+$"\\Repos);(mkdir " + $"{Username})");
            PS.AddScript($"(cd {Directory.GetCurrentDirectory()+ "\\Repos\\" + Username+"\\"});(git clone https://{Token}@github.com/{Username}/{RepoName}.git)");
            PS.Invoke();
        }

        public void SetUpCredentials(string Email)
        {
            PS.AddScript($"cd {HomePath}; git config user.name \"{Username}\";git config user.email \"{Email}\"");
            PS.Invoke();
        }

        public void AddActivity(int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                int r = new Random().Next(2000000);
                string file = $"{ HomePath }\\{ r}.txt";
                string s = "cd " + HomePath;
                PS.AddScript($"{s};echo JALOL > {file}");
                PS.AddScript($"git add \"{file}\"");
                PS.AddScript($"{s};git commit -m {r}");
                PS.AddScript($"{s};git push https://{Token}@github.com/{Username}/{RepoName}.git");
                PS.Invoke();
            }
        }

        public void Invite(string UserName)
        {
            PS.AddScript($".\\gh.exe api /repos/{Username}/{RepoName}/collaborators/{Username} --method=PUT");
            PS.Invoke();
        }

        public bool CheckIfInvited(string Username)
        {
            PS.AddCommand($"$i = .\\gh.exe api /repos/Dingsi1/Dummy/collaborators/Maxasdasd;$i");
            var str = PS.Invoke();
            var dings = str[0];
            string s1 = str.ToString();

            return false;

        }

        public void SetUpCredentials(string Email, string Username)
        {
            throw new NotImplementedException();
        }
    }
}
