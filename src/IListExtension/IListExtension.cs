using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IListExtension
{
    public static class IListExtension
    {
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
        
        public static bool Exists<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (source is List<T> concreteList)
            {
                return concreteList.Exists(predicate);
            }
           
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static T Find<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            if (source is List<T> concreteList)
            {
                return concreteList.Find(predicate);
            }
            
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return default;
        }

        public static IList<T> FindAll<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            if (source is List<T> concreteList)
            {
                return concreteList.FindAll(predicate);
            }
            
            IList<T> results = new List<T>();

            foreach (T element in source)
            {
                if (predicate(element))
                {
                    results.Add(element);
                }
            }

            return results;
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
        
        public static T FindLast<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            if (source is List<T> concreteList)
            {
                return concreteList.FindLast(predicate);
            }

            T result = default;
            
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    result = element;
                }
            }

            return result;
        }
        
        public static int FindLastIndex<T>(this IList<T> source, Predicate<T> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            
            if (source is List<T> concreteList)
            {
                return concreteList.FindLastIndex(predicate);
            }

            int result = -1;
            
            foreach (T element in source)
            {
                if (predicate(element))
                {
                    result = source.IndexOf(element);
                }
            }

            return result;
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

        //AsReadOnly
        //BinarySearch
        //ConvertAll
        //GetRange
        //InsertRange
        //RemoveRange
        //Sort
        //TrimExcess
        //TrueForAll
    }
}