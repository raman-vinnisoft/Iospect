using IospectAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace IospectAPI.Repositories.Context.Context
{
    public class SqlDbContext:DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options): base(options)
        {

        }

        
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

    }
}
