using Microsoft.EntityFrameworkCore;
using Pamwork.Core.Exceptions;
using Pamwork.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pamwork.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;
        private DbSet<TEntity> _dbSet;

        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected DbSet<TEntity> DbSet => _dbSet ??= _context.Set<TEntity>();

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error fetching all entities", ex);
            }
        }

        public virtual async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                return await DbSet.Where(match).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error finding matching entities", ex);
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            try
            {
                return await DbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error fetching entity with id {id}", ex);
            }
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            try
            {
                return await DbSet.SingleOrDefaultAsync(match);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error finding single entity", ex);
            }
        }

        public async Task<int> CountAsync()
        {
            try
            {
                return await DbSet.CountAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error counting entities", ex);
            }
        }

        public virtual async Task<object> AddAsync(TEntity entity, bool saveChanges = false)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                await DbSet.AddAsync(entity);
                if (saveChanges)
                {
                    await CommitAsync();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error adding entity", ex);
            }
        }

        public virtual async Task DeleteAsync(object id, bool saveChanges = false)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) throw new DataAccessException($"Entity with id {id} not found");

            await DeleteAsync(entity, saveChanges);
        }

        public virtual async Task DeleteAsync(TEntity entity, bool saveChanges = false)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                DbSet.Remove(entity);
                if (saveChanges)
                {
                    await CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error deleting entity", ex);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity, bool saveChanges = false)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var entry = _context.Entry(entity);
                DbSet.Attach(entity);
                entry.State = EntityState.Modified;

                if (saveChanges)
                {
                    await CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error updating entity", ex);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, object key, bool saveChanges = false)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var existingEntity = await GetByIdAsync(key);
                if (existingEntity == null) return null;

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);

                if (saveChanges)
                {
                    await CommitAsync();
                }

                return existingEntity;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error updating entity with key {key}", ex);
            }
        }

        public virtual async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error committing changes to database", ex);
            }
        }

        #region IDisposable Implementation

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
