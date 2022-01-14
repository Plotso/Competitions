namespace Competitions.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Domain.BL.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ViewModels.Customer;

    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsersService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IUsersService userService,
            ILogger<UsersController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
        }
        
        public async Task<IActionResult> All()
        {
            //var users = _signInManager.UserManager.Users;  //Filters deleted
            var users = _userService.GetAllWithDeleted<ApplicationUserViewModel>();
            var userViewModels = new List<ApplicationUserViewModel>();
            foreach (var user in users)
            {
                var isAdmin = await IsAdminAsync(user.Email);
                var userModel = user;
                userModel.IsAdmin = isAdmin;
                userViewModels.Add(userModel);
            }
            
            var viewModel = new UsersListViewModel()
            {
                Users = userViewModels
            };
            return View(viewModel);
        }

        public async Task<IActionResult> EditUserRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any(r => r == GlobalConstants.AdministratorRoleName))
            {
                await _userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Id == user.Id)
                {
                    await _signInManager.SignOutAsync();
                }
            }
            else
            {
                await _userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }


            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> EditUserIsDeletedStatus(string email)
        {
            try
            {
                await _userService.ChangeIsDelete(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occured during new education record creation.");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All));
        }
        
        private async Task<bool> IsAdminAsync(string email)
        {
            var user = _userService.FindByEmail(email);
            if (user == null) 
                return false;
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Any(r => r == GlobalConstants.AdministratorRoleName);
        }
    }
}