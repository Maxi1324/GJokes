using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Linq;
using System;

namespace GStatsFaker.Repository
{
    public class StatsFaker : IStatsFaker
    {
        public const string Repos = "Repos";
        public PowerShell PS { get; private set; }
        public Runspace runspace { get; private set; }

        public string HomePath { get => "" + Directory.GetCurrentDirectory() + "\\Repos\\" + Username + "\\" + RepoName; }

        public string Username { get; private set; } = "";
        public string Email { get; private set; } = "";
        public string RepoName { get; private set; } = "";
        public string Token { get; private set; } = "";

        private static List<string> Jokes = default!;

        private Random rand = new Random();

        public StatsFaker(string RepoName) : this()
        {
            InitRep(RepoName);
        }

        public StatsFaker()
        {
            if(Jokes == null)
            {
                LoadJokes();
            }

            PS = PowerShell.Create();
            runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
        }

        public void LoadJokes()
        {
            Jokes = new List<string>();
            string[] lines = File.ReadAllLines(Config.JokesPath);
            foreach (string line in lines)
            {
                Jokes.Add(line.Replace("<>","\n"));
            }
        }

        public void InitRep(string RepoName)
        {
            PS.Commands.Clear();
            this.Username = Config.GAccountName;
            this.RepoName = RepoName;
            this.Token = Config.GToken;

            string s5 = $"cd {Directory.GetCurrentDirectory()};.\\gh.exe repo create {RepoName} --private";
            PS.AddScript(s5);
            PS.Invoke();

            string s = "cd " + Directory.GetCurrentDirectory();
            PS.AddScript("(cd " + Directory.GetCurrentDirectory() + $");(mkdir Repos)");
            PS.AddScript("(cd " + Directory.GetCurrentDirectory() + $"\\Repos);(mkdir " + $"{Username})");

            string s6 = $"(cd {Directory.GetCurrentDirectory() + "\\Repos\\" + Username + "\\"});(git clone https://{Token}@github.com/{Username}/{RepoName}.git)";
            PS.AddScript($"(cd {Directory.GetCurrentDirectory() + "\\Repos\\" + Username + "\\"});(git clone https://{Token}@github.com/{Username}/{RepoName}.git)");
            PS.Invoke();
        }

        public void SetUpCredentials(string Email, string UUsername)
        {
            PS.Commands.Clear();
            PS.AddScript($"cd {HomePath}; git config user.name \"{UUsername}\";git config user.email \"{Email}\"");
            this.Email = Email;
            PS.Invoke();
        }

        public void AddActivity(int n = 1, string AddToCommit = "", bool directPush = true)
        {
            string s = "cd " + HomePath;
            int randi = rand.Next(9999);
            for (int i = 0; i < n; i++)
            {
                string Joke = Jokes[rand.Next(Jokes.Count)];
                string file = $"{DateTime.Now.ToShortDateString()}-{i}-{randi}.txt";
                PS.AddScript($"{s};echo \"{Joke}\" > {file}");
                
                PS.AddScript($"git add '{file}'");
                PS.AddScript($"{s};git commit -m AddedAFunnyJoke " + AddToCommit);
            }
            if (directPush)
            {
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
            PL.Commands.AddScript($"cd {Directory.GetCurrentDirectory()};.\\gh.exe api /repos/{Username}/{RepoName}/collaborators/{UUsername}");
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

        public void AddActivityPast(int LetztenTage, int offset, int MinRange, int MaxRange)
        {
            Random rand = new Random();
            var FCC = ArrayFetchCommitCount();
            DateTimeOffset today = System.DateTime.Now.AddDays(-offset);
            for (int i = offset; i < LetztenTage; i++)
            {
                DateTimeOffset yesterday = today.AddDays(-1);
                if (CountActivity(FCC, yesterday) < 15)
                {
                    //float rand = new Random().Next(30)+35;
                    //int count = (int)Math.Pow(3,rand*0.1);
                    
                    int count = rand.Next(MinRange, MaxRange);

                    AddActivity(count, $" --date '{yesterday}'", false);
                }
                if (i % 30 == 0 || LetztenTage-1 == i)
                {
                    string s = "cd " + HomePath;
                    PS.AddScript($"{s};git push https://{Token}@github.com/{Username}/{RepoName}.git");
                    PS.Invoke();
                }
                today = yesterday;
            }
        }

        public void AddActivityPast(DateTime SD, DateTime ED, int MinCont, int MaxCont)
        {
            if (ED < SD)
            {
                DateTime ED1 = ED;
                ED = SD;
                SD = ED1;
            }
            int totalDays = (int)Math.Ceiling((DateTime.Now- SD).TotalDays);
            int offset = (int)Math.Ceiling((DateTime.Now-ED).TotalDays);

            AddActivityPast(totalDays-1, offset-2, MinCont, MaxCont);
        }

        public int CountActivity(System.Collections.ObjectModel.Collection<PSObject>? FCC, DateTimeOffset DTO)
        {
            string D = DTO.ToString("u").Split(" ")[0];
            if (FCC != null)
            {
                string h = FCC[0].ToString();
                return FCC.Where(f => f.ToString().Split(" ")[0] == D && f.ToString().Split(" ")[1] == Email).Count();
            }
            else
            {
                return int.MaxValue;
            }
        }

        public System.Collections.ObjectModel.Collection<PSObject>? ArrayFetchCommitCount()
        {
            try
            {
                PS.Commands.Clear();
                var PL = runspace.CreatePipeline();
                string s = "cd " + HomePath;
                PL.Commands.AddScript($"{s}; git log --date=short --pretty=format:'%ad %ce'");
                System.Collections.ObjectModel.Collection<PSObject>? R = PL.Invoke();
                return R;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void GenContText(string text)
        {
            var myBitmap = new System.Drawing.Bitmap(@"input.png");

            for (int x = 0; x < myBitmap.Width; x++)
                for (int y = 0; y < myBitmap.Height; y++)
                {
                   // Color pixelColor = myBitmap.GetPixel(x, y);
                    // things we do with pixelColor
                }
        }
    }
}
