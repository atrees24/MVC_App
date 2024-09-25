using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentaionLayer.ViewModels;

namespace PresentaionLayer.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				var User = await _userManager.Users.Select(u => new UserViewModel
				{
					Id = u.Id,
					FName= u.FirstName,
					LName= u.LastName,
					UserName=u.UserName,
					Email= u.Email,
					Roles= _userManager.GetRolesAsync(u).GetAwaiter().GetResult()

				}).ToListAsync();

				return View(User);
			}

			var user = await _userManager.FindByNameAsync(email);
			if (user is null) return View(Enumerable.Empty<UserViewModel>);

			var model = new UserViewModel
			{
				Id = user.Id,
				FName = user.FirstName,
				LName = user.LastName,
				Email = user.Email,
				Roles = await _userManager.GetRolesAsync(user)
			};
			
			return View(model);
		}


        public async Task<IActionResult> Details(string id , string Viewname = nameof(Details))
		{
			if (string.IsNullOrWhiteSpace(id)) return BadRequest();

			var user = await _userManager.FindByIdAsync(id);
			if (user is null) return NotFound();

			var model = new UserViewModel
			{
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(Viewname , model);

        }

        public async Task<IActionResult> Edit(string id) => await Details(id , nameof(Edit));


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FName;
            user.LastName = model.LName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

           
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

        
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            var model = new UserViewModel
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View("Delete", model);
        }


    }
}
