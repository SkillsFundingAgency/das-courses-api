using System;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Validators.RequiredFieldPresentValidatorTests
{
    public static class JsonHelper
    {
        public static string GetJsonPropertyName<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            var jsonPropertyAttribute = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                                                .Cast<JsonPropertyAttribute>()
                                                .FirstOrDefault();

            return jsonPropertyAttribute?.PropertyName ?? propertyName;
        }
    }
}
