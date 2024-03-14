using System.Linq.Expressions;

namespace Common.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, new() 
{
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

    Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null,
        Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null);
    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
    TEntity? GetSingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
    int Count(Expression<Func<TEntity, bool>>? predicate = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
    TEntity Add(TEntity entity);
    Task<TEntity?> AddAsync(TEntity entity);
    TEntity Update(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    bool Delete(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
}