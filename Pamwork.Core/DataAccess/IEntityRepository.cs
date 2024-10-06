using Pamwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.Core.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match);
        Task<int> CountAsync();
        Task<object> AddAsync(TEntity entity, bool saveChanges = false);
        Task DeleteAsync(object id, bool saveChanges = false);
        Task DeleteAsync(TEntity entity, bool saveChanges = false);
        Task UpdateAsync(TEntity entity, bool saveChanges = false);
        Task<TEntity> UpdateAsync(TEntity entity, object key, bool saveChanges = false);
        Task CommitAsync();
        void Dispose();

    }
}
