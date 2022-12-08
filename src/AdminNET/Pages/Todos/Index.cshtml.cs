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
    public class IndexModel : PageModel
    {
        private readonly AdminNET.Data.ApplicationDbContext _context;

        public IndexModel(AdminNET.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Todo> Todo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Todos != null)
            {
                Todo = await _context.Todos.ToListAsync();
            }
        }
    }
}
