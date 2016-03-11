using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpFora.Security;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using SharpFora.DAL.Models;
using SharpFora.Attributes;

namespace SharpFora.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> LoginManager { get; set; }

        public AccountController(SignInManager<User> loginManager)
        {
            LoginManager = loginManager;
        }

        [Antiforgery(AntiforgeryMode.Set)]
        public IActionResult Register()
        {
            return View();
        }

        /*public async Task<IActionResult> Login()
        {
            var user = new User()
            {
                Name = "Admin"
            };
            await LoginManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        [Authorize("Login"), HttpPost]
        public IActionResult Secret()
        {
            return Json(new
            {
                token = "" //Base32.ToString(TOTPTokenProvider.GenerateSecret())
            });
        }*/
    }
}
