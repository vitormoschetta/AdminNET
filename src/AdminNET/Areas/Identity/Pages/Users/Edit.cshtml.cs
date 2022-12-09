using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AdminNET.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminNET.Areas.Identity.Pages.Users
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public class InputModel
        {
            public string Id { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>();
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                Input.Id = user.Id;
                Input.UserName = user.UserName ?? string.Empty;

                // get all roles
                var roles = await _roleManager.Roles.ToListAsync();

                // get user roles
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    Input.Roles.Add(role.Name, userRoles.Contains(role.Name));
                }
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.Id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

            var selectedRoles = Request.Form["checkRoles"].ToString();
            var roles = selectedRoles.Split(',');

            foreach (var role in roles)
            {
                var rolesUser = await _userManager.GetRolesAsync(user);

                if (rolesUser.Contains(role))
                {
                    continue;
                }

                var roleToAdd = await _roleManager.FindByNameAsync(role);

                if (roleToAdd != null)
                {
                   await _userManager.AddToRoleAsync(user, roleToAdd.Name);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
