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
    }
}
