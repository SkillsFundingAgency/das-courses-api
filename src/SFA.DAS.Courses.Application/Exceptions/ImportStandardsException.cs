using System;

namespace SFA.DAS.Courses.Application.Exceptions
{
    public class ImportStandardsException : Exception
    {
        public ImportStandardsException(string message, Exception innerException) 
            : base(message, innerException)
        { 
        }
    }
}
