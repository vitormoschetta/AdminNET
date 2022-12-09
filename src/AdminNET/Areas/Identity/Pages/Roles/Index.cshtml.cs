using AdminNET.Areas.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminNET.Areas.Identity.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IndexModel(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<ApplicationRole> ApplicationRoles { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_roleManager.Roles != null)
            {
                ApplicationRoles = await _roleManager.Roles.ToListAsync();
            }
        }
    }
}