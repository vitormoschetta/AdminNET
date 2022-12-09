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
        private readonly IdentityUserRole<string> _userRole;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IdentityUserRole<string> userRole)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRole = userRole;
        }

        public class InputModel
        {
            public string Id { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
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

                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();

                Input.Roles = userRoles;
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

            user.UserName = Input.UserName;

           

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
