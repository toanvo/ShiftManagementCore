namespace ShiftManagement.DataAccess.Repositories
{
    using ShiftManagement.DataAccess.Interfaces;
    using ShiftManagement.Infrastructure;
    using System;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ShiftManagementDbContext _context;
        private readonly IObjectFactory _objectFactory;
        private bool disposedValue = false; 

        public UnitOfWork(ShiftManagementDbContext context, IObjectFactory objectFactory)
        {
            _context = context;
            _objectFactory = objectFactory;
        }

        public int Commit()
        {
            return _context.SaveChanges();    
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
        
        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _objectFactory.GetByType<TRepository>(typeof(IRepository<>));
        }

        public void Dispose()
        {            
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing && _context != null)
            {
                _context.Dispose();
            }

            disposedValue = true;
        }

    }
}
