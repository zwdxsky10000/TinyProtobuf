using System;
using System.Collections.Generic;

namespace PGCompiler.Extensions
{
    internal static class CollectionExtensions
    {
        internal static void AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var list1 = list as List<T>;
            if (list1 != null)
            {
                list1.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }
    }
}