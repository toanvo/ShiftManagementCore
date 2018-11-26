namespace ShiftManagement.DataAccess.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(TKey id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TKey id);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey id);

        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> expression = null);
        
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> expression = null);
        
        IQueryable<TEntity> Select
            (Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
    }
}
