namespace ShiftManagement.DataAccess.Interfaces
{
    public interface IRepository<TEntity> : IBaseRepository<TEntity, int> where TEntity : class
    {
    }
}
