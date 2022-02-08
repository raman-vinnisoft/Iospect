using IospectAPI.Common.Enums;
using IospectAPI.Repositories.Abstraction.Contracts;
using IospectAPI.Repositories.Context.Entities;
using IospectAPI.Services.Abstraction.Contracts;
using IospectAPI.Services.Abstraction.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IospectAPI.Services.Implementation.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<BankAccount> _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Transaction> _transactionRepository;

        public AccountService(IRepository<BankAccount> accountRepository, IRepository<Transaction> transactionRepository,
                              IConfiguration configuration)
        {
            this._accountRepository = accountRepository;
            this._configuration = configuration;
            this._transactionRepository = transactionRepository;
        }


        public List<AccountModel> GetAccounts()
        {
            try
            {
                List<AccountModel> lst = new List<AccountModel>();
                var accounts = this._accountRepository.GetAll(x => x.Id > 0).ConfigureAwait(false).GetAwaiter().GetResult();

                lst = (from a in accounts
                       select new AccountModel
                       {
                           Id = a.Id,
                           Balance = a.Balance,
                           AccountHolder = a.AccountHolder,
                           AccountNumber = a.AccountNumber,
                           AccountStatus = a.AccountStatus,
                           AccountStatusName = Enum.GetName(typeof(AccountStatus), a.AccountStatus),
                           AccountType = a.AccountType,
                           AccountTypeName = Enum.GetName(typeof(AccountType), a.AccountStatus),
                           CreatedDate = a.CreatedDate,
                       }).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public List<TransactionModel> GetAccountTransactions(int accountId)
        {
            try
            {
                List<TransactionModel> lst = new List<TransactionModel>();
                var account = this._accountRepository.Get(x => x.Id == accountId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (account != null)
                {
                    lst = (from a in account.Transaction.ToList()
                           select new TransactionModel
                           {
                               Id = a.Id,
                               AccountId = a.AccountId,
                               AccountNumber = account.AccountNumber,
                               Amount = a.Amount,
                               TransactionType = a.TransactionType,
                               TransactionTypeName = Enum.GetName(typeof(TransactionType), a.TransactionType),
                               Remarks = a.Remarks,
                               CreatedDate = a.CreatedDate,
                           }).ToList();
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<TransactionModel>();
            }
        }

        public int AddAccount(AccountModel model)
        {
            try
            {
                var entityAccount = this._accountRepository.Get(a => a.AccountNumber == model.AccountNumber).ConfigureAwait(false).GetAwaiter().GetResult();

                if (entityAccount != null)
                {
                    return 1;
                }

                BankAccount account = new BankAccount()
                {
                    AccountHolder = model.AccountHolder,
                    AccountNumber = model.AccountNumber,
                    Balance = model.Balance,
                    AccountType = model.AccountType,
                    AccountStatus = (int)AccountStatus.Approved
                };

                if (model.Balance > 0)
                {
                    Transaction trns = new Transaction();
                    trns.Amount = model.Balance;
                    trns.Remarks = "At the time of Account creation";
                    trns.CreatedDate = DateTime.Now;
                    trns.AccountId = account.Id;
                    trns.TransactionType = (int)TransactionType.Deposit;
                    account.Transaction = new List<Transaction>();
                    account.Transaction.Add(trns);
                }

                var result = this._accountRepository.Add(account).ConfigureAwait(false).GetAwaiter().GetResult();
                if (result == null) return -1;
                return 2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int PutDeposit(TransactionModel model)
        {
            try
            {

                var entityAccount = this._accountRepository.Get(a => a.Id == model.AccountId && a.AccountStatus == (int)AccountStatus.Approved).ConfigureAwait(false).GetAwaiter().GetResult();

                if (entityAccount == null)
                    return 0;

                entityAccount.Balance = entityAccount.Balance + model.Amount;

                Transaction trans = new Transaction()
                {
                    Amount = model.Amount,
                    AccountId = model.AccountId,
                    TransactionType = (int)TransactionType.Deposit,
                    Remarks = model.Remarks,
                };

                entityAccount.Transaction.Add(trans);
                var result = this._accountRepository.Update(entityAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!result) return -1;
                return 2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int PutWithdraw(TransactionModel model)
        {
            try
            {

                var entityAccount = this._accountRepository.Get(a => a.Id == model.AccountId && a.AccountStatus == (int)AccountStatus.Approved).ConfigureAwait(false).GetAwaiter().GetResult();

                if (entityAccount == null)
                    return 0;


                if (entityAccount.Balance > model.Amount)
                {
                    entityAccount.Balance = entityAccount.Balance - model.Amount;
                }
                else
                {
                    return 1;
                }


                Transaction trans = new Transaction()
                {
                    Amount = model.Amount,
                    AccountId = model.AccountId,
                    TransactionType = (int)TransactionType.Withdraw,
                    Remarks = model.Remarks,
                };

                entityAccount.Transaction.Add(trans);
                var result = this._accountRepository.Update(entityAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!result) return -1;
                return 2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}

