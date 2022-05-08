using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;

namespace GStatsFaker.Repository.Implementations
{
    public class ContRepo:IContRepo
    {
        public GenerateJokeResult GenerateJokes(User user, IContManager Manager, int num)
        {
            if (num > Config.MaxCont)
            {
                return GenerateJokeResult.ZuVieleConts;
            }
            if(num <= 0)
            {
                return GenerateJokeResult.WenigerAls0Conts;
            }
            
            IStatsFaker f = Manager.GetStatsFaker(user);
            f.AddActivity(num);
            return GenerateJokeResult.Success;
        }
    }

}
