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
    public class Course
    {
        private UniversityReactEntities db = new UniversityReactEntities();
        public List<Models.Course> GetCourses()
        {
            try
            {
                List<Models.Course> courses = new List<Models.Course>();
                var coursesDb = db.Courses.ToList();
                if (courses == null)
                {
                    return courses;
                }

                foreach (Data.Courses course in coursesDb)
                {
                    courses.Add(new Models.Course
                    {
                        CourseID = course.CourseID,
                        Title = course.Title,
                        Credits = course.Credits
                    });
                }

                return courses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Course> CreateCourse(Models.Course courseModel)
        {
            try
            {
                var course = db.Courses.Add( new Data.Courses { 
                    CourseID = courseModel.CourseID,
                    Title = courseModel.Title,
                    Credits = courseModel.Credits
                });

                await db.SaveChangesAsync();

                courseModel.CourseID = course.CourseID;
                return courseModel;
            }catch( Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Course> getCourseById(int id)
        {
            try
            {
                var course = await db.Courses.FindAsync(id);
                if (course == null) return null;

                return new Models.Course
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Credits = course.Credits
                };
            }catch ( Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Models.Course> UpdateCourse(Models.Course course)
        {
            try
            {
                db.Courses.AddOrUpdate( new Data.Courses { 
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Credits = course.Credits
                });

                await db.SaveChangesAsync();
                return course;
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool DeleteCourse(int id)
        {
            try
            {
                var courseData = db.Courses.Find(id);
                db.Courses.Remove(courseData);

                db.SaveChanges();
                return true;
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> existsCourseInOtherTable(int id)
        {
            try
            {
                var courses = await (from course in db.Courses
                                         join courseInstructor in db.CoursesInstructor on course.CourseID equals courseInstructor.CourseID into coin
                                         from ci in coin.DefaultIfEmpty()
                                         join enrollment in db.Enrollments on course.CourseID equals enrollment.CourseID into enro
                                         from en in enro.DefaultIfEmpty()
                                         where ci != null || en != null
                                         select new Models.Course
                                         {
                                             CourseID = course.CourseID
                                         }).FirstOrDefaultAsync(x => x.CourseID == id);

                return courses != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }
    }
}