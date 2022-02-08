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
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly SqlDbContext _dbContext;
        public TransactionRepository(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public async Task<Transaction> Add(Transaction entity)
        {
            try
            {
                EntityEntry<Transaction> result = this._dbContext.Transaction.Add(entity);
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
                var trans = this._dbContext.Transaction.Where(s => s.Id == id).FirstOrDefault();
                this._dbContext.Transaction.Remove(trans);
                return await this.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Transaction> Get(Expression<Func<Transaction, bool>> expression)
        {
            try
            {
                var trans = this._dbContext.Transaction.Where(expression).FirstOrDefault();
                return trans;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetAll(Expression<Func<Transaction, bool>> expression = null)
        {
            try
            {
                List<Transaction> trans = await this._dbContext.Transaction.Where(expression).ToListAsync();
                return trans;
            }
            catch (Exception)
            {
                throw;
            }
            //
        }

        public async Task<bool> Update(Transaction entity)
        {
            try
            {
                this._dbContext.Transaction.Update(entity);
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
