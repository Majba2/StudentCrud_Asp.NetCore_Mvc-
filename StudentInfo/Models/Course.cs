using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInfo.Models
{
    public class Course
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int CourseID { get; set; }
        public string? CourseName { get; set; }

        public virtual IList<Enrollment>? Enrollments { get; set; }

    }
}
