using System.Linq.Expressions;
using Common.Application.Abstractions.Persistence;
using Common.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new() 
{
    private readonly ApplicationDbContext _applicationDbContext;

    public BaseRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public TEntity[] GetList(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null)
    {
        var queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }

        if (orderBy is not null)
        {
            queryable = descending == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
        }

        if (offset.HasValue)
        {
            queryable = queryable.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            queryable = queryable.Take(limit.Value);
        }

        return queryable.ToArray();
    }    
    
    public async Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null, CancellationToken cancellationToken=default)
    {
        var queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }

        if (orderBy is not null)
        {
            queryable = descending == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
        }

        if (offset.HasValue)
        {
            queryable = queryable.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            queryable = queryable.Take(limit.Value);
        }

        return await queryable.ToArrayAsync(cancellationToken);
    }

    public TEntity? GetSingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? set.SingleOrDefault() : set.SingleOrDefault(predicate);
    } 
   

    public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.SingleOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
    } 
    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.SingleAsync(cancellationToken) : await set.SingleAsync(predicate, cancellationToken);
    }
    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.FirstOrDefaultAsync(cancellationToken) : await set.FirstOrDefaultAsync(predicate,cancellationToken);
    }
    public int Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? set.Count() : set.Count(predicate);
    } 
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ?await set.CountAsync(cancellationToken) :await set.CountAsync(predicate,cancellationToken);
    }

    public TEntity Add(TEntity entity)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Add(entity);
        _applicationDbContext.SaveChanges();
        return entity;
    } 
    public async Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        await set.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }
 

    public TEntity Update(TEntity entity)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Update(entity);
        _applicationDbContext.SaveChanges();
        return entity;
    }  
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Update(entity);
        await  _applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public bool Delete(TEntity entity)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Remove(entity);
        return _applicationDbContext.SaveChanges() > 0;
    }  
    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Remove(entity);
        return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}