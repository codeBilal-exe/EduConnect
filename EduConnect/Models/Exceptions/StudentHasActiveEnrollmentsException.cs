namespace EduConnect.Models.Exceptions;

public class StudentHasActiveEnrollmentsException : Exception
{
    public StudentHasActiveEnrollmentsException(string studentName)
        : base($"Student '{studentName}' has active enrollments and cannot be deleted.")
    {
    }
}
