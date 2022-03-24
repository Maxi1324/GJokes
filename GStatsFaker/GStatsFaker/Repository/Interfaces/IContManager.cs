using GStatsFaker.Model;

namespace GStatsFaker.Repository.Interfaces
{
    public interface IContManager
    {
        public void CreateCons();

        public IStatsFaker GetStatsFaker(User user);
    }
}
