namespace GStatsFaker.Repository.Interfaces
{
    public interface IContManager
    {
        public void CreateCons();

        public IStatsFaker GetStatsFaker(string mail);
    }
}
