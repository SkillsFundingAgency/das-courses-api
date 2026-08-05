using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SFA.DAS.Courses.Data.Extensions
{
    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(
            this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            var converter = new ValueConverter<T, string>
            (
                v => Serialise(v),
                v => Deserialise<T>(v)
            );

            var comparer = new ValueComparer<T>
            (
                equalsExpression: (l, r) =>
                    JsonSerializer.Serialize(l, (JsonSerializerOptions)null) == JsonSerializer.Serialize(r, (JsonSerializerOptions)null),

                hashCodeExpression: v =>
                    v == null ? 0 : JsonSerializer.Serialize(v, (JsonSerializerOptions)null).GetHashCode(),

                snapshotExpression: v =>
                    JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, (JsonSerializerOptions)null), (JsonSerializerOptions)null)
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }

        public static string Serialise(object value) => JsonSerializer.Serialize(value);

        public static T Deserialise<T>(string json) where T : class, new()
            => JsonSerializer.Deserialize<T>(json) ?? new T();
    }
}
