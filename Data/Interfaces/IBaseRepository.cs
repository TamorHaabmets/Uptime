using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> AllAsync();
        Task<TEntity> FindAsync(params object[] id);
        Task AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task<int> SaveChangesAsync();
        void Remove(TEntity entity);
        bool Exists(string id);
    }
}
