using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
     
        public static IReadOnlyCollection<T> AsReadOnly<T>(this IList<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is List<T> concreteList)
            {
                return concreteList.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(source);
        }
        
        public static int BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (index < 0)
            {
                throw new ArgumentNullException(nameof(index));
            }
            
            if (count < 0)
            {
                throw new ArgumentNullException(nameof(count));
            }

            if (source.Count - index < count)
            {
                throw new ArgumentNullException("invalid length");
            }
                
            if (source is List<T> concreteList)
            {
                return concreteList.BinarySearch(index, count, item, comparer);
            }

            return Array.BinarySearch(source.ToArray(), index, count, item, comparer);
        }
        
        public static int BinarySearch<T>(this IList<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
                
            if (source is List<T> concreteList)
            {
                return concreteList.BinarySearch(item);
            }

            return source.BinarySearch(0, source.Count, item, null);
        }
        
        public static int BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
                
            if (source is List<T> concreteList)
            {
                return concreteList.BinarySearch(item);
            }

            return source.BinarySearch(0, source.Count, item, comparer);
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
        
        public static IList<T> GetRange<T>(this IList<T> source, int index, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (index < 0)
            {
                throw new ArgumentNullException(nameof(index));
            }
            
            if (count < 0)
            {
                throw new ArgumentNullException(nameof(count));
            }
            
            if (source.Count - index < count)
            {
                throw new ArgumentNullException("invalid length");
            }

            if (source is List<T> concreteList)
            {
                return concreteList.GetRange(index, count);
            }
            
            IList<T> results = new List<T>(count);
            
            Array.Copy(source.ToArray(), index, results.ToArray(), 0, count);

            return results;
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
        
        public static bool TrueForAll<T>(this IList<T> source, Predicate<T> predicate)
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
                return concreteList.TrueForAll(predicate);
            }
           
            foreach (T element in source)
            {
                if (!predicate(element))
                {
                    return false;
                }
            }

            return true;
        }

        //ConvertAll
        //InsertRange
        //RemoveRange
        //Sort
        //TrimExcess
    }
}