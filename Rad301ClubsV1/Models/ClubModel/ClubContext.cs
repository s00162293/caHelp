﻿
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Rad301ClubsV1.Models.ClubModel
{
    
    public class ClubContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }
        
        public ClubContext()
            : base("DefaultConnection")
        {
        }

        public static ClubContext Create()
        {
            return new ClubContext();
        }
    }
}