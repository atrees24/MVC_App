using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentaionLayer.ViewModels;
using System.Data;

namespace PresentaionLayer.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var Roles = await _roleManager.Roles.Select(u => new RoleViewModel
                {
                    Id = u.Id,
                    Name = u.Name

                }).ToListAsync();

                return View(Roles);
            }

            var role = await _roleManager.FindByNameAsync(name);
            if (role is null) return View(Enumerable.Empty<RoleViewModel>);

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = new IdentityRole
            {
                Name = model.Name
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Details(string id, string Viewname = nameof(Details))
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound();

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name

            };

            return View(Viewname, model);

        }

        public async Task<IActionResult> Edit(string id) => await Details(id, nameof(Edit));


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);

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
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,

            };

            return View("Delete", model);
        }

        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }

            ViewBag.RoleId = roleId;
            var users = await _userManager.Users.ToListAsync();
            var usersInRole = new List<UserInRoleViewModel>();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    ISInRole = await _userManager.IsInRoleAsync(user, role.Name)
                };

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();
            if (ModelState.IsValid) { 
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.UserId);
                if (appUser == null) continue;

                if (user.ISInRole && !await _userManager.IsInRoleAsync(appUser, role.Name))
                {
                    await _userManager.AddToRoleAsync(appUser, role.Name);
                }
                if (!user.ISInRole && await _userManager.IsInRoleAsync(appUser, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                }
            }

            return RedirectToAction(nameof(Edit), new { id = roleId });
            }

            return View(users);

        }

    }
}
