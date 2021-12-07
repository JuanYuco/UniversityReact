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
    public class Department
    {
        private UniversityReactEntities db = new UniversityReactEntities();

        public async Task<List<Models.Department>> GetDepartments()
        {
            try
            {
                return await (from department in db.Departments
                              join instructor in db.Instructors on department.InstructorID equals instructor.ID
                              select new Models.Department
                              {
                                  DepartmentID = department.DepartmentID,
                                  Name = department.Name,
                                  Budget = department.Budget,
                                  StartDate = department.StartDate,
                                  InstructorID = department.InstructorID,
                                  Instructor = new Models.Instructor
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
                return new List<Models.Department>();
            }
        }

        public async Task<Models.Department> CreateDepartment(Models.Department departmentModel)
        {
            try
            {
                var department = db.Departments.Add(new Data.Departments
                {
                    DepartmentID = departmentModel.DepartmentID,
                    Name = departmentModel.Name,
                    Budget = departmentModel.Budget,
                    StartDate = departmentModel.StartDate,
                    InstructorID = departmentModel.InstructorID
                });

                await db.SaveChangesAsync();
                return await getDepartmentById(department.DepartmentID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Department> getDepartmentById(int id)
        {
            try
            {
                var department = await db.Departments.Include(x => x.Instructors).FirstOrDefaultAsync(x => x.DepartmentID == id);
                if (department == null) return null;

                return new Models.Department
                {
                    DepartmentID = department.DepartmentID,
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    InstructorID = department.InstructorID,
                    Instructor = new Models.Instructor
                    {
                        ID = department.Instructors.ID,
                        FirstMidName = department.Instructors.FirstMidName,
                        LastName = department.Instructors.LastName,
                        HireDate = department.Instructors.HireDate
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Department> UpdateDepartment(Models.Department department)
        {
            try
            {
                db.Departments.AddOrUpdate(new Data.Departments
                {
                    DepartmentID = department.DepartmentID,
                    Name = department.Name,
                    Budget = department.Budget,
                    StartDate = department.StartDate,
                    InstructorID = department.InstructorID
                });

                await db.SaveChangesAsync();
                return await getDepartmentById(department.DepartmentID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteDepartment(int id)
        {
            try
            {
                var departamentData = db.Departments.Find(id);
                db.Departments.Remove(departamentData);

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