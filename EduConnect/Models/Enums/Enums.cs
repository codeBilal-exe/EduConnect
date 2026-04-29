namespace EduConnect.Models.Enums;

public enum AlertType 
{ 
    Success, 
    Warning, 
    Error, 
    Info 
}

public enum EnrollmentState 
{ 
    Active, 
    Dropped 
}

public enum EnrollmentStatus 
{ 
    Open, 
    AlmostFull, 
    Full 
}

public enum NotificationType 
{ 
    Enrollment, 
    GradePosted, 
    Announcement 
}

public enum UserRole 
{ 
    Admin, 
    Faculty, 
    Student 
}
