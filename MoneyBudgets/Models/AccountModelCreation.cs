using Microsoft.AspNetCore.Mvc.Rendering;

namespace MoneyBudgets.Models;

public class AccountModelCreation : AccountModel
{
    public IEnumerable<SelectListItem>? AccountTypes {  get; set; }

}
