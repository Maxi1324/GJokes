namespace GStatsFaker.Model
{
    public class ConSettings
    {
        public int Id { get; set; }

        private int _MinCon;

        public int MinCon
        {
            get { return _MinCon; }
            set { _MinCon = (value >= 0 && value <= Config.MaxCont) ? value : throw new ArgumentException("Inpute Fehlerhaft"); }
        }

        private int _MaxCon;

        public int MaxCon
        {
            get { return _MaxCon; }
            set { _MaxCon = (value >= 0 && value <= Config.MaxCont) ? value : throw new ArgumentException("Inpute Fehlerhaft"); }
        }

        public string GithubEmail { get; set; } = string.Empty;
        public string GithubUsername { get; set; } = string.Empty;

        public string RepoName { get; set; } = "Dummy";


        public User User { get; set; } = default!;
        public int UserId { get; set; } = default!;

    }
}
