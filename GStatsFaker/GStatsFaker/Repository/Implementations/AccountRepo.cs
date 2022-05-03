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
        public IContManager ContManager { get; private set; }
        public IJwtAuthenticationManager JWM { get; private set; }

        public AccountRepo(GSFContext Context,IMailManager MailManager, IContManager ContManager,IJwtAuthenticationManager JWM)
        {
            this.Context = Context; 
            this.MailManager = MailManager;
            this.ContManager = ContManager;
            this.JWM = JWM;
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

            IStatsFaker Faker = ContManager.GetStatsFaker(user);


            Context.SaveChanges();
            return 1;
        }

        public int CreateAccount(string Email, string Password)
        {
            int R = 0;
            User? user = Context.Users.Include(u => u.EmalVerifikations).FirstOrDefault(u => u.Email == Email);

            int pState = CheckPassword(Password);

            if (Email == null || Password == null)
            {
                R = -4;
            }
            else if (!IsValidEmail(Email))
            {
                R = -2;
            }
            else if(pState != 1)
            {
                return pState;
            }
            else if (user != null)
            {
                User u = user ?? default!;
                if (u.EmalVerifikations.Any(e => e.IsVerifiziert))
                {
                    R = -6;
                }
                else
                {
                    R = -1;
                }
            } else if (Context.BlockList.Any((b)=>b.Email == Email)) 
            {
                R = -7;
            }
            else
            {
                string Hash = SecurePasswordHasher.Hash(Password);

                Random Rand = new Random();
                int ID = 0;
                int MaxTries = 10;
                for (int i = 0; i < MaxTries; i++)
                {
                    ID = Rand.Next();
                    if (Context.Users.FirstOrDefault(u => u.Id == ID) == null)
                    {
                        break;
                    }
                }

                User u = new User() { Created = DateTime.Now, Email = Email, Password = Hash, Id = new Random().Next(), GithubUsername = Email.Split("@")[0],GithubEmail = Email, MaxCon = 5 };
                Context.Users.Add(u);
                Context.SaveChanges();
                R = u.Id;
            }
            return R;
        }

        private int CheckPassword(string Password)
        {
            if(Password == null)
            {
                return -4;
            }else if(Password.Length < 5)
            {
                return -3;
            }else if (Password.Length > 20)
            {
                return -5;
            }
            return 1;
        }

        public int DeleteAccount(int UserID, string Password)
        {
            User? uN = FindUser(UserID);
            if (uN == null) return -1;
            User u = uN ?? default!;
            bool PasswordCorrect = SecurePasswordHasher.Verify(Password, u.Password);
            if (PasswordCorrect)
            {
                return DeleteAcccountBase(UserID);
            }
            else
            {
                return -2;
            }
        }

        public int DeleteAcccountBase(int UserID)
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
            Context.SaveChanges();
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
        public static bool IsValidEmail(string email)
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

        public int ChangePassword(User user,string oldPassword, string newPassword)
        {
            bool PasswordCorrect = SecurePasswordHasher.Verify(oldPassword, user.Password);
            if (PasswordCorrect)
            {
                int Pstate = CheckPassword(newPassword);
                if(Pstate != 1)
                {
                    return Pstate;
                }
                user.Password = SecurePasswordHasher.Hash(newPassword);
                Context.SaveChanges();
                return 1;
            }
            return -1;
        }
    }
}
