using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CUniversity.Models;
using CUniversity.Models.SchoolViewModels;

namespace CUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly CUniversity.Data.SchoolContext _context;

        public IndexModel(CUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseID)
        {
            InstructorData = new InstructorIndexData();
            
            InstructorData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses).ThenInclude(c => c.Department)
                .OrderBy(i => i.LastName)
                .ToListAsync();
            
            if(id != null)
            {
                InstructorID = id.Value;
                Instructor instructor = InstructorData.Instructors.Where(i => i.ID == id.Value).Single();
                InstructorData.Courses = instructor.Courses;
            }

            if(courseID != null)
            {
                CourseID = courseID.Value;
                var selectedCourse = InstructorData.Courses.Where(c => c.CourseID == courseID.Value).Single();
                
                // Loads Enrollments for the selected Course, since Course - Enrollments is a one-to-many relationship so we use Collection().LoadAsync()
                await _context.Entry(selectedCourse).Collection(c => c.Enrollments).LoadAsync();
                foreach(Enrollment enrollment in selectedCourse.Enrollments)
                {
                    // Loads in each retrieved enrollment the student who signed for. Enrollment - Student is a one-to-one relationship so we use Reference().LoadAsync()
                    await _context.Entry(enrollment).Reference(e => e.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}
