using System.Linq.Expressions;

namespace Common.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, new() 
{
    Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null,
        Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="offset">count miss values</param>
    /// <param name="limit">count take values</param>
    /// <param name="predicate">search by field</param>
    /// <param name="orderBy">sort by field</param>
    /// <param name="descending">sort desc or asc</param>
    /// <returns></returns>
    TEntity[] GetList(
        int? offset = null,
        int? limit = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null);

    TEntity? GetSingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
    int Count(Expression<Func<TEntity, bool>>? predicate = null);
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    bool Delete(TEntity entity);
}