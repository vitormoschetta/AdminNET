using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminNET.Data;
using AdminNET.Models;
using Microsoft.AspNetCore.Authorization;

namespace AdminNET.Pages.Todos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AdminNET.Data.ApplicationDbContext _context;

        public EditModel(AdminNET.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Todos == null)
            {
                return NotFound();
            }

            var todo =  await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            Todo = todo;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(Todo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TodoExists(Guid id)
        {
          return (_context.Todos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
