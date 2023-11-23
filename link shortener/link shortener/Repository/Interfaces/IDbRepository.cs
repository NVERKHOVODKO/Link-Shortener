using System.Linq.Expressions;
using Entities;

namespace Repository;

public interface IDbRepository
{
    IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;
    IQueryable<T> Get<T>() where T : class, IEntity;
    IQueryable<T> GetAll<T>() where T : class, IEntity;
    Task<Guid> Add<T>(T newEntity) where T : class, IEntity;
    Task Delete<T>(Guid entity) where T : class, IEntity;
    Task Update<T>(T entity) where T : class, IEntity;
    Task<int> SaveChangesAsync();
}