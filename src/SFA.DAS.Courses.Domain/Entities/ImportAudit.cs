using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ImportAudit
    {
        public ImportAudit(DateTime timeStarted, int rowsImported)
        {
            TimeStarted = timeStarted;
            RowsImported = rowsImported;
            TimeFinished = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime TimeFinished { get; set; }
        public int RowsImported { get; set; }
    }
}