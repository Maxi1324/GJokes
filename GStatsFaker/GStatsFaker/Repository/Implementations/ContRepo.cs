using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using System.Globalization;

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

        public GenerateJokeResult GenerateJokes(User user,IContManager Manager, string StartDate, string EndDate, int MinCont, int  MaxCont)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            IStatsFaker Faker = Manager.GetStatsFaker(user);

            try
            {
                if(MinCont > Config.MaxCont || MaxCont > Config.MaxCont)
                {
                    return GenerateJokeResult.ZuVieleConts;
                }
                if(MinCont < 0 | MaxCont < 0)
                {
                    return GenerateJokeResult.WenigerAls0Conts;
                }
                if(MinCont > MaxCont)
                {
                    int dings = MinCont;
                    MinCont = MaxCont;
                    MaxCont = dings;
                }
                
                DateTime StartDate1 = DateTime.ParseExact(StartDate.Replace('-','/'), "dd/MM/yyyy", provider);
                DateTime EndDate1 = DateTime.ParseExact(EndDate.Replace('-', '/'), "dd/MM/yyyy", provider);

                Faker.AddActivityPast(StartDate1, EndDate1, MinCont, MaxCont);
                return GenerateJokeResult.Success;
            }
            catch (FormatException e)
            {
                return GenerateJokeResult.DateRangeNotSet;
            }
        }
    }

}
