namespace GStatsFaker.Repository.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        public string Authenticate(string username, string password);
    }
}
