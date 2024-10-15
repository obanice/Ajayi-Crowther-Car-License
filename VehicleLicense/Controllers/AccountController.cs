using Core.Db;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VehicleLicense.Controllers
{
    public class AccountController : Controller
    {
		private readonly AppDbContext _context;
		public AccountController(AppDbContext context)
        {
			_context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult LogIn(LoginCredential request)
		{
             var checkUser = _context.LoginCredentials.Where(c => c.UserName == request.UserName && c.Password == request.Password).FirstOrDefault();
            if (checkUser != null) 
            {
				return RedirectToAction("Index", "Admin");
			}
			TempData["Message"] = "incorrect Credentials";
			return RedirectToAction("Login", "Account");
		}
    }
}