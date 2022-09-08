using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

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
                    JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),

                hashCodeExpression: v =>
                    v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),

                snapshotExpression: v =>
                    JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }

        public static string Serialise(object value) => JsonConvert.SerializeObject(value);

        public static T Deserialise<T>(string json) where T : class, new()
            => JsonConvert.DeserializeObject<T>(json) ?? new T();
    }
}
