using AdminNET.Areas.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminNET.Areas.Identity.Pages.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
            }
        }
    }
}