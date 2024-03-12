using Core.Entities;
using Core.Interfaces;
using Infrastructue.Data;
using System.Collections;

namespace Infrastructure.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _storeContext;

    private Hashtable _repositories;

    public UnitOfWork(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories is null)
        {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
            var repositoryInstance = Activator.CreateInstance(repositoryType, _storeContext);

            _repositories[type] = repositoryInstance;
        }

        return (IGenericRepository<TEntity>) _repositories[type];
    }

    public async Task<int> Complete()
    {
        return await _storeContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _storeContext.Dispose();
    }
}
