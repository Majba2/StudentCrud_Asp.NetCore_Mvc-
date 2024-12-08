using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInfo.Models
{
    public class Enrollment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int EnrollmentID { get; set; }
        [ForeignKey("Student")]
        public int StudentID { get; set; }
        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }

    }
}
