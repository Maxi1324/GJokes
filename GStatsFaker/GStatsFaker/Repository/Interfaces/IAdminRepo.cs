using GStatsFaker.Model;

namespace GStatsFaker.Repository.Interfaces
{
    public enum OrderBy
    {
        Email,
        Id,
        Joined,
        JoinedDesc
    }

    public enum Filter
    {
        All,
        Blocked,
        NotBlocked,
        NotAuthenticated,
        Authenticated
    }

    public interface IAdminRepo
    {
        public int BlockPerson(BlockUser UI);
        public int UnblockPerson(BlockUser UI);

        public List<HoleUserInfos> GetUsers(int Page, OrderBy OB, Filter F, string password);
        public bool CheckAdminPassword(string password);

    }
}