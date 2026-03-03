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

        private static void InitializeSettableProperties(object target)
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
                        value = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(target, value);
                    }
                    else if (value is ISettable settableValue)
                    {
                        if (!settableValue.HasValue) continue;

                        if (settableValue.UntypedValue is IEnumerable enumerableValue && enumerableValue is not string)
                        {
                            foreach (var item in enumerableValue)
                            {
                                InitializeIfTagged(item);
                            }
                        }
                        else
                        {
                            InitializeIfTagged(settableValue.UntypedValue);
                        }
                    }
                }
            }
        }

        private static bool IsSettableType(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Settable<>);
    }
}
