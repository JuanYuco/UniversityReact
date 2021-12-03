using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UniversityReact.API.Data;

namespace UniversityReact.API.BL
{
    public class Instructor
    {
        private UniversityReactEntities db = new UniversityReactEntities();
        public List<Models.Instructor> GetInstrutors()
        {
            try
            {
                List<Models.Instructor> instructors = new List<Models.Instructor>();
                var instructorsDb = db.Instructors.ToList();
                if (instructors == null)
                {
                    return instructors;
                }

                foreach (Data.Instructors instructor in instructorsDb)
                {
                    instructors.Add(new Models.Instructor {
                        ID = instructor.ID,
                        FirstMidName = instructor.FirstMidName,
                        LastName = instructor.LastName,
                        HireDate = instructor.HireDate
                    });
                }

                return instructors;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Models.Instructor> CreateInstructor(Models.Instructor instructorModel)
        {
            try
            {
                var instructor = db.Instructors.Add(new Data.Instructors
                {
                    ID = instructorModel.ID,
                    FirstMidName = instructorModel.FirstMidName,
                    LastName = instructorModel.LastName,
                    HireDate = instructorModel.HireDate
                });

                await db.SaveChangesAsync();

                instructorModel.ID = instructor.ID;
                return instructorModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Models.Instructor> getIntructorById(int id)
        {
            try
            {
                var instructor = await db.Instructors.FindAsync(id);
                if (instructor == null) return null;

                return new Models.Instructor
                {
                    ID = instructor.ID,
                    FirstMidName = instructor.FirstMidName,
                    LastName = instructor.LastName,
                    HireDate = instructor.HireDate
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Models.Instructor> UpdateInstructor(Models.Instructor instructor)
        {
            try
            {
                db.Instructors.AddOrUpdate(new Data.Instructors
                {
                    ID = instructor.ID,
                    FirstMidName = instructor.FirstMidName,
                    LastName = instructor.LastName,
                    HireDate = instructor.HireDate
                });

                await db.SaveChangesAsync();
                return instructor;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DeleteInstructor(int id)
        {
            try
            {
                var instructorData = db.Instructors.Find(id);
                db.Instructors.Remove(instructorData);

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}