using System;
using System.Collections;
using AccessTokenDomain.Interfaces.IRepositories;
using AccessTokenInfrastructure.Context;
using AccessTokenInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AccessTokenInfrastructure.UnitofWorks
{
	public class UnitOfWork : IUnitOfWork
	{
        public readonly AppDbContext _dbContext;
        private Hashtable _repositories;
        public DatabaseFacade Database => _dbContext.Database;
        public IUserRepository userRepo { get; private set; }


        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            userRepo = new UserRepository(dbContext);
        }

        public IGenericRepository<TEntity> repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();
            var Type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(Type))
            {
                var repositoryType = typeof(GenericRepository<TEntity>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
                _repositories.Add(Type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repositories[Type];
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

