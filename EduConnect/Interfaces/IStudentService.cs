using EduConnect.Models;

namespace EduConnect.Interfaces;

// ISP: Extends IRepository but doesn't include grade methods (those belong to IGradeService)
// SRP: Only manages student data
public interface IStudentService : IRepository<Student>
{
    List<Student> Search(string term);
    double ComputeCGPA(Guid studentId);
}
