using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UniversityReact.API.Data;

namespace UniversityReact.API.BL
{
    public class OfficeAssignment
    {
        private UniversityReactEntities db = new UniversityReactEntities();

        public async Task<List<Models.OfficeAssignment>> GetOfficesAssignment()
        {
            try
            {
                return await (from officeAssignment in db.OfficesAssignment
                                join instructor in db.Instructors on officeAssignment.InstructorID equals instructor.ID
                                select new Models.OfficeAssignment
                                {
                                    InstructorID = officeAssignment.InstructorID,
                                    Location     = officeAssignment.Location,
                                    Instructor   = new Models.Instructor
                                    {
                                        ID = instructor.ID,
                                        FirstMidName = instructor.FirstMidName,
                                        LastName = instructor.LastName,
                                        HireDate = instructor.HireDate
                                    }
                                }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Models.OfficeAssignment>();
            }
        }
    }
}