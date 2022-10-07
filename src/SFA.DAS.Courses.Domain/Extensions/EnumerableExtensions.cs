using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TDest> SelectManyOrEmptyList<TSource, TDest>(
            this IEnumerable<TSource> source, Func<TSource, IEnumerable<TDest>> mapper)
            => source.EmptyEnumerableIfNull().SelectMany(SelectOrEmptyList(mapper));

        public static IEnumerable<TSource>
        EmptyEnumerableIfNull<TSource>(this IEnumerable<TSource> source)
            => source ?? Enumerable.Empty<TSource>();

        private static Func<TSource, IEnumerable<TDest>>
        SelectOrEmptyList<TSource, TDest>(Func<TSource, IEnumerable<TDest>> mapper)
            => x => mapper(x) ?? Enumerable.Empty<TDest>();
    }
}
