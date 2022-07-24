using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CUniversity.Data;
using CUniversity.Models;

namespace CUniversity.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly CUniversity.Data.SchoolContext _context;

        public DetailsModel(CUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

      public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            else 
            {
                Instructor = instructor;
            }
            return Page();
        }
    }
}
