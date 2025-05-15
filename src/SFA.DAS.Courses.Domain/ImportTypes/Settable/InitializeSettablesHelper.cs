using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    [ExcludeFromCodeCoverage]
    public static class InitializeSettablesHelper
    {
        public static void InitializeIfTagged(object obj)
        {
            if (obj == null) return;

            var type = obj.GetType();
            if (type.GetCustomAttribute<InitializeSettablesAttribute>() != null)
            {
                InitializeSettableProperties(obj);
            }
        }

        public static void InitializeIfTagged(IEnumerable<object> objects)
        {
            foreach (var obj in objects)
            {
                InitializeIfTagged(obj);
            }
        }

        public static void InitializeSettableProperties(object target)
        {
            if (target == null) return;

            var type = target.GetType();
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(target);

                if (IsSettableType(prop.PropertyType))
                {
                    if (value == null)
                    {
                        var settableInstance = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(target, settableInstance);
                    }
                }
                else if (IsComplexType(prop.PropertyType))
                {
                    if (value == null)
                    {
                        try
                        {
                            value = Activator.CreateInstance(prop.PropertyType);
                            prop.SetValue(target, value);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    InitializeSettableProperties(value);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                         prop.PropertyType != typeof(string) &&
                         value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        InitializeSettableProperties(item);
                    }
                }
            }
        }

        private static bool IsSettableType(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Settable<>);

        private static bool IsComplexType(Type type) =>
            type.IsClass && type != typeof(string);
    }
}
