using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class ValidatorBase<T> : AbstractValidator<T>
    {
        private readonly ValidationFailureType _validationFailureType;

        public ValidatorBase(ValidationFailureType validationFailureType)
        {
            _validationFailureType = validationFailureType;
        }

        public ValidationFailureType ValidationFailureType => _validationFailureType;
        protected static string ReferenceNumber(Standard importedStandard)
        {
            if (!importedStandard.ReferenceNumber.IsSet)
            {
                return "UNKNOWN";
            }
            else if (importedStandard.ReferenceNumber.HasInvalidValue)
            {
                return importedStandard.ReferenceNumber.InvalidValue.ToString();
            }

            return importedStandard.ReferenceNumber.Value;
        }

        protected static string Version(Standard importedStandard)
        {
            if (!importedStandard.Version.IsSet)
            {
                return "UNKNOWN";
            }
            else if (importedStandard.Version.HasInvalidValue)
            {
                return importedStandard.Version.InvalidValue.ToString();
            }

            return importedStandard.Version.Value;
        }
    }
}
