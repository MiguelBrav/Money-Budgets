using Microsoft.AspNetCore.Mvc;
using MoneyBudgets.Interfaces;
using MoneyBudgets.Models;
using System.Diagnostics;

namespace MoneyBudgets.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountTypeService _accountTypeService;
        public AccountTypeController(ILogger<HomeController> logger, IAccountTypeService accountTypeService)
        {
            _logger = logger;
            _accountTypeService = accountTypeService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AccountTypeModel account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);

            }
            // TODO- Make users and quit this mock example
            account.UserId = 1;

            _accountTypeService.AddAccountType(account);

            return View();
        }

    }
}