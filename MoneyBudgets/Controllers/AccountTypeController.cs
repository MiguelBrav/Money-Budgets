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
        private readonly IUsersService _usersService;
        public AccountTypeController(ILogger<HomeController> logger, IAccountTypeService accountTypeService, IUsersService usersService)
        {
            _logger = logger;
            _accountTypeService = accountTypeService;
            _usersService = usersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> IndexAsync()
        {
            var id = _usersService.GetUserId();
            var accountTypes = await _accountTypeService.GetAccountsbyUser(id);


            return View(accountTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTypeModel account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);

            }
            // TODO- Make users and quit this mock example
            account.UserId = _usersService.GetUserId();

            bool exists = await _accountTypeService.ExistsAccount(account.Name,account.UserId);

            if (exists)
            {
                ModelState.AddModelError(nameof(account.Name), $"Account type {account.Name} already exists");

                return View(account);
            }

            await _accountTypeService.AddAccountType(account);

            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> ValidateIfExists(string name)
        {
            // TODO- Make users and quit this mock example
            var id = _usersService.GetUserId();
            var existsAccount = await _accountTypeService.ExistsAccount(name, id);

            if(existsAccount) {
                return Json($"Name {name} already exists");
            }

            return Json(true);
        }

    }
}