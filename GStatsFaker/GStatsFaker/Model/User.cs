
namespace GStatsFaker.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        public int MinCon { get;set; }
        public int MaxCon { get;set; }
        
        public DateTime Created { get; set; }

        public List<EmalVerifikation> EmalVerifikations { get; set; } = default!;

    }
}
