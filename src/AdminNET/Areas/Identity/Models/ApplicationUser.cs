using Microsoft.AspNetCore.Identity;

namespace AdminNET.Areas.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Document { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
    }
}