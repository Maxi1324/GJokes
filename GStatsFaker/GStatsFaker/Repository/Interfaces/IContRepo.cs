using GStatsFaker.Model;

namespace GStatsFaker.Repository.Interfaces
{
    public interface IContRepo
    {
        public GenerateJokeResult GenerateJokes(User user, IContManager Manager, int num);

        public GenerateJokeResult GenerateJokes(User user, IContManager Manager, string StartDate, string EndDate, int MinCont, int MaxCont);


    }

    public enum GenerateJokeResult
    {
        Success = 1,
        ZuVieleConts = -1,
        WenigerAls0Conts = -2,
        DateRangeNotSet = -3
    }
}
