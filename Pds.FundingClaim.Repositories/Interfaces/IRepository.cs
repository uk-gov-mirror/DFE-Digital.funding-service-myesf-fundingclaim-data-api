using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Repositories.Interfaces
{
    /// <summary>
    /// The generic repository to perform CRUD operations on database entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to act upon.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets the all the entities matching the filter.
        /// </summary>
        /// <param name="filter">The filter to find the match.</param>
        /// <typeparam name="TEntity">The type of entity to act upon.</typeparam>
        /// <returns>The matched entities.</returns>
        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Gets the first or default matched entity.
        /// </summary>
        /// <param name="filter">The filter to find the match.</param>
        /// <typeparam name="TEntity">The type of entity to act upon.</typeparam>
        /// <returns>The matched entity.</returns>
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Gets all the items of entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to act upon.</typeparam>
        /// <returns>All the entities of the type.</returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        /// Gets the item by the predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="includes">Entity to include.</param>
        /// <returns>Entity by given Id.</returns>
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes);

        /// <summary>
        /// Adds an instance of entity to database.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<TEntity> Create(TEntity entity);

        /// <summary>
        /// Updates an existing instance of entity in database.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Update(TEntity entity);
    }
}