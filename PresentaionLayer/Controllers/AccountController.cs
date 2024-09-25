using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PresentaionLayer.Utilites;
using PresentaionLayer.ViewModels;
using System.Security.Cryptography.Pkcs;
using Email = PresentaionLayer.Utilites.Email;

namespace PresentaionLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.userName,
                Email = model.email,
                FirstName = model.firstName,
                LastName = model.lastName,
            };

           var result = _userManager.CreateAsync(user , model.password).Result;
            if (result.Succeeded)
                return RedirectToAction(nameof(Login));
            foreach (var Error in result.Errors)
                ModelState.AddModelError(string.Empty, Error.Description);
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
			if (!ModelState.IsValid) return View(model);

            var user = _userManager.FindByEmailAsync(model.email).Result;   
            if(user is not null)
            {
                if(_userManager.CheckPasswordAsync(user, model.password).Result)
                {
                    var result = _signInManager.PasswordSignInAsync(user,model.password,model.RememberMe,false).Result;
					if (result.Succeeded)
						return RedirectToAction(nameof(HomeController.Index),"Home");
				}
            }
            return View(model);
		}

        public  IActionResult SignOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

		public IActionResult ForgetPassword()
		{
			return View();
		}

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user is not null)
            {
                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

                var url = Url.Action(nameof(ResetPassword), nameof(AccountController).
                    Replace("Controller", string.Empty),
                    new {email = model.Email , Token = token },Request.Scheme);

                var email = new Email
                {
                    Subject = "Reset Your Password",
                    Body = url,
                    Recipient = model.Email
				};

                return RedirectToAction(nameof(CheckYourInbox));
            }
            return View(model);
        }

		public IActionResult CheckYourInbox()
		{

			return View();
		}
		public IActionResult ResetPassword()
        {

           return View(); 
        }
    }
}
