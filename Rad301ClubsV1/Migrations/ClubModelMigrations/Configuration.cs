namespace Rad301ClubsV1.Migrations.ClubModelMigrations
{
    using CsvHelper;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.ClubModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Rad301ClubsV1.Models.ClubModel.ClubContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ClubModelMigrations";
        }

        protected override void Seed(Rad301ClubsV1.Models.ClubModel.ClubContext context)
        {
         //   seedStudents(context);
         //   seedClubs(context);     
         //   SeedClubMembers(context);
           // seedAdmin(context);
            SeedCourses(context);
        }

        private void seedClubs(ClubContext context)
        {
            #region club1
            context.Clubs.AddOrUpdate(c => c.ClubName,
                        new Club
                        {
                            ClubName = "The Tiddly Winks Club",
                            CreationDate = DateTime.Now,
                            adminID = -1,
                            //clubMembers = SeedMembers(context),
                            clubEvents = new List<ClubEvent>()
                        {	// Create a new ClubEvent 
                        new ClubEvent { StartDateTime = DateTime.Now.Subtract( new TimeSpan(5,0,0,0,0)),
                           EndDateTime = DateTime.Now.Subtract( new TimeSpan(5,0,0,0,0)),
                           Location="Sligo", Venue="Arena",
                          // attendees = selectedMembers // You?ll need to comment this out on subsequent seeds
                        },
                        new ClubEvent { StartDateTime = DateTime.Now.Subtract( new TimeSpan(3,0,0,0,0)),
                           EndDateTime = DateTime.Now.Subtract( new TimeSpan(3,0,0,0,0)),
                           Location="Sligo", Venue="Main Canteen"
        },
                        }
                        });
            #endregion
        
            #region club2
            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club {
                    ClubName = "The Chess Club",
                    CreationDate = DateTime.Now,
                    adminID = -1,
                    // clubMembers = SeedMembers(context)

                    //clubMembers = GetStudents(context)
                });
                    #endregion
         context.SaveChanges(); // NOTE EF will update the relevant foreign key fields in the clubs, club events and member tables based on the attributes

        }
        /*
        private static List<Member> SeedMembers(ClubContext context)
        {
            List<Member> selectedMembers = new List<Member>();
            //get 10 random set of students
            var randomSetStudents = context.Students.Select(s => new { s.StudentID, r = Guid.NewGuid() });
            List<string> subset = randomSetStudents.OrderBy(s => s.r).Select(s => s.StudentID.ToString()).Take(10).ToList();
            foreach (string s in subset)
            {
                selectedMembers.Add(
                   new Member { StudentID = s }
                   );
            }
            return selectedMembers;
        } */
        public void seedStudents(Rad301ClubsV1.Models.ClubModel.ClubContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Rad301ClubsV1.Migrations.ClubModelMigrations.TestStudents.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    var testStudents = csvReader.GetRecords<Student>().ToArray();
                    context.Students.AddOrUpdate(s => s.StudentID, testStudents);
                }

            }
        }

        //seed new club member
        private void SeedClubMembers(ClubContext context)
        {
            // Create a list to hold students
            List<Student> selectedStudents = new List<Student>();

            //save newly created clubs first , then retrieve them as a list
            foreach (var club in context.Clubs.ToList())
            {
                //set member if not set yet
                if (club.clubMembers== null ||  club.clubMembers.Count() < 1)
                {
                    //set randoms one --method below
                    selectedStudents = GetStudents(context);
                    foreach (var m in selectedStudents)
                    {
                        //new member with a ref to a club ,EF will join fields later
                        context.members.AddOrUpdate(member => member.StudentID,
                            new Member { ClubId = club.ClubId, StudentID= m.StudentID});

                    }
                }
            }     
            context.SaveChanges();
        }

        //random students method
        private List<Student> GetStudents(ClubContext context)
        {
            //random list of srudent ids
            var randomSetStudent = context.Students.Select(s => new { s.StudentID, r = Guid.NewGuid() });
            //sort and take 10
            List<string> subset = randomSetStudent.OrderBy(s => s.r)
                .Select(s => s.StudentID.ToString()).Take(10).ToList();
            //return sel students as a relaized list
            return context.Students.Where(s => subset.Contains(s.StudentID)).ToList();
        }

        //seed admin
        private void seedAdmin(ClubContext context)
        {
            List<Club> clubs = context.Clubs.Include("clubMembers").ToList();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //get the first
                var manager =
               new UserManager<ApplicationUser>(
                   new UserStore<ApplicationUser>(db));

                foreach (Club c in clubs)
                {
                    c.adminID = c.clubMembers.First().memberID;

                    db.Users.AddOrUpdate(u => u.Email, new ApplicationUser
                    {
                        StudentID = c.clubMembers.First().StudentID,
                        Email = c.clubMembers.First().StudentID + "@mail.itsligo.ie",
                        DateJoined = DateTime.Now,
                        EmailConfirmed = true,
                        UserName = c.clubMembers.First().StudentID + "@mail.itsligo.ie",
                        PasswordHash = new PasswordHasher().HashPassword(c.clubMembers.First().StudentID + "$1"),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    });
                    db.SaveChanges();

                    ApplicationUser user = manager.FindByEmail(c.clubMembers.First().StudentID + "@mail.itsligo.ie");
                    if (user != null)                
                      manager.AddToRole(user.Id, "ClubAdmin");               
                    
                }
                context.Clubs.AddOrUpdate(c => c.ClubName, clubs.ToArray());
            }

        
        }

        //seed courses details 
        public static void SeedCourses(ClubContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Rad301ClubsV1.Migrations.ClubModelMigrations.Courses.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    var courseData = csvReader.GetRecords<CourseDataImport>().ToArray();
                    foreach (var dataItem in courseData)
                    {
                        context.Courses.AddOrUpdate(c =>
                                new { c.CourseCode, c.CourseName },
                                new Course
                                {
                                    CourseCode = dataItem.CourseCode,
                                    CourseName = dataItem.CourseName,
                                    CourseYear = dataItem.Year
                                });

                    }
                }
            }
            context.SaveChanges();
        }




    }

}
