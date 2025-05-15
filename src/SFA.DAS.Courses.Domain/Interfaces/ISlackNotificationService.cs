using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISlackNotificationService
    {
        Task UploadFile(List<string> content, string fileName, string message);
        string FormattedTag();

    }
}
