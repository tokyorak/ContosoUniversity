using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CUniversity.Data;
using CUniversity.Models;
using CUniversity.Utility;

namespace CUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly CUniversity.Data.SchoolContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(CUniversity.Data.SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        // public IList<Student> Students { get;set; } = default!;
        public PaginatedList<Student> Students { get; set; }    
        
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            // SORTING using System
            NameSort = string.IsNullOrEmpty(sortOrder)? "name_desc" : "";
            DateSort = sortOrder == "Date"? "date_desc" : "Date";

            if(searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // FILTERING
            CurrentFilter = searchString;

            // SORTING Iqueryable isn't sent to DB until turned into a collection
            IQueryable<Student> studentsIQ = from s in _context.Students
                                                select s;

            // FILTERING
            if(!string.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString) || s.FirstMidName.Contains(searchString));
            }
            
            // SORTING 
            switch(sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Students = await PaginatedList<Student>.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        #region OLD OnGetAsync
        // public async Task OnGetAsync(string sortOrder, string searchString)
        // {
        //     // SORTING using System
        //     NameSort = string.IsNullOrEmpty(sortOrder)? "name_desc" : "";
        //     DateSort = sortOrder == "Date"? "date_desc" : "Date";

        //     // FILTERING
        //     CurrentFilter = searchString;

        //     // SORTING Iqueryable isn't sent to DB until turned into a collection
        //     IQueryable<Student> studentsIQ = from s in _context.Students
        //                                         select s;

        //     // FILTERING
        //     if(!string.IsNullOrEmpty(searchString))
        //     {
        //         studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString) || s.FirstMidName.Contains(searchString));
        //     }
            
        //     // SORTING 
        //     switch(sortOrder)
        //     {
        //         case "name_desc":
        //             studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
        //             break;
        //         case "Date":
        //             studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
        //             break;
        //         case "date_desc":
        //             studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
        //             break;
        //         default:
        //             studentsIQ = studentsIQ.OrderBy(s => s.LastName);
        //             break;
        //     }

        //     Students = await studentsIQ.AsNoTracking().ToListAsync();
        // }

        #endregion OLD OnGetAsync
    }
}
