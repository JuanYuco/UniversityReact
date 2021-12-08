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
    public class Enrollment
    {
        private UniversityReactEntities db = new UniversityReactEntities();
        public async Task<List<Models.Enrollment>> GetEnrollments()
        {
            try
            {
                return await (from enrollment in db.Enrollments
                              join student in db.Students on enrollment.StudentID equals student.ID
                              join course in db.Courses on enrollment.CourseID equals course.CourseID
                              select new Models.Enrollment
                              {
                                  EnrollmentID = enrollment.EnrollmentID,
                                  StudentID = enrollment.StudentID,
                                  CourseID = enrollment.CourseID,
                                  Grade = enrollment.Grade,
                                  Course = new Models.Course
                                  {
                                      CourseID = course.CourseID,
                                      Title = course.Title,
                                      Credits = course.Credits
                                  },
                                  Student = new Models.Student
                                  {
                                      ID = student.ID,
                                      FirstMidName = student.FirstMidName,
                                      EnrollmentDate = student.EnrollmentDate,
                                      LastName = student.LastName
                                  }
                              }).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Models.Enrollment>();
            }
        }

        public async Task<Models.Enrollment> CreateEnrollment(Models.Enrollment enrollmentModel)
        {
            try
            {
                var enrollmentAdd = db.Enrollments.Add(new Data.Enrollments
                {
                    CourseID = enrollmentModel.CourseID,
                    StudentID = enrollmentModel.StudentID,
                    Grade = enrollmentModel.Grade
                });

                await db.SaveChangesAsync();
                return await getEnrollmentById(enrollmentAdd.EnrollmentID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Enrollment> getEnrollmentById(int id)
        {
            try
            {
                var enrollment = await db.Enrollments
                                         .Include(x => x.Students)
                                         .Include(x => x.Courses)
                                         .FirstOrDefaultAsync(x => x.EnrollmentID == id);

                if (enrollment == null) return null;

                return new Models.Enrollment
                {
                    EnrollmentID = enrollment.EnrollmentID,
                    StudentID = enrollment.StudentID,
                    CourseID = enrollment.CourseID,
                    Grade = enrollment.Grade,
                    Course = new Models.Course
                    {
                        CourseID = enrollment.Courses.CourseID,
                        Title = enrollment.Courses.Title,
                        Credits = enrollment.Courses.Credits
                    },
                    Student = new Models.Student
                    {
                        ID = enrollment.Students.ID,
                        FirstMidName = enrollment.Students.FirstMidName,
                        EnrollmentDate = enrollment.Students.EnrollmentDate,
                        LastName = enrollment.Students.LastName
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Models.Enrollment>> getEnrollmentByCourse(int id)
        {
            try
            {
                var enrollmentsData = await db.Enrollments
                                                .Where(x => x.CourseID == id)
                                                .Include(x => x.Students)
                                                .Include(x => x.Courses)
                                                .ToListAsync();

                if (enrollmentsData == null) return new List<Models.Enrollment>();

                return convert(enrollmentsData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Models.Enrollment>> getEnrollmentByStudent(int id)
        {
            try
            {
                var enrollmentsData = await db.Enrollments
                                                .Where(x => x.StudentID == id)
                                                .Include(x => x.Students)
                                                .Include(x => x.Courses)
                                                .ToListAsync();

                if (enrollmentsData == null) return new List<Models.Enrollment>();

                return convert(enrollmentsData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<bool> existsEnrollment(int ?coursesId, int ?studentId, int ?id)
        {
            try
            {
                var enrollment = await db.Enrollments
                                                .Where(x => x.CourseID == coursesId && x.StudentID == studentId && x.EnrollmentID != id)
                                                .FirstOrDefaultAsync();

                return enrollment != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }

        public List<Models.Enrollment> convert(List<Data.Enrollments> enrollmentsData)
        {
            List<Models.Enrollment> enrollments = new List<Models.Enrollment>();
            foreach (Data.Enrollments enrollment in enrollmentsData)
            {
                enrollments.Add(new Models.Enrollment
                {
                    EnrollmentID = enrollment.EnrollmentID,
                    StudentID = enrollment.StudentID,
                    CourseID = enrollment.CourseID,
                    Grade = enrollment.Grade,
                    Course = new Models.Course
                    {
                        CourseID = enrollment.Courses.CourseID,
                        Title = enrollment.Courses.Title,
                        Credits = enrollment.Courses.Credits
                    },
                    Student = new Models.Student
                    {
                        ID = enrollment.Students.ID,
                        FirstMidName = enrollment.Students.FirstMidName,
                        LastName = enrollment.Students.LastName,
                        EnrollmentDate = enrollment.Students.EnrollmentDate
                    }
                });
            }

            return enrollments;
        }

        public async Task<Models.Enrollment> UpdateEnrollment(Models.Enrollment enrollment)
        {
            try
            {
                db.Enrollments.AddOrUpdate(new Data.Enrollments
                {
                    EnrollmentID = enrollment.EnrollmentID,
                    StudentID = enrollment.StudentID,
                    CourseID = enrollment.CourseID,
                    Grade = enrollment.Grade
                });

                await db.SaveChangesAsync();
                return await getEnrollmentById(enrollment.EnrollmentID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteEnrollment(int id)
        {
            try
            {
                var enrollmentData = db.Enrollments.Find(id);
                db.Enrollments.Remove(enrollmentData);

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