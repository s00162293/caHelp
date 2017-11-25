using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rad301ClubsV1.Models.ClubModel
{
    [Table("StudentCourses")]
    public class StudentCourses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentCourseID { get; set; }
    
        [ForeignKey("course")]
        public int CourseID { get; set; }
 
        [ForeignKey("student")]
        public string StudentID { get; set;}
  
        public virtual Course course { get; set; }
     
        public virtual Student student { get; set; }

    }
}