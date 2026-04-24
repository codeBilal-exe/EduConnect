namespace EduConnect.Models.Exceptions;

public class CourseFullException : Exception
{
    public CourseFullException(string courseTitle)
        : base($"Course '{courseTitle}' is full and cannot accept more enrollments.")
    {
    }
}
