using MoneyBudgets.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoneyBudgets.Models;

public class AccountModel
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:80)]
    [FirstCapitalLetter]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Account Type")]
    public int AccountTypeId { get; set; }
    public decimal Balance { get; set; }
    [StringLength(maximumLength: 1000)]
    public string Description { get; set; } = string.Empty;
    public string? AccountType { get; set; }
}
