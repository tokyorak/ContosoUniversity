using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CUniversity.Models;
using CUniversity.Models.SchoolViewModels;

namespace CUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly CUniversity.Data.SchoolContext _context;

        public IndexModel(CUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get;set; } = default!;

        // Alternative way  
        public IList<CourseViewModel> CourseVM { get; set; }


        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();

            // Alternative way
            CourseVM = await _context.Courses
                .Select(p => new CourseViewModel
                {
                    CourseID = p.CourseID,
                    Title = p.Title,
                    Credits = p.Credits,
                    DepartmentName = p.Department.Name
                }).ToListAsync();
            
        }
    }
}
