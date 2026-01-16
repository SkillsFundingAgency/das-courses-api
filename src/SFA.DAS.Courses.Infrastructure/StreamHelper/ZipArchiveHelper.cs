using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CsvHelper;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Infrastructure.StreamHelper
{
    public class ZipArchiveHelper : IZipArchiveHelper
    {
        public IEnumerable<T> ExtractModelFromCsvFileZipStream<T>(Stream stream, string filePath)
        {

            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                var entry = zip.Entries.FirstOrDefault(m => m.Name.Equals(filePath));

                if (entry == null)
                {
                    return [];
                }

                using (var reader = new StreamReader(entry.Open()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }
    }
}
