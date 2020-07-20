using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IJsonFileHelper
    {
        IEnumerable<T> ParseJsonFile<T>(string filePath);
        string GetLatestFrameworkFileFromDataDirectory();
    }
}