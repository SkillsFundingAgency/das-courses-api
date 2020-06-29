using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.PageParsing
{
    public class LarsPageParser : ILarsPageParser
    {
        public async Task<string> GetCurrentLarsDataDownloadFilePath()
        {
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(Constants.LarsDownloadPageUrl);

            var downloadHref = document
                .QuerySelectorAll("span:contains('LARS CSV')")
                .First()
                .ParentElement.GetAttribute("Href");
            
            var uri = new Uri(new Uri(Constants.LarsBasePageUrl),downloadHref);

            return uri.AbsoluteUri;
        }
    }
}