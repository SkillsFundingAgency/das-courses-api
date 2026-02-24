using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.ImportTypes.Settable
{
    public static  class SettableMapExtensions
    {
        public static Settable<TOut> Map<TIn, TOut>(
            this Settable<TIn> source,
            Func<TIn, TOut> map)
        {
            if (source == null || !source.IsSet)
                return Settable<TOut>.Undefined;

            if (source.HasInvalidValue)
                return Settable<TOut>.FromInvalidValue(source.InvalidValue);

            if (!source.HasValue)
                return new Settable<TOut>(default);

            return new Settable<TOut>(map(source.Value));
        }

        public static Settable<List<TOut>> MapList<TIn, TOut>(
            this Settable<List<TIn>> source,
            Func<TIn, TOut> mapItem)
        {
            if (source == null || !source.IsSet)
                return Settable<List<TOut>>.Undefined;

            if (source.HasInvalidValue)
                return Settable<List<TOut>>.FromInvalidValue(source.InvalidValue);

            if (!source.HasValue)
                return new Settable<List<TOut>>(default);

            if (source.Value == null)
                return new Settable<List<TOut>>(null);

            return new Settable<List<TOut>>(source.Value.Select(mapItem).ToList());
        }
    }
}
