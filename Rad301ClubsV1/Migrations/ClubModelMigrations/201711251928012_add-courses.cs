namespace Rad301ClubsV1.Migrations.ClubModelMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcourses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Course",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        CourseYear = c.String(),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.CourseID);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        StudentCourseID = c.Int(nullable: false, identity: true),
                        CourseID = c.Int(nullable: false),
                        StudentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StudentCourseID)
                .ForeignKey("dbo.Course", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.StudentID)
                .Index(t => t.CourseID)
                .Index(t => t.StudentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentCourses", "StudentID", "dbo.Student");
            DropForeignKey("dbo.StudentCourses", "CourseID", "dbo.Course");
            DropIndex("dbo.StudentCourses", new[] { "StudentID" });
            DropIndex("dbo.StudentCourses", new[] { "CourseID" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.Course");
        }
    }
}
