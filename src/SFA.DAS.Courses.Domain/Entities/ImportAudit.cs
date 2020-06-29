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
        public DateTime TimeStarted { get;  }
        public DateTime TimeFinished { get;  }
        public int RowsImported { get;  }
        public ImportType ImportType { get ;  }
        public string FileName { get ;  }
    }

    public enum ImportType : byte
    {
        IFATEImport = 0,
        LarsImport = 1,
    }
}
