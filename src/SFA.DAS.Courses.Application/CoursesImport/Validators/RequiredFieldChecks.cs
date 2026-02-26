using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    internal static class RequiredFieldChecks
    {
        private static readonly ConcurrentDictionary<(Type TargetType, string PropName), string> _jsonNameCache = new();
        private static readonly ConcurrentDictionary<string, JsonMetadata> _metadataCache = new();

        public static void RequireSet<TTarget, T>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<T>>> propertyExpr)
        {
            var jsonData = GetJsonMetadata(propertyExpr);

            var settable = (Settable<T>)jsonData.Getter(target)!;
            undefinedFields[jsonData.JsonName] = !settable.IsSet;
        }

        public static void RequireSetAt<TTarget, T>(
            Dictionary<string, bool> undefinedFields,
            string prefix,
            TTarget target,
            Expression<Func<TTarget, Settable<T>>> propertyExpr)
        {
            var jsonData = GetJsonMetadata(propertyExpr);

            var settable = (Settable<T>)jsonData.Getter(target)!;
            undefinedFields[$"{prefix}.{jsonData.JsonName}"] = !settable.IsSet;
        }

        public static void RequireSetObject<TTarget, TObject>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<TObject>>> objectExpr,
            Action<Dictionary<string, bool>, TObject, string> childChecks)
        {
            var jsonData = GetJsonMetadata(objectExpr);

            var settableObj = (Settable<TObject>)jsonData.Getter(target)!;
            undefinedFields[jsonData.JsonName] = !settableObj.IsSet;

            if (!settableObj.IsSet || !settableObj.HasValue) return;

            childChecks(undefinedFields, settableObj.Value, jsonData.JsonName);
        }

        public static void RequireSetList<TTarget, TItem>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<List<TItem>>>> listExpr,
            Action<Dictionary<string, bool>, TItem, string> perItemChecks)
        {
            var jsonData = GetJsonMetadata(listExpr);

            var settableList = (Settable<List<TItem>>)jsonData.Getter(target)!;
            undefinedFields[jsonData.JsonName] = !settableList.IsSet;

            if (!settableList.IsSet || !settableList.HasValue) return;

            var list = settableList.Value;
            for (var i = 0; i < list.Count; i++)
            {
                perItemChecks(undefinedFields, list[i], $"{jsonData.JsonName}[{i}]");
            }
        }

        public static void RequireEitherSet<TTarget, TLeft, TRight>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<TLeft>>> leftExpr,
            Expression<Func<TTarget, Settable<TRight>>> rightExpr,
            string separator = " or ")
        {
            var leftMeta = GetJsonMetadata(leftExpr);
            var rightMeta = GetJsonMetadata(rightExpr);

            var left = (Settable<TLeft>)leftMeta.Getter(target)!;
            var right = (Settable<TRight>)rightMeta.Getter(target)!;

            undefinedFields[$"{leftMeta.JsonName}{separator}{rightMeta.JsonName}"] = !left.IsSet && !right.IsSet;
        }

        public static void ValidateListIfSet<TTarget, TItem>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<List<TItem>>>> listExpr,
            Action<Dictionary<string, bool>, TItem, string> perItemChecks)
        {
            var jsonData = GetJsonMetadata(listExpr);

            var settableList = (Settable<List<TItem>>)jsonData.Getter(target)!;
            if (!settableList.IsSet || !settableList.HasValue) return;

            var list = settableList.Value;
            for (var i = 0; i < list.Count; i++)
            {
                perItemChecks(undefinedFields, list[i], $"{jsonData.JsonName}[{i}]");
            }
        }

        private static string GetJsonName(Type targetType, string propName)
        {
            return _jsonNameCache.GetOrAdd((targetType, propName), key =>
            {
                var property = key.TargetType.GetProperty(key.PropName, BindingFlags.Instance | BindingFlags.Public)
                    ?? throw new ArgumentException($"Property '{key.PropName}' not found on type '{key.TargetType.Name}'.");

                var attr = property.GetCustomAttribute<JsonPropertyAttribute>(inherit: false);
                return attr?.PropertyName ?? key.PropName;
            });
        }

        private static JsonMetadata GetJsonMetadata<TTarget, TValue>(Expression<Func<TTarget, TValue>> expr)
        {
            var propName = GetPropertyName(expr);
            var targetType = typeof(TTarget);
            var jsonName = GetJsonName(targetType, propName);

            var key = $"{targetType.FullName}:{expr}";

            return _metadataCache.GetOrAdd(key, _ =>
            {
                var compiled = expr.Compile();
                Func<object, object?> getter = o => compiled((TTarget)o);
                return new JsonMetadata(jsonName, getter);
            });
        }

        private static string GetPropertyName<TTarget, TValue>(Expression<Func<TTarget, TValue>> expr)
        {
            if (expr.Body is MemberExpression m) return m.Member.Name;
            if (expr.Body is UnaryExpression u && u.Operand is MemberExpression um) return um.Member.Name;
            throw new ArgumentException("Expression must be a property access, e.g. x => x.Foo");
        }

        private readonly struct JsonMetadata
        {
            public JsonMetadata(string jsonName, Func<object, object> getter)
            {
                JsonName = jsonName;
                Getter = getter;
            }

            public string JsonName { get; }
            public Func<object, object> Getter { get; }
        }
    }
}
