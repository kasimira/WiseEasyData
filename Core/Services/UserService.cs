using Core.Contracts;
using Core.Models.User;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicatioDbRepository repo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserService (IApplicatioDbRepository _repo, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            repo = _repo;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<EditUserViewModel> GetUserForEdit (string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);
            var roles = roleManager.Roles.ToList()
               .Select(r => new SelectListItem()
               {
                   Text = r.Name,
                   Value = r.Id
               });

            return new EditUserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = roles
            };
        }

        public async Task<bool> EditUser (EditUserViewModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;

                if (model.RoleNames.Count > 0)
                {
                    var roles = userManager.GetRolesAsync(user);

                    foreach (var item in model.RoleNames)
                    {
                        var role = await roleManager.FindByIdAsync(item);
                        var roleExist = await roleManager.RoleExistsAsync(role.Name);
                        if (roleExist)
                        {
                            if (roles.Result.Contains(role.Name))
                            {
                                roles.Result.Remove(role.Name);
                            }
                            else
                            {
                                await userManager.AddToRoleAsync(user, role.Name);
                            }
                        }
                    }

                    if (roles.Result.Count > 0)
                    {
                        foreach (var item in roles.Result)
                        {
                            userManager.RemoveFromRoleAsync(user, item);
                        }
                    }
                }

                try
                {
                    await repo.SaveChangesAsync();
                    result = true;
                }
                catch (Exception)
                {
                    throw new ArgumentException("Тhe user change was not made");
                }
            }

            return result;
        }

        public IEnumerable<AllUsersViewModel> GetUsers (int page, int itemsPerPage)
        {
            int countUsers = GetCount();

            if (itemsPerPage > countUsers)
            {
                itemsPerPage = countUsers;
            }

            return repo.AllReadonly<ApplicationUser>()
                .OrderBy(e => e.FirstName)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(e => new AllUsersViewModel()
                {
                    Id = e.Id,
                    FullName = e.FirstName + " " + e.LastName,
                    Email = e.Email,
                    JoinedDate = e.JoinedDate,
                    Role = userManager.GetRolesAsync(e).Result,
                })
                .ToList();
        }

        public int GetCount ()
        {
            return repo.AllReadonly<ApplicationUser>().Count();
        }
    }
}
