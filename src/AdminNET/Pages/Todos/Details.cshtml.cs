using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminNET.Data;
using AdminNET.Models;
using Microsoft.AspNetCore.Authorization;

namespace AdminNET.Pages.Todos
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly AdminNET.Data.ApplicationDbContext _context;

        public DetailsModel(AdminNET.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Todo Todo { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Todos == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            else 
            {
                Todo = todo;
            }
            return Page();
        }
    }
}
