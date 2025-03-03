using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities;

public class StandardApprenticeshipType
{
    public const string DefaultApprenticeshipType = "Apprenticeship";
    public string IfateReferenceNumber { get; set; }
    public string ApprenticeshipType { get; set; }

    public virtual ICollection<Standard> Standards { get; set; }
}
