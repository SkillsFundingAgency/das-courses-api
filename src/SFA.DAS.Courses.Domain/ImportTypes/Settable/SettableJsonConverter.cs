using System;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    public class SettableJsonConverter<T> : JsonConverter<Settable<T>>
    {
        public override Settable<T> ReadJson(JsonReader reader, Type objectType, Settable<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return new Settable<T>(default);

            try
            {
                var value = serializer.Deserialize<T>(reader);
                value = typeof(T) == typeof(string) ? (T)(object)value?.ToString()?.Trim() : value; // Trim string values to avoid leading/trailing spaces
                return new Settable<T>(value);
            }
            catch (Exception)
            {
                var rawValue = reader.Value;
                return Settable<T>.FromInvalidValue(rawValue);
            }
        }

        public override void WriteJson(JsonWriter writer, Settable<T> value, JsonSerializer serializer)
        {
            if (!value.IsSet)
            {
                return;
            }

            if (value.HasValue)
            {
                serializer.Serialize(writer, value.Value);
            }
            else if (value.HasInvalidValue)
            {
                serializer.Serialize(writer, value.InvalidValue);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }

}
