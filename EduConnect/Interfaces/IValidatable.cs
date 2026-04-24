namespace EduConnect.Interfaces;

public record ValidationResult(bool IsValid, List<string> Errors);

// ISP: Only classes that need validation implement this
public interface IValidatable
{
    ValidationResult Validate();
}
