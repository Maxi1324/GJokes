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

        public void AddActivity(int n = 1);

        public void Invite(string Username);

        public bool CheckIfInvited(string Username);
    }
}
