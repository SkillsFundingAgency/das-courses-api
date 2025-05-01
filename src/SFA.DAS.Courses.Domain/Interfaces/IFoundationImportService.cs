using System;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces;

public interface IFoundationImportService
{
    Task ImportDataIntoStaging();
}
