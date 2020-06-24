using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ISearchManager
    {
        StandardSearchResultsList Query(string searchTerm);
    }
}
