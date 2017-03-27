﻿namespace ShiftManagement.DataAccess.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using ShiftManagement.DataAccess.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ShiftManagementDbContext _context;

        private DbSet<TEntity> _dbSet;

        public GenericRepository(ShiftManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> pression = null)
        {
            IQueryable<TEntity> query = null;            
            query = pression == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(pression);
            return query;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Insert(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
