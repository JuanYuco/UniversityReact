using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

        public async Task<Models.OfficeAssignment> CreateOfficeAssignment(Models.OfficeAssignment officeAssignmentModel)
        {
            try
            {
                var officeAssignment = db.OfficesAssignment.Add(new Data.OfficesAssignment {
                    InstructorID = officeAssignmentModel.InstructorID,
                    Location = officeAssignmentModel.Location
                });

                await db.SaveChangesAsync();
                return await getOfficeAssignmentById(officeAssignment.InstructorID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.OfficeAssignment> getOfficeAssignmentById(int id)
        {
            try
            {
                var officeAssignment = await db.OfficesAssignment.Include( x => x.Instructors ).FirstOrDefaultAsync( x => x.InstructorID == id );
                if (officeAssignment == null) return null;

                return new Models.OfficeAssignment
                {
                    InstructorID = officeAssignment.InstructorID,
                    Location = officeAssignment.Location,
                    Instructor = new Models.Instructor
                    {
                        ID = officeAssignment.Instructors.ID,
                        FirstMidName = officeAssignment.Instructors.FirstMidName,
                        LastName = officeAssignment.Instructors.LastName,
                        HireDate = officeAssignment.Instructors.HireDate
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.OfficeAssignment> UpdateOfficeAssignment(Models.OfficeAssignment officeAssignment)
        {
            try
            {
                db.OfficesAssignment.AddOrUpdate(new Data.OfficesAssignment
                {
                    InstructorID = officeAssignment.InstructorID,
                    Location = officeAssignment.Location
                });

                await db.SaveChangesAsync();
                return await getOfficeAssignmentById(officeAssignment.InstructorID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteOfficeAssignment(int id)
        {
            try
            {
                var officesAssignmentData = db.OfficesAssignment.Find(id);
                db.OfficesAssignment.Remove(officesAssignmentData);

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}