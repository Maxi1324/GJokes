using GStatsFaker.Model;

namespace GStatsFaker.Repository.Interfaces
{
    public interface IAccountRepo
    {
        /// <summary>
        /// Erstellt einen neuen User
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns>
        /// r >= 0  UserID
        /// r == -1 Nutzer Mail exestiert bereits
        /// r == -2 Mail is not valid
        /// r == -3 Password kürzer als 5
        /// r == -4 Email || Password ist null
        /// r == -5 Password is to long > 20
        /// r == -6 Email already in use
        /// </returns>
        public int CreateAccount(string Email, string Password);

        /// <summary>
        /// Send Email Verifkation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// r == 1 Alles OK
        /// r == -1 User not found
        /// r == -2 Zuviele Requests Email ist gesperrt
        /// </returns>
        public int SendEmailVerfikation(int userId);

        /// <summary>
        /// Verifiziert eine Email
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="code"></param>
        /// <returns>
        /// r == 1 hat funktioniert
        /// r == -1 Nutzer exestiert nicht
        /// r == -2 Code ungültig
        /// r == -3 EmailVerifikationabgelaufen
        /// </returns>
        public int ActivateAccount(int UserID,string code);

        /// <summary>
        /// Ja lol löscht einen User
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="code"></param>
        /// <returns>
        /// r == 1 hat funktioniert
        /// r == -1 Nutzer exestiert nicht
        /// </returns>
        public int DeleteAccount(int UserID, string Password);

        /// <summary>
        /// Retunrs the ID of user
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>
        /// Returns the ID of a user.
        /// If there is no User with this Email, -1 is returnd
        /// </returns>
        public int FindUserId(string Email);
    
        /// <summary>
        /// Überprüft, ob ein Nutzer gelöscht gehört
        /// </summary>
        /// <param name="UserID"></param>
        public bool CheckIfUserSD(int UserID);

        public User? FindUser(int UserID, bool Include = false);

        /// <summary>
        /// Ändert das Passwort lol
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>
        /// R== 1 Alles ok
        /// R == -1 Passwort ist falsch
        /// R == -4 Passwort ist null
        /// R == -3 Passwort ist < 5
        /// R == -5 Passwort ist > 20
        /// </returns>
        public int ChangePassword(User user, string oldPassword, string newPassword);

        public int DeleteAcccountBase(int UserID);
    }
}
