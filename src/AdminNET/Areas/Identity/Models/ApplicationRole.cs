using Microsoft.AspNetCore.Identity;

namespace AdminNET.Areas.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}