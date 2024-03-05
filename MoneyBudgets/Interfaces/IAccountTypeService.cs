using MoneyBudgets.Models;

namespace MoneyBudgets.Interfaces
{
    public interface IAccountTypeService
    {
        public Task AddAccountType(AccountTypeModel account);

        public Task<bool> ExistsAccount(string accountName, int userId);
        public Task<List<AccountTypeModel>> GetAccountsbyUser(int userId);
        public Task<AccountTypeModel> GetAccountbyUserAndId(int userId, int id);
        public Task UpdateAccount(AccountTypeModel account);
    }
}
