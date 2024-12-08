using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentInfo.Models;
using StudentInfo.Models.ViewModels;

namespace StudentInfo.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentDbContext db;
        private readonly IWebHostEnvironment en;

        public StudentController(StudentDbContext db, IWebHostEnvironment en)
        {
            this.db = db;
            this.en = en;
        }
        public IActionResult Index()
        {
            var students = db.Students.Include(e => e.Enrollments).ThenInclude(c => c.Course).OrderByDescending
                (s => s.StudentID).ToList();
            return View(students);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.courses = new SelectList(db.Courses.ToList(), "CourseID", "CourseName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentVM studentVM, int[] courseID)
        {
            if (ModelState.IsValid)
            {
                var student = new Student()
                {
                    StudentName = studentVM.StudentName,
                    StudentAge = studentVM.StudentAge,
                    DateOfBirth = studentVM.DateOfBirth,
                    IsActive = studentVM.IsActive,
                };
                string file = DateTime.Now.Ticks.ToString() + Path.GetExtension(studentVM.ImageFile.FileName);
                string filePath = en.WebRootPath + "/Images/" + file;
                using (var strem = System.IO.File.Create(filePath))
                {
                    studentVM.ImageFile.CopyTo(strem);
                }
                student.image = "/Images/" + file;
                foreach (var item in courseID)
                {
                    Enrollment enrollment = new Enrollment() {
                        Student = student,
                        StudentID = student.StudentID,
                        CourseID = item

                    };
                    await db.Enrollments.AddAsync(enrollment);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");


            }
            return View(studentVM);
        }
        public async Task<IActionResult> AddNewCourses(int? id)
        {
            ViewBag.courses = new SelectList(await db.Courses.ToListAsync(), "CourseID", "CourseName", id ?? 0);
            return PartialView("_addNewCourses");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.courses = new SelectList(db.Courses.ToList(), "CourseID", "CourseName");
            if (id != null)
            {
                var student = await db.Students.FindAsync(id);
                var courses = await db.Enrollments.Where(c => c.StudentID == student.StudentID).ToListAsync();
                var stud = new StudentVM()
                {
                    StudentID = student.StudentID,
                    StudentName = student.StudentName,
                    Image = student.image,
                    IsActive = student.IsActive,
                    DateOfBirth = student.DateOfBirth,
                };
                stud.Enrollment = courses;
                return View(stud);

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentVM studentVM, int[] courseID) 
        {
            if (ModelState.IsValid)
            {
              var student= await db.Students.FindAsync(studentVM.StudentID);
                if (student != null) 
                {
                    student.StudentID = studentVM.StudentID;
                 student.StudentName= studentVM.StudentName;
                    student.DateOfBirth= studentVM.DateOfBirth;
                    student.StudentAge = studentVM.StudentAge;
                    student.IsActive = studentVM.IsActive;
                    //FOr update
                    var existingEnrollment = db.Enrollments.Where(s => s.StudentID == student.StudentID);
                    db.RemoveRange(existingEnrollment);
                    foreach (var item in courseID) {
                        db.Enrollments.Add(new Enrollment { StudentID = student.StudentID, CourseID = item });
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(studentVM);
        }
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id != null) 
            {
              var student = await db.Students.FindAsync(id);
                var course= await db.Enrollments.Where(s => s.StudentID == student.StudentID).ToListAsync();
                db.Enrollments.RemoveRange(course);
                db.Students.Remove(student);    
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            
            }
         return NotFound();
        }
    }
}
