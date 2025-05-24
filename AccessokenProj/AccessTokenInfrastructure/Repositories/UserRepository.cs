using System;
using AccessTokenDomain.Entity;
using AccessTokenDomain.Interfaces.IRepositories;
using AccessTokenInfrastructure.Context;

namespace AccessTokenInfrastructure.Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}

