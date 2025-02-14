using System.Collections.Generic;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class ValidationFailures
    {
        private readonly List<string> _warnings = [];
        private readonly List<string> _standardErrors = [];
        private readonly List<string> _errors = [];

        public void AddValidationFailure(ValidationFailureType validationFailureType, string validationFailure)
        {
            switch (validationFailureType)
            {
                case ValidationFailureType.Warning:
                    _warnings.Add(validationFailure);
                    break;
                case ValidationFailureType.StandardError:
                    _standardErrors.Add(validationFailure);
                    break;
                case ValidationFailureType.Error:
                    _errors.Add(validationFailure);
                    break;
            }
        }

        public List<string> Warnings => _warnings;
        public List<string> StandardErrors => _standardErrors;
        public List<string> Errors => _errors;
    }

    public enum ValidationFailureType
    {
        Warning,
        StandardError,
        Error
    }
}
