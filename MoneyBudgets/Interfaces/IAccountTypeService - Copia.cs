using MoneyBudgets.Models;

namespace MoneyBudgets.Interfaces;

public interface IAccountService
{
    public Task AddAccount(AccountModel account);
    Task<List<AccountModel>> GetAccountsbyUserId(int userId);
    Task<AccountModel> GetAccountById(int id,int userId);
    Task UpdateAccount(AccountModelCreation account);
}
