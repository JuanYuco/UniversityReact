using System;
using System.Collections.Generic;
using System.Linq;
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
                if ( courses == null )
                {
                    return courses;
                }

                foreach( Data.Courses course in coursesDb )
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
                return null;
            }
        }
    }
}