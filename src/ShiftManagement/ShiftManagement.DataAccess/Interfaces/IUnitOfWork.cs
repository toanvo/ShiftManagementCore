namespace ShiftManagement.DataAccess.Interfaces
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        int Commit();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> CommitAsync();
    }
}
