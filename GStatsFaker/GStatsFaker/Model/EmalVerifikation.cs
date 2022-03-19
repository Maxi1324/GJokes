namespace GStatsFaker.Model
{
    public class EmalVerifikation
    {
        public int Id { get; set; }

        public DateTime Erstellt { get; set; }

        public bool IsVerifiziert { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;

    }
}
