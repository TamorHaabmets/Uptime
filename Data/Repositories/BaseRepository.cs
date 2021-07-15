using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DbContext RepositoryDbContext { get; set; }
        protected DbSet<TEntity> RepositoryDbSet { get; set; }

        public BaseRepository(ApplicationDbContext dataContext)
        {
            RepositoryDbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            RepositoryDbSet = RepositoryDbContext.Set<TEntity>() ?? throw new NullReferenceException($"DbSet for {nameof(TEntity)} was not found.");
        }

        public virtual async Task<List<TEntity>> AllAsync() => await RepositoryDbSet.ToListAsync();

        public virtual Task<TEntity> FindAsync(params object[] id)
        {
            return RepositoryDbSet.FindAsync(id).AsTask();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
              await RepositoryDbSet.AddAsync(entity);
        }

        public virtual TEntity Update(TEntity entity) => RepositoryDbSet.Update(entity).Entity;

        public virtual Task<int> SaveChangesAsync()
        {
            return RepositoryDbContext.SaveChangesAsync();
        }

        public virtual void Remove(TEntity entity)
        {
            RepositoryDbSet.Attach(entity);
            RepositoryDbContext.Entry(entity).State = EntityState.Deleted;
            RepositoryDbSet.Remove(entity);
        }

        public bool Exists(string id) => RepositoryDbSet.Find(id) != null;
    }
}
