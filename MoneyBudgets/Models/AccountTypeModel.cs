using System.ComponentModel.DataAnnotations;

namespace MoneyBudgets.Models
{
    public class AccountTypeModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Order { get; set; }
  
    }
}
