using System.Linq.Expressions;

namespace Common.Repositories.Deprecated;

public interface IBaseRepositoryDeprecated<TEntity> where TEntity : class, new() 
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
    TEntity? GetSingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
    int Count(Expression<Func<TEntity, bool>>? predicate = null);
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    bool Delete(TEntity entity);
}