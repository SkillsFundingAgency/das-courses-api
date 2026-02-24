using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    internal static class RequiredFieldChecks
    {
        public static string JsonName<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName)
                ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");

            var jsonPropertyAttribute = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                .Cast<JsonPropertyAttribute>()
                .FirstOrDefault();

            return jsonPropertyAttribute?.PropertyName ?? propertyName;
        }

        public static void RequireSet<TTarget, T>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<T>>> propertyExpr)
        {
            var propName = GetPropertyName(propertyExpr);
            var jsonName = JsonName<TTarget>(propName);

            var settable = propertyExpr.Compile()(target);
            undefinedFields[jsonName] = !settable.IsSet;
        }

        public static void RequireSetAt<TTarget, T>(
            Dictionary<string, bool> undefinedFields,
            string prefix,
            TTarget target,
            Expression<Func<TTarget, Settable<T>>> propertyExpr)
        {
            var propName = GetPropertyName(propertyExpr);
            var jsonName = JsonName<TTarget>(propName);

            var settable = propertyExpr.Compile()(target);
            undefinedFields[$"{prefix}.{jsonName}"] = !settable.IsSet;
        }

        public static void RequireSetObject<TTarget, TObject>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<TObject>>> objectExpr,
            Action<Dictionary<string, bool>, TObject, string> childChecks)
        {
            var propName = GetPropertyName(objectExpr);
            var jsonName = JsonName<TTarget>(propName);

            var settableObj = objectExpr.Compile()(target);

            undefinedFields[jsonName] = !settableObj.IsSet;

            if (!settableObj.IsSet || !settableObj.HasValue) return;

            childChecks(undefinedFields, settableObj.Value, jsonName);
        }


        public static void RequireSetList<TTarget, TItem>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<List<TItem>>>> listExpr,
            Action<Dictionary<string, bool>, TItem, string> perItemChecks)
        {
            var listPropName = GetPropertyName(listExpr);
            var listJsonName = JsonName<TTarget>(listPropName);

            var settableList = listExpr.Compile()(target);
            undefinedFields[listJsonName] = !settableList.IsSet;

            if (!settableList.IsSet || !settableList.HasValue) return;

            for (var i = 0; i < settableList.Value.Count; i++)
            {
                perItemChecks(undefinedFields, settableList.Value[i], $"{listJsonName}[{i}]");
            }
        }

        public static void RequireEitherSet<TTarget, TLeft, TRight>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<TLeft>>> leftExpr,
            Expression<Func<TTarget, Settable<TRight>>> rightExpr,
            string separator = " or ")
         {
            var leftName = JsonName<TTarget>(GetPropertyName(leftExpr));
            var rightName = JsonName<TTarget>(GetPropertyName(rightExpr));
    
            var left = leftExpr.Compile()(target);
            var right = rightExpr.Compile()(target);

            undefinedFields[$"{leftName}{separator}{rightName}"] = !left.IsSet && !right.IsSet;
        }

        public static void ValidateListIfSet<TTarget, TItem>(
            Dictionary<string, bool> undefinedFields,
            TTarget target,
            Expression<Func<TTarget, Settable<List<TItem>>>> listExpr,
            Action<Dictionary<string, bool>, TItem, string> perItemChecks)
        {
            var listPropName = GetPropertyName(listExpr);
            var listJsonName = JsonName<TTarget>(listPropName);

            var settableList = listExpr.Compile()(target);

            if (!settableList.IsSet || !settableList.HasValue) return;

            for (var i = 0; i < settableList.Value.Count; i++)
            {
                perItemChecks(undefinedFields, settableList.Value[i], $"{listJsonName}[{i}]");
            }
        }

        private static string GetPropertyName<TTarget, TValue>(Expression<Func<TTarget, TValue>> expr)
        {
            if (expr.Body is MemberExpression m) return m.Member.Name;
            if (expr.Body is UnaryExpression u && u.Operand is MemberExpression um) return um.Member.Name;
            throw new ArgumentException("Expression must be a property access, e.g. x => x.Foo");
        }
    }

}
