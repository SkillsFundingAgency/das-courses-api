using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.FileHelper
{
    public class JsonFileHelper : IJsonFileHelper
    {
        public IEnumerable<T> ParseJsonFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json).ToList();
            }
        }

        public string GetLatestFrameworkFileFromDataDirectory()
        {
            var directoryInfo = new DirectoryInfo("DataFiles");

            var file = directoryInfo.GetFiles().ToList().OrderByDescending(c => c.Name).FirstOrDefault();

            return file?.FullName ?? "";
        }
    }
}