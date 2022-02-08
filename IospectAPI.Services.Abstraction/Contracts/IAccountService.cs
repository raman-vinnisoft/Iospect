using IospectAPI.Services.Abstraction.Models;
using System.Collections.Generic;

namespace IospectAPI.Services.Abstraction.Contracts
{
    public interface IAccountService
    {
        int AddAccount(AccountModel model);

        int PutDeposit(TransactionModel model);

        int PutWithdraw(TransactionModel model);

        List<AccountModel> GetAccounts();

        List<TransactionModel> GetAccountTransactions(int accountId);
    }
}
