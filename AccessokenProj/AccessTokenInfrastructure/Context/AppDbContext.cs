using System;
using System.Collections.Generic;
using AccessTokenDomain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AccessTokenInfrastructure.Context
{
	public class AppDbContext : DbContext
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           

        }

        public DbSet<User> Users { get; set; }

    }
}

