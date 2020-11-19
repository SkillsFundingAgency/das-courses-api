using System.Threading.Tasks;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardDocumentRepository
    {
        string SaveDocument(string key, string content);
    }
}
