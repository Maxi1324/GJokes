
namespace GStatsFaker.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        private int _MinCon;

        public int MinCon
        {
            get { return _MinCon; }
            set { _MinCon = (value >= 0 && value <= Config.MaxCont)?value:throw new ArgumentException("Inpute Fehlerhaft"); }
        }

        private int _MaxCon;

        public int MaxCon
        {
            get { return _MaxCon; }
            set { _MaxCon = (value >= 0 && value <= Config.MaxCont) ? value : throw new ArgumentException("Inpute Fehlerhaft"); }
        }

        public string GithubEmail { get; set; } = string.Empty;
        public string GithubUsername { get; set; } = string.Empty;

        public string RepoName { get; set; }= "Dummy";


        public DateTime Created { get; set; }

        public List<EmalVerifikation> EmalVerifikations { get; set; } = default!;

    }
}
