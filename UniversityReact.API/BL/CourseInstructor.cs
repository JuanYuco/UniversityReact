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
    public class CourseInstructor
    {
        private UniversityReactEntities db = new UniversityReactEntities();

        public async Task<List<Models.CourseInstructor>> GetCourseInstructor()
        {
            try
            {
                return await (from courseInstructor in db.CoursesInstructor
                              join instructor in db.Instructors on courseInstructor.InstructorID equals instructor.ID
                              join course in db.Courses on courseInstructor.CourseID equals course.CourseID
                              select new Models.CourseInstructor
                              {
                                  ID = courseInstructor.ID,
                                  CourseID = courseInstructor.CourseID,
                                  InstructorID = courseInstructor.InstructorID,
                                  Course = new Models.Course
                                  {
                                      CourseID = course.CourseID,
                                      Title = course.Title,
                                      Credits = course.Credits
                                  },
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
                return new List<Models.CourseInstructor>();
            }
        }

        public async Task<Models.CourseInstructor> CreateCoursesInstructor(Models.CourseInstructor courseInstructorModel)
        {
            try
            {
                var courseInstructorAdd = db.CoursesInstructor.Add(new Data.CoursesInstructor
                {
                    InstructorID = courseInstructorModel.InstructorID,
                    CourseID = courseInstructorModel.CourseID
                });

                await db.SaveChangesAsync();
                return await getCourseInstructorById(courseInstructorAdd.ID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.CourseInstructor> getCourseInstructorById(int id)
        {
            try
            {
                var courseInstructor = await db.CoursesInstructor
                                                .Include(x => x.Instructors)
                                                .Include(x => x.Courses)
                                                .FirstOrDefaultAsync(x => x.ID == id);

                if (courseInstructor == null) return null;

                return new Models.CourseInstructor
                {
                    ID = courseInstructor.ID,
                    CourseID = courseInstructor.CourseID,
                    InstructorID = courseInstructor.InstructorID,
                    Course = new Models.Course
                    {
                        CourseID = courseInstructor.Courses.CourseID,
                        Title = courseInstructor.Courses.Title,
                        Credits = courseInstructor.Courses.Credits
                    },
                    Instructor = new Models.Instructor
                    {
                        ID = courseInstructor.Instructors.ID,
                        FirstMidName = courseInstructor.Instructors.FirstMidName,
                        LastName = courseInstructor.Instructors.LastName,
                        HireDate = courseInstructor.Instructors.HireDate
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Models.CourseInstructor>> getCourseInstructorByCourse(int id)
        {
            try
            {
                var coursesInstructorData = await db.CoursesInstructor
                                                .Where( x => x.CourseID == id )
                                                .Include(x => x.Instructors)
                                                .Include(x => x.Courses)
                                                .ToListAsync();

                if (coursesInstructorData == null) return new List<Models.CourseInstructor>();

                return convert(coursesInstructorData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Models.CourseInstructor>> getCourseInstructorByInstructor(int id)
        {
            try
            {
                var coursesInstructorData = await db.CoursesInstructor
                                                .Where(x => x.InstructorID == id)
                                                .Include(x => x.Instructors)
                                                .Include(x => x.Courses)
                                                .ToListAsync();

                if (coursesInstructorData == null) return new List<Models.CourseInstructor>();

                return convert(coursesInstructorData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public List<Models.CourseInstructor> convert( List<Data.CoursesInstructor> coursesInstructorsData )
        {
            List<Models.CourseInstructor> courseInstructors = new List<Models.CourseInstructor>();
            foreach (Data.CoursesInstructor courseInstructor in coursesInstructorsData)
            {
                courseInstructors.Add(new Models.CourseInstructor
                {
                    ID = courseInstructor.ID,
                    CourseID = courseInstructor.CourseID,
                    InstructorID = courseInstructor.InstructorID,
                    Course = new Models.Course
                    {
                        CourseID = courseInstructor.Courses.CourseID,
                        Title = courseInstructor.Courses.Title,
                        Credits = courseInstructor.Courses.Credits
                    },
                    Instructor = new Models.Instructor
                    {
                        ID = courseInstructor.Instructors.ID,
                        FirstMidName = courseInstructor.Instructors.FirstMidName,
                        LastName = courseInstructor.Instructors.LastName,
                        HireDate = courseInstructor.Instructors.HireDate
                    }
                });
            }

            return courseInstructors;
        }

        public async Task<Models.CourseInstructor> UpdateCoursesInstructor(Models.CourseInstructor coursesInstructor)
        {
            try
            {
                db.CoursesInstructor.AddOrUpdate(new Data.CoursesInstructor
                {
                    ID = coursesInstructor.ID,
                    InstructorID = coursesInstructor.InstructorID,
                    CourseID = coursesInstructor.CourseID
                });

                await db.SaveChangesAsync();
                return await getCourseInstructorById(coursesInstructor.ID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteCourseInstructor(int id)
        {
            try
            {
                var coursesInstructorData = db.CoursesInstructor.Find(id);
                db.CoursesInstructor.Remove(coursesInstructorData);

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