using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    public class SettableContractResolver : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Settable<>))
            {
                Type genericArg = objectType.GetGenericArguments()[0];
                Type converterType = typeof(SettableJsonConverter<>).MakeGenericType(genericArg);
                return (JsonConverter)Activator.CreateInstance(converterType);
            }

            return base.ResolveContractConverter(objectType);
        }
    }
}
