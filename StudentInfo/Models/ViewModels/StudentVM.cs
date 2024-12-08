namespace StudentInfo.Models.ViewModels
{
    public class StudentVM
    {
        public  StudentVM()
        {
           this. Enrollment = new List<Enrollment>();
        }
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public int StudentAge { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }   
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual List<Enrollment> Enrollment { get; set; }
    }
}
