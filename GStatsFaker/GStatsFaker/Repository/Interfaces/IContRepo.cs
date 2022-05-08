using GStatsFaker.Model;

namespace GStatsFaker.Repository.Interfaces
{
    public interface IContRepo
    {
        public GenerateJokeResult GenerateJokes(User user, IContManager Manager, int num);
    }

    public enum GenerateJokeResult
    {
        Success = 1,
        ZuVieleConts = -1,
        WenigerAls0Conts = -2
    }
}
