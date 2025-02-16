namespace SFA.DAS.Courses.Domain.Entities;
public class StandardApprenticeshipType
{
    public string IfateReferenceNumber { get; set; }
    public string ApprenticeshipType { get; set; }

    public virtual Standard Standard { get; set; }
}
