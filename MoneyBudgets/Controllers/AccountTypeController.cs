using Microsoft.AspNetCore.Mvc;
using MoneyBudgets.Models;
using System.Diagnostics;

namespace MoneyBudgets.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AccountTypeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

            return View();
        }

    }
}