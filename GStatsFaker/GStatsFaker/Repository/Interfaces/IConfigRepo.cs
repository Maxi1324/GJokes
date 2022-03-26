using GStatsFaker.Model;
using System.Security.Claims;

namespace GStatsFaker.Repository.Interfaces
{
    public interface IConfigRepo
    {
        public ConfigInfos GetUserConfigData(User User);
        /// <summary>
        /// Set die Contributen Range für einen User
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <returns>
        /// R == 1 Alles Ok
        /// R == -1 MinCon oder MaxCon ist kleiner als 0
        /// R == -2 MinCon ist größergleich MaxCon
        /// </returns>
        public int SetConRange(User User,int Min,int Max);   
        /// <summary>
        /// Setz den Repo Name lol
        /// </summary>
        /// <param name="User"></param>
        /// <param name="repoName"></param>
        /// <returns>
        /// R== 1 Alles Ok
        /// R==-1 Name bereits vergeben
        /// </returns>
        public int SetRepoName(User User,string repoName);
        /// <summary>
        /// Finden einen Nutzer über das ClaimsPricipal Objekt(Was man über user beim Controller bekommt)
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public User FindUser(ClaimsPrincipal Email);

        /// <summary>
        /// Set GAS
        /// </summary>
        /// <param name="GAS"></param>
        /// <returns>
        /// R == -1 GithubEmail not valid
        /// R == 1 Hatfunktioniert
        /// </returns>
        public int SetGAS(GithubAccountSettings GAS, User user);

        /// <summary>
        /// Invited einen user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// R== 1 AllesOk
        /// R == -1 Github Email must be set
        /// R == -2 Github Username must be set
        /// R == -3 User is already in Repo
        /// </returns>
        public int Invite(User user);

        public void CreateCont(int num, User user);
    }
}
