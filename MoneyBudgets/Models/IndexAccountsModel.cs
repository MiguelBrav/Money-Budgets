namespace MoneyBudgets.Models;

public class IndexAccountsModel
{
    public string AccountType { get; set; } = string.Empty;
    public List<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    public decimal Balance => Accounts.Sum(x => x.Balance);
}
