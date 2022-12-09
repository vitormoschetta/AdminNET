using AdminNET.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminNET.Areas.Identity.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DeleteModel(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationRole Input { get; set; } = null!;


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            else
            {
                Input = role;
            }
            return Page();
        }


        public async Task<IActionResult> OnPost(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role not found");
            }
            return Page();
        }
    }
}