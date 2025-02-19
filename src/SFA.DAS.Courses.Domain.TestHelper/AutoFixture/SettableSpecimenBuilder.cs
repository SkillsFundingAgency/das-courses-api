using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoFixture.Kernel;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.TestHelper.AutoFixture
{
    public class SettableSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type.IsClass)
            {
                var hasSettableProperties = type.GetProperties()
                    .Any(p => p.PropertyType.IsGenericType &&
                              p.PropertyType.GetGenericTypeDefinition() == typeof(Settable<>));

                if (!hasSettableProperties)
                {
                    return new NoSpecimen();
                }

                var instance = Activator.CreateInstance(type);

                foreach (var property in type.GetProperties())
                {
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(Settable<>))
                    {
                        var innerType = property.PropertyType.GetGenericArguments()[0];
                        object resolvedValue;

                        var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

                        if (rangeAttribute != null && (innerType == typeof(int) || innerType == typeof(long)))
                        {
                            resolvedValue = Convert.ChangeType(rangeAttribute.Minimum, innerType);
                        }
                        else
                        {
                            resolvedValue = context.Resolve(innerType);
                        }

                        var settableInstance = Activator.CreateInstance(
                            typeof(Settable<>).MakeGenericType(innerType),
                            resolvedValue
                        );
                        property.SetValue(instance, settableInstance);
                    }
                    else
                    {
                        property.SetValue(instance, context.Resolve(property.PropertyType));
                    }
                }

                return instance;
            }

            return new NoSpecimen();
        }
    }

}
