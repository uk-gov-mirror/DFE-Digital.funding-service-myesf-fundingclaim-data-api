using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Repositories.Implementation
{
    /// <inheritdoc cref="IRepository"/>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly PdsContext _context;
        private readonly ILoggerAdapter<Repository<TEntity>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// The parametrised constructor.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logging service.</param>
        public Repository(PdsContext context, ILoggerAdapter<Repository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null)
            => _context.Set<TEntity>().Where(filter);

        /// <inheritdoc/>
        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
            => await _context.Set<TEntity>().FirstOrDefaultAsync(filter);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            return await includes(query).Where(predicate).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<TEntity> Create(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "User just created a {entityType} with id {createdEntityId}.",
                entity.GetType(),
                entity?.GetType().GetProperty("Id")?.GetValue(entity));
            return entity;
        }

        /// <inheritdoc/>
        public async Task Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User just updated a {entity.GetType()} with id {entity?.GetType().GetProperty("Id")?.GetValue(entity)}.");
        }
    }
}