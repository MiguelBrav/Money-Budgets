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

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
          
            var userId = _usersService.GetUserId();
            var accounttype = await _accountTypeService.GetAccountbyUserAndId(userId, id);

            if (accounttype is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            return View(accounttype);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(AccountTypeModel accountType)
        {

            var userId = _usersService.GetUserId();
            var accounttype = await _accountTypeService.GetAccountbyUserAndId(userId, accountType.Id);

            if (accounttype is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            await _accountTypeService.UpdateAccountType(accountType);

            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id)
        {

            var userId = _usersService.GetUserId();
            var accounttype = await _accountTypeService.GetAccountbyUserAndId(userId, id);

            if (accounttype is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            return View(accounttype);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteAccountType(int id)
        {

            var userId = _usersService.GetUserId();
            var accounttype = await _accountTypeService.GetAccountbyUserAndId(userId, id);

            if (accounttype is null)
            {
                return RedirectToAction("NotExists", "Home");
            }

            try
            {
                await _accountTypeService.DeleteAccountType(userId, id);
            }
            catch(Exception ex)
            {
                {
                    TempData["error"] = "An error occurred: " + ex.Message;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> OrderAccounts([FromBody] int[] ids)
        {
            var userId = _usersService.GetUserId();
            List<AccountTypeModel> accounttypes = await _accountTypeService.GetAccountsbyUser(userId);

            var accountsIdsToOrder = accounttypes.Select(x => x.Id);

            var IdsNotSameUser = accountsIdsToOrder.Except(ids).ToList();

            if(IdsNotSameUser.Count > 0)
            {
                return Forbid();
            }

            var accountsOrdered = ids.Select((value,index) => 
            new AccountTypeModel() { Id = value, Order = index + 1}).AsEnumerable();

            await _accountTypeService.OrderAccounts(accountsOrdered);

            return Ok();
        }
    }
}