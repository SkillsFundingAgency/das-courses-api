using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    [ExcludeFromCodeCoverage]
    public class InitializeSettablesJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return Attribute.IsDefined(objectType, typeof(InitializeSettablesAttribute));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // the current convertor will be re-entered and cause a recursive stack overflow if not removed
            var settings = new JsonSerializerSettings
            {
                ContractResolver = serializer.ContractResolver,
                Converters = serializer.Converters
                    .Where(c => c.GetType() != typeof(InitializeSettablesJsonConverter))
                    .ToList()
            };

            var safeSerializer = JsonSerializer.Create(settings);

            var obj = safeSerializer.Deserialize(reader, objectType);
            InitializeSettablesHelper.InitializeSettableProperties(obj);
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
