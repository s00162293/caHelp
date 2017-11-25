using Rad301ClubsV1.Models.ClubModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Rad301ClubsV1.Migrations.ClubModelMigrations
{
    public class CourseData
    {
        public List<Course> Courses = new List<Course>();

        public CourseData()
        {
            if (File.Exists(@".\Courses.csv"))
                Courses = File.ReadAllLines(@".\Courses.csv")
                                               .Select(v => FromCsv(v)).ToList();
            else throw new
                    Exception
            {
                Source = "Course class" + @".\Courses.csv" + " does not exist",


            };
        }

        public static Course FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Course Courses = new Course();
           // Courses.CourseID = values[0];
            Courses.CourseCode = values[1];
            Courses.CourseYear = values[2];
            Courses.CourseName = values[3];
            

            return Courses;
        }
    }
}
