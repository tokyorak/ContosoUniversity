using CUniversity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CUniversity.Pages.Courses;

public class DepartmentNamePageModel : PageModel
{
    public SelectList DepartmentNameSL { get; set; }

    public void PopulateDepartmentsDropDownList(SchoolContext _context, object selectedDepartment = null)
    {
        var departmentsQuery = from d in _context.Departments
                                orderby d.Name
                                select d;
        
        DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
    }
}