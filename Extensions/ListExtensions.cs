using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;


namespace AtaLib.ListExtensions
{
    public static class collectionExtensions
    {
        /// <summary>
        /// An Async version of ForEach. Iterates through an IEnumerable, executing a function on each item.
        /// </summary>
        /// <typeparam name="T">The Type stored in the IEnumerable</typeparam>
        /// <param name="Enumerable">The IEnumerable being iterated over</param>
        /// <param name="func">The function being executed on each item</param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> Enumerable, Func<T, Task> func)
        {
            foreach (var item in Enumerable)
                await func(item);
        }

        /// <summary>
        /// Iterates through an IEnumerable, executing a function on each item.
        /// </summary>
        /// <typeparam name="T">The Type stored in the IEnumerable</typeparam>
        /// <param name="Enumerable">The IEnumerable being iterated over</param>
        /// <param name="func">The function being executed on each item</param>
        public static void ForEach<T>(this IEnumerable<T> Enumerable, Action<T> Action)
        {
            foreach (var item in Enumerable)
                Action(item);
        }

        /// <summary>
        /// Removes an item from the list, and returns the removed item
        /// </summary>
        /// <param name="index">The index of the item you want removed. Default: last item</param>
        /// <returns>The removed item</returns>
        public static T Pop<T>(this List<T> source, int index = -1)
        {
            T item = source[index];
            source.RemoveAt(index > -1 ? index : source.Count - 1);

            return item;
        }
    }
}