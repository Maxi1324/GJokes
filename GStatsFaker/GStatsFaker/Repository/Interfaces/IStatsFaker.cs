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

        public void InitRep(string Username, string Token, string RepoName);

        public void SetUpCredentials(string Email, string Username);

        public void AddActivity(int n = 1);
    }
}
