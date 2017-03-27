namespace ShiftManagement.DataAccess.Interfaces
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
