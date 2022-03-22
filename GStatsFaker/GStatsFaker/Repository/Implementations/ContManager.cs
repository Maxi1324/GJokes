using GStatsFaker.DBContexts;
using GStatsFaker.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.Repository.Implementations
{
    public class ContManager:IContManager
    {
        public Dictionary<string, IStatsFaker> Fakers = new Dictionary<string, IStatsFaker>();
        public IServiceScopeFactory Factory { get; set; }
        public ContManager(IServiceScopeFactory scopeFactory)
        {
            Factory = scopeFactory;
            CallMathodeAtSpecificTime(CreateCons, "12:00:00");
        }

        /// <summary>
        /// Copyed Methode from https://stackoverflow.com/questions/39202158/call-a-method-in-a-specific-time-of-the-day
        /// </summary>
        public void CallMathodeAtSpecificTime(Action Methode, string Time)
        {
            var DailyTime = Time;
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            //waits certan time and run the code
            Task.Delay(ts).ContinueWith((x) => Methode());
        }

        public void CreateCons()
        {
            using (var scope = Factory.CreateScope())
            {
                Random random = new Random();
                GSFContext Context = scope.ServiceProvider.GetService<GSFContext>() ?? default!;
                if (Context == null) throw new Exception("Funktioniert nicht");
                    Context.Users.AsEnumerable().ToList().ForEach(u =>
                    {
                        string Email = u.Email;
                        IStatsFaker Faker = default!;
                        if (Fakers.ContainsKey(Email))
                        {
                            Faker = Fakers[Email];
                        }
                        else
                        {
                            Faker = new StatsFaker();
                            Fakers.Add(Email, Faker);
                            Faker.InitRep(Email, Config.Token, u.RepoName);
                            Faker.SetUpCredentials(Email, Email.Split("@")[0]);
                        }
                        int Conts = random.Next(u.MaxCon - u.MinCon) + u.MinCon;
                        Faker.AddActivity(Conts);
                    });
            }
            return;
        }
    }
}
