using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GitServer.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using GitServer.ApplicationCore.Interfaces;
using GitServer.ApplicationCore.Models;
using GitServer.Services;
using GitServer.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace GitServer.Controllers
{
    public class UserController : Controller
    {
        private IRepository<User> _user;
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IRepository<User> user, UserService service, ILogger<UserController> logger)
        {
            _user = user;
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Content("ok");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _user.List(r => r.Name == model.Username && r.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return Redirect("/");
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                _user.Add(new User()
                {
                    Name = model.Username,
                    Email = model.Email,
                    Password = model.ConfirmPassword,
                    CreationDate = DateTime.Now
                });
                return Redirect("/");
            }

            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public IActionResult ProfileSetting()
        {
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = _service.GetUserByName(currentUserName);
            var model = new UserProfileSettingViewModel
            {
                Name = currentUser.Name,
                Email = currentUser.Email,
                Description = currentUser.Description,
                WebSite = currentUser.WebSite
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ProfileSetting(UserProfileSettingViewModel newProfile)
        {
            if (newProfile != null)
            {
                var user = _service.GetUserByName(HttpContext.User.Identity.Name);
                user.Name = newProfile.Name;
                user.Email = newProfile.Email;
                user.Description = newProfile.Description;
                user.WebSite = newProfile.WebSite;
                _service.Save(user);
            }

            return Redirect(nameof(ProfileSetting));
        }

        [HttpGet]
        public IActionResult SecuritySetting()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SecuritySetting(UserSecurityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
            {
                ModelState.AddModelError(string.Empty, "Password confirmation doesn\'t match the password");
                return View();
            }

            var user = GetCurrentUser();
            if (!model.OldPassword.Equals(user.Password))
            {
                ModelState.AddModelError(string.Empty,
                    "New Password confirmation doesn\'t match the previous password");
                return View();
            }

            user.Password = model.NewPassword;
            _service.Save(user);
            return RedirectToAction(nameof(SignOut));
        }

        private User GetCurrentUser()
        {
            return _service.GetUserByName(HttpContext.User.Identity.Name);
        }
    }
}