using System.Collections.Generic;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetOptions
{
    public struct GetStandardOptionsResult
    {
        public string StandardUId { get; set; }
        public IEnumerable<string> Options { get; set; }
        public GetStandardOptionsResult(string standardUId, IEnumerable<string> options)
        {
            StandardUId = standardUId;
            Options = options;
        }
    }
}
