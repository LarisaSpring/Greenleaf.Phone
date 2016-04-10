using System;
using System.Collections.Generic;

namespace Greenleaf
{
    public static class CollectionExtensins
    {
        public static IEnumerable<TItem> ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }
    }
}