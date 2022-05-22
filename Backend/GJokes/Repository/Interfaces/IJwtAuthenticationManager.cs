namespace GStatsFaker.Repository.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        public string? Authenticate(string Email, string password);

        public string GenToken(string Email);
    }
}
