using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.Repository.Implementations
{
    public class AccountRepo : IAccountRepo
    {

        public GSFContext Context { get; set; } 
        public IMailManager MailManager { get; set; }
        public IContManager ContManager;

        public AccountRepo(GSFContext Context,IMailManager MailManager, IContManager ContManager)
        {
            this.Context = Context; 
            this.MailManager = MailManager;
            this.ContManager = ContManager;
        }

        public int ActivateAccount(int UserID, string code)
        {
            User? userN = FindUser(UserID,true);
            if (userN == null) return -1;
            User user = userN ?? default!;

            EmalVerifikation ? SEVN = user.EmalVerifikations.FirstOrDefault(e => e.Code == code);
            if (SEVN == null) return -2;
            EmalVerifikation SVE = SEVN ?? default!;

            if (!(DateTime.Now.Subtract(SVE.Erstellt).TotalHours < Config.EmailVerExireTime)) return -3;

            SVE.IsVerifiziert = true;

            Random Rand = new Random();
            
            for(int i = 0; i < 10; i++)
            {
                string Name = Rand.Next() + "";
                if (!Context.Users.Any((u) => u.RepoName == Name))
                {
                    user.RepoName = Name;
                    break;
                }
            }

            IStatsFaker Faker = ContManager.GetStatsFaker(user.Email);
            Faker.InitRep(user.RepoName);

            Context.SaveChanges();
            return 1;
        }

        public int CreateAccount(string Email, string Password)
        {
            int R = 0;
            if (Email == null || Password == null)
            {
                R = - 4;
            }
            else if (!IsValidEmail(Email))
            {
                R = - 2;
            }
            else if (Password.Length < 5)
            {
                R = - 3;
            }
            else if (Context.Users.Any(u => u.Email == Email))
            {
                R = - 1;
            }
            else
            {
                string Hash = SecurePasswordHasher.Hash(Password);

                Random Rand = new Random();
                int ID = 0;
                int MaxTries = 10;
                for(int i = 0; i < MaxTries; i++)
                {
                    ID = Rand.Next();
                    if(Context.Users.FirstOrDefault(u=> u.Id == ID)== null)
                    {
                        break;
                    }
                }

                User u = new User() { Created = DateTime.Now, Email = Email, Password = Hash, Id = new Random().Next() };
                Context.Users.Add(u);
                Context.SaveChanges();
                R = u.Id;
            }
            return R;
        }

        public int DeleteAccount(int UserID, string code)
        {
            User? uN = FindUser(UserID);
            if (uN == null) return -1;
            User u = uN ?? default!;

            Context.EmalVerifikations.RemoveRange(
                Context.EmalVerifikations.Where(
                    e => e.UserId == u.Id
                ).ToList()
            );

            Context.Users.Remove(u);
            return 1;
        }

        public int FindUserId(string Email)
        {
            return Context.Users.FirstOrDefault(u => u.Email == Email)?.Id ??-1;
        }

        public int SendEmailVerfikation(int UserId)
        {
            User? uN = FindUser(UserId,true);
            if (uN == null) return -1;
            User u = uN ?? default!;

            int EVS = u.EmalVerifikations.Count();
            if(EVS > Config.MaxMailRequests)
            {
                return -2;
            }

            int Code = new Random().Next();

            EmalVerifikation EV = new EmalVerifikation() {Code = Code+"wow", Erstellt = DateTime.Now ,User=u,UserId=u.Id};

            MailManager.SendCodeMail(u.Email, EV.Code);

            Context.EmalVerifikations.Add(EV);
            Context.SaveChanges();

            return 1;
        }

        public bool CheckIfUserSD(int UserID)
        {
            throw new NotImplementedException();
        }

        public User? FindUser(int UserID, bool Include = false)
        {
            return (Include)?Context.Users.Include(u=>u.EmalVerifikations).FirstOrDefault(u => u.Id == UserID) : Context.Users.FirstOrDefault(u=>u.Id == UserID);
        }

        //kopiert von https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
