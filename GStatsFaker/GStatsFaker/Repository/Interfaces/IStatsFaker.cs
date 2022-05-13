using System.Management.Automation;

namespace GStatsFaker.Repository
{
    public interface IStatsFaker
    {
        public PowerShell PS { get;   }

        public string HomePath { get; }

        public string Username { get;  }
        public string RepoName { get;   }
        public string Token { get;  }

        public void InitRep(string RepoName);

        public void SetUpCredentials(string Email, string Username);

        public void AddActivityPast(DateTime SD, DateTime ED, int MinCont, int MaxCont);
        public void AddActivityPast(int LetztenTage, int offset, int MinRange, int MaxRange);
        public void AddActivity(int n = 1, string AddToCommit = "", bool directPush = true);

        /// <summary>
        /// Invites an User if it the user is not in the Repo
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>
        /// R == 1 Alles OK
        /// R == -1 User is already in Repo
        /// </returns>
        public int Invite(string Username);

        public bool InRepository(string Username);

        public void Delete();

        public void Rename(string Reponame);
    }
}
