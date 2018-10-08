using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IListExtension
{
    public static class IListExtension
    {
        public static int RemoveAll<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null || predicate == null)
            {
                return -1;
            }

            if (source is List<T> concreteList)
            {
                return concreteList.RemoveAll(predicate);
            }

            int result = 0;

            for (int i = source.Count - 1; i >= 0; i--)
            {
                if (!predicate(source[i]))
                {
                    continue;
                }

                ++result;
                source.RemoveAt(i);
            }

            return result;
        }

        public static void AddRange<T>(this IList<T> source, IEnumerable<T> newList)
        {
            if (source == null || newList == null)
            {
                return;
            }

            if (source is List<T> concreteList)
            {
                concreteList.AddRange(newList);
                return;
            }

            foreach (var element in newList)
            {
                source.Add(element);
            }
        }

        public static void ForEach<T>(this IList<T> source, Action<T> action)
        {
            if (source == null || action == null)
            {
                return;
            }

            if (source is List<T> concreteList)
            {
                concreteList.ForEach(action);
                return;
            }

            foreach (var element in source)
            {
                action.Invoke(element);
            }
        }

        public static async Task ForEach<T>(this IList<T> source, Func<T, Task> func)
        {
            if (source == null || func == null)
            {
                return;
            }

            foreach (var element in source)
            {
                await func.Invoke(element);
            }
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null || predicate == null)
            {
                return -1;
            }

            if (source is List<T> concreteList)
            {
                return concreteList.RemoveAll(predicate);
            }

            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}