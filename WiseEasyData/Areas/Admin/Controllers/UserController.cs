using Core.Constants;
using Core.Contracts;
using Core.Models.User;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController (RoleManager<IdentityRole> _roleoManager, UserManager<ApplicationUser> _userManager
            , IUserService _userService)
        {
            roleManager = _roleoManager;
            userManager = _userManager;
            userService = _userService;
        }

        public IActionResult Index ()
        {
            return View();
        }

        public async Task<IActionResult> All (int id = 1)
        {
            const int ItemsPerPage = 6;

            var viewModel = new UsersListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                UserCount = userService.GetCount(),
                Users = userService.GetUsers(id, ItemsPerPage),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ManageUsers ()
        {
            return View();
        }

        public async Task<IActionResult> Edit (string id)
        {
            var model = await userService.GetUserForEdit(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await userService.GetUserForEdit(model.Id);

                return View(model);
            }

            if (await userService.EditUser(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Successful change";

            }

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Delete (string id)
        {
            return Ok();
        }

        public async Task<IActionResult> CreateRole ()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Editor"
            });
            return Ok();
        }
    }
}
