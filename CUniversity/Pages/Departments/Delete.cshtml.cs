using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CUniversity.Data;
using CUniversity.Models;

namespace CUniversity.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly CUniversity.Data.SchoolContext _context;

        public DeleteModel(CUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }
        public string ConcurrencyErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? concurrencyError)
        {
            Department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if(Department == null)
            {
                return NotFound();
            }

            if(concurrencyError.GetValueOrDefault()) // is false by default if null
            {
                ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you selected delete. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again.";
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if(await _context.Departments.AnyAsync(m => m.DepartmentID == id))
                {
                    // Department.ConcurrencyToken value is from when the entity
                    // was fetched. If it doesn't match the DB, a
                    // DbUpdateConcurrencyException exception is thrown
                    _context.Departments.Remove(Department);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./Index");
            }
            catch(DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete", new { concurrencyError = true, id = id});
            }
        }
    }
}