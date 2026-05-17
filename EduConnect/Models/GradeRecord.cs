namespace EduConnect.Models;

public class GradeRecord
{
    public int Id { get; set; } = 0;
    public required int StudentId { get; set; }
    public required int CourseId { get; set; }
    public required double Marks { get; set; }

    // Computed property: letter grade based on marks
    public string LetterGrade
    {
        get
        {
            return Marks switch
            {
                >= 85 => "A",
                >= 70 => "B",
                >= 55 => "C",
                >= 45 => "D",
                _ => "F"
            };
        }
    }

    // Computed property: GPA point value for CGPA calculation
    public double GradePoint
    {
        get
        {
            return LetterGrade switch
            {
                "A" => 4.0,
                "B" => 3.0,
                "C" => 2.0,
                "D" => 1.0,
                _ => 0.0
            };
        }
    }
}
