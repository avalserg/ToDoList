using System.Linq.Expressions;

namespace Common.Repositories.Deprecated;

public class BaseRepositoryDeprecated<TEntity> : IBaseRepositoryDeprecated<TEntity> where TEntity : class, new() 
{
    private static readonly List<TEntity> _data=new();

    public TEntity[] GetList(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null)
    {
        IEnumerable<TEntity> result = _data;

        if (predicate != null)
        {
            result = result.Where(predicate.Compile());
        }

        if (orderBy != null)
        {
            result = descending.GetValueOrDefault()
                ? result.OrderByDescending(orderBy.Compile())
                : result.OrderBy(orderBy.Compile());
        }
        
        result = result.Skip(offset.GetValueOrDefault());
       
        if (limit.HasValue)
        {
            result = result.Take(limit.Value);
        }

        return result.ToArray();
    }

    public TEntity? GetSingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate is null)
        {
            return  _data.SingleOrDefault();
        }
       
        return _data.SingleOrDefault(predicate.Compile());
    }

    public int Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IEnumerable<TEntity> entities = _data;
        if (predicate == null)
        {
            return entities.Count();
        }

        return entities.Where(predicate.Compile()).Count();
    }

    public TEntity Add(TEntity entity)
    {
        _data.Add(entity);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        //!!
        Delete(entity);
        _data.Add(entity);
        
        return entity;
    }

    public bool Delete(TEntity entity)
    {
        return _data.Remove(entity);
    }
}