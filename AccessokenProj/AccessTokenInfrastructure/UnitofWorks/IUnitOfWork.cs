using System;
using AccessTokenDomain.Interfaces.IRepositories;

namespace AccessTokenInfrastructure.UnitofWorks
{
	public interface IUnitOfWork
	{
        IGenericRepository<TEntity> repository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();

        IUserRepository userRepo { get; }
    }
}

