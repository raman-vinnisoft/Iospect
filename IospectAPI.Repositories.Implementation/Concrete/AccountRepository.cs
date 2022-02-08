using IospectAPI.Repositories.Abstraction.Contracts;
using IospectAPI.Repositories.Context.Context;
using IospectAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IospectAPI.Repositories.Implementation.Concrete
{
    public class AccountRepository : IRepository<BankAccount>
    {
        private readonly SqlDbContext _dbContext;
        public AccountRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<BankAccount> Add(BankAccount entity)
        {
            try
            {
                EntityEntry<BankAccount> result = this._dbContext.BankAccount.Add(entity);
                if (await this.SaveChanges()) return result.Entity;
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> Delete(long id)
        {
            try
            {
                var account = this._dbContext.BankAccount.Where(s => s.Id == id).FirstOrDefault();
                this._dbContext.BankAccount.Remove(account);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BankAccount> Get(Expression<Func<BankAccount, bool>> expression)
        {
            try
            {
                BankAccount account = this._dbContext.BankAccount.Where(expression).Include(s => s.Transaction).FirstOrDefault();
                return account;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BankAccount>> GetAll(Expression<Func<BankAccount, bool>> expression = null)
        {
            try
            {
                List<BankAccount> accounts = await this._dbContext.BankAccount.Where(expression).ToListAsync();
                return accounts;
            }
            catch (Exception ex)
            {
                throw;
            }
            //
        }

        public async Task<bool> Update(BankAccount entity)
        {
            try
            {
                this._dbContext.BankAccount.Update(entity);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> SaveChanges()
        {
            int rowsEffected = await this._dbContext.SaveChangesAsync();
            if (rowsEffected == 0) return false;
            return true;
        }
    }
}
