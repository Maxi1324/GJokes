using GStatsFaker.DBContexts;
using GStatsFaker.Model;
using GStatsFaker.Repository.Interfaces;

namespace GStatsFaker.Repository.Implementations
{
    public class AccountRepo : IAccountRepo
    {
        public GSFContext Context { get; set; } 

        public AccountRepo(GSFContext Context)
        {
            this.Context = Context; 
        }

        public int ActivateAccount(int UserID, string code)
        {
            User? userN = FindUser(UserID);
            if (userN == null) return -1;
            User user = userN ?? default!;

            EmalVerifikation? SEVN = user.EmalVerifikations.FirstOrDefault(e => e.Code == code);
            if (SEVN == null) return -2;
            EmalVerifikation SVE = SEVN ?? default!;

            if (!(DateTime.Now.Subtract(SVE.Erstellt).TotalHours < Config.EmailVerExireTime)) return -3;

            SVE.IsVerifiziert = true;
            return 1;
        }

        public int CreateAccount(string Email, string Password)
        {
            int R = 0;
            if (Email == null || Password == null)
            {
                R = - 4;
            }
            else if (IsValidEmail(Email))
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
                User u = new User() { Created = DateTime.Now, Email = Email, Password = Password };
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
            User? uN = FindUser(UserId);
            if (uN == null) return -1;
            User u = uN ?? default!;

            int Code = new Random().Next();

            EmalVerifikation EV = new EmalVerifikation() {Code = Code+"wow", Erstellt = DateTime.Now };
            
            u.EmalVerifikations.Add(EV);
            Context.SaveChanges();

            return 1;
        }

        public bool CheckIfUserSD(int UserID)
        {
            throw new NotImplementedException();
        }

        public User? FindUser(int UserID)
        {
            return Context.Users.FirstOrDefault(u=>u.Id == UserID);
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
