using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ImportAudit
    {
        public ImportAudit(  DateTime timeStarted, int rowsImported, ImportType importType = 0, string fileName = "")
        {
            TimeStarted = timeStarted;
            RowsImported = rowsImported;
            TimeFinished = DateTime.UtcNow;
            ImportType = importType;
            FileName = fileName;
        }

        public int Id { get; set; }
        public DateTime TimeStarted { get;  set; }
        public DateTime TimeFinished { get;  set; }
        public int RowsImported { get;  set; }
        public ImportType ImportType { get ;  set; }
        public string FileName { get ;  set; }
    }

    public enum ImportType : byte
    {
        IFATEImport = 0,
        LarsImport = 1,
        FrameworkImport =2,
    }
}
