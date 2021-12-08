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
    public class Student
    {
        private UniversityReactEntities db = new UniversityReactEntities();

        public List<Models.Student> GetStudents()
        {
            try
            {
                List<Models.Student> students = new List<Models.Student>();
                var studetnsDb = db.Students.ToList();
                if (studetnsDb == null)
                {
                    return students;
                }

                foreach (Data.Students student in studetnsDb)
                {
                    students.Add(new Models.Student
                    {
                        ID = student.ID,
                        FirstMidName = student.FirstMidName,
                        LastName = student.LastName,
                        EnrollmentDate = student.EnrollmentDate
                    });
                }

                return students;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Student> CreateStudent(Models.Student studentModel)
        {
            try
            {
                var student = db.Students.Add(new Data.Students
                {
                    ID = studentModel.ID,
                    FirstMidName = studentModel.FirstMidName,
                    LastName = studentModel.LastName,
                    EnrollmentDate = studentModel.EnrollmentDate
                });

                await db.SaveChangesAsync();

                studentModel.ID = student.ID;
                return studentModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Student> getStudentById(int id)
        {
            try
            {
                var student = await db.Students.FindAsync(id);
                if (student == null) return null;

                return new Models.Student
                {
                    ID = student.ID,
                    FirstMidName = student.FirstMidName,
                    LastName = student.LastName,
                    EnrollmentDate = student.EnrollmentDate
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Student> UpdateStudent(Models.Student student)
        {
            try
            {
                db.Students.AddOrUpdate(new Data.Students
                {
                    ID = student.ID,
                    FirstMidName = student.FirstMidName,
                    LastName = student.LastName,
                    EnrollmentDate = student.EnrollmentDate
                });

                await db.SaveChangesAsync();
                return student;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                var studentData = db.Students.Find(id);
                db.Students.Remove(studentData);

                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> existsStudentInOtherTable(int id)
        {
            try
            {
                var students = await (from student in db.Students
                                     join enrollment in db.Enrollments on student.ID equals enrollment.StudentID into enro
                                     from en in enro.DefaultIfEmpty()
                                     where en != null
                                     select new Models.Student
                                     {
                                         ID = student.ID
                                     }).FirstOrDefaultAsync(x => x.ID == id);

                return students != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }
    }
}