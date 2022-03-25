using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace GStatsFaker.Repository
{
    public class StatsFaker: IStatsFaker
    {
        public const string Repos = "Repos";
        public PowerShell PS { get; private set; }
        public Runspace runspace { get; private set; }

        public string HomePath { get => ""+Directory.GetCurrentDirectory()+"\\Repos\\"+Username+"\\"+RepoName; }

        public string Username { get; private set; } = "";
        public string RepoName { get; private set; } = "";
        public string Token { get; private set; } = "";

        public StatsFaker(string RepoName):this()
        {
            InitRep(RepoName);
        }

        public StatsFaker()
        {
            PS = PowerShell.Create();
            runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
        }

        public void InitRep(string RepoName)
        {
            PS.Commands.Clear();
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

        public void SetUpCredentials(string Email, string UUsername)
        {
            PS.Commands.Clear();
            PS.AddScript($"cd {HomePath}; git config user.name \"{UUsername}\";git config user.email \"{Email}\"");
            PS.Invoke();
        }

        public void AddActivity(int n = 1)
        {
            PS.Commands.Clear();
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

        public int Invite(string UUserName)
        {
            PS.Commands.Clear();
            if (InRepository(UUserName))
            {
                return -1;
            }
            else
            {
                var PL = runspace.CreatePipeline();
                PL.Commands.AddScript($".\\gh.exe api /repos/{Username}/{RepoName}/collaborators/{UUserName} --method=DELETE");
                PL.Commands.AddScript($".\\gh.exe api /repos/{Username}/{RepoName}/collaborators/{UUserName} --method=PUT");
                var R = PL.Invoke();
                return 1;
            }
        }

        public bool InRepository(string UUsername)
        {
            PS.Commands.Clear();
            bool inRepo = false;
            var PL = runspace.CreatePipeline();
            PL.Commands.AddScript($".\\gh.exe api /repos/{Username}/{RepoName}/collaborators/{UUsername}");
            var R = PL.Invoke();
            if (R.Count == 0)
            {
                inRepo = true;
            }
            return inRepo;

        }

        public void Delete()
        {
            PS.Commands.Clear();
            PS.AddScript($"del {HomePath}");
            RepoName = string.Empty;
            PS.Invoke();
        }

        public void Rename(string Reponame)
        {
            PS.Commands.Clear();
            PS.AddScript($".\\gh.exe repo rename {Reponame} -R https://github.com/{Config.GAccountName}/{this.RepoName}");
            PS.Invoke();
        }
    }
}
