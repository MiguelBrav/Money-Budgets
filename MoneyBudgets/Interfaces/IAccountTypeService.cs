using MoneyBudgets.Models;

namespace MoneyBudgets.Interfaces
{
    public interface IAccountTypeService
    {
        public Task AddAccountType(AccountTypeModel account);

        public Task<bool> ExistsAccount(string accountName, int userId);
    }
}
