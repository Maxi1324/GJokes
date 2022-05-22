namespace GStatsFaker.Model
{
    public class ConfigInfos
    {
        public int MinCon { get; set; }
        public int MaxCon { get; set; }
        public string RepoName { get; set; } = string.Empty;
        public DateTime Erstellung { get; set; }

        public string GithubEmail { get; set; } = string.Empty;
        public string GithubUsername { get; set; } = string.Empty;
    }
}