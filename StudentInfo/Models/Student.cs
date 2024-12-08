using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInfo.Models
{
    public class Student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public int StudentAge { get; set; }
        public string? image { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual IList<Enrollment>? Enrollments { get; set; }

    }
}
