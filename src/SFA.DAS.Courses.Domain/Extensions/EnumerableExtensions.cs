using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<TDest> SelectManyOrEmptyList<TSource, TDest>(
            this IEnumerable<TSource> source, Func<TSource, List<TDest>> mapper)
            => source.EmptyEnumerableIfNull().SelectMany(SelectOrEmptyList(mapper)).ToList();

        private static IEnumerable<TSource>
        EmptyEnumerableIfNull<TSource>(this IEnumerable<TSource> source)
            => source ?? Enumerable.Empty<TSource>();

        private static Func<TSource, List<TDest>>
        SelectOrEmptyList<TSource, TDest>(Func<TSource, List<TDest>> mapper)
            => x => mapper(x) ?? new List<TDest>();
    }
}
