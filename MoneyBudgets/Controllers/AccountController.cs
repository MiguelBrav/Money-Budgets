using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyBudgets.Interfaces;
using MoneyBudgets.Models;

namespace MoneyBudgets.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountTypeService _accountTypeService;
        private readonly IAccountService _accountService;
        private readonly IUsersService _usersService;

        public AccountController(IAccountTypeService accountTypeService, IUsersService usersService, IAccountService accountService)
        {
            _accountTypeService = accountTypeService;
            _usersService = usersService;
            _accountService = accountService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userId = _usersService.GetUserId();
            var accountWithTypeAccount = await _accountService.GetAccountsbyUserId(userId);

            var model = accountWithTypeAccount.GroupBy(x => x.AccountType).
                Select(group => new IndexAccountsModel { 
                        AccountType = group.Key,
                        Accounts = group.AsList()
                }).ToList();

            return View(model);
        }


        [HttpGet]
        public async Task <IActionResult> Create()
        {
            var userId = _usersService.GetUserId();
            var model = new AccountModelCreation();
            model.AccountTypes = await GetAccountTypes(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountModelCreation account)
        {            
            var userId = _usersService.GetUserId();
            var accountType = await _accountTypeService.GetAccountbyUserAndId(userId, account.AccountTypeId);

            if (accountType is null)
            {
                return RedirectToAction("NotExists", "Home");              
            }

            //await _accountTypeService.AddAccountType(account);

            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes(userId);
                return View(account);
            }

            await _accountService.AddAccount(account);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _usersService.GetUserId();

            var account = await _accountService.GetAccountById(id,userId);

            if(account is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            var model = new AccountModelCreation();

            model.Id = account.Id;
            model.Name = account.Name;
            model.Description = account.Description;
            model.AccountTypeId = account.AccountTypeId;
            model.Balance = account.Balance;
            model.AccountTypes = await GetAccountTypes(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(AccountModelCreation editAccount)
        {
            var userId = _usersService.GetUserId();

            var account = await _accountService.GetAccountById(editAccount.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            var accountType = await _accountTypeService.GetAccountbyUserAndId(userId, editAccount.AccountTypeId);

            if (accountType is null)
            {
                return RedirectToAction("NotExists", "Home");

            }

            await _accountService.UpdateAccount(editAccount);

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _accountTypeService.GetAccountsbyUser(userId);
            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }


    }
}
