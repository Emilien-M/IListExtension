using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IListExtensions; // Assuming the namespace for BinarySearch is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class BinarySearchTests
    {
        // Helper class for testing with a custom IList<T> implementation
        private class CustomList<T> : IList<T>
        {
            private readonly List<T> _internalList = new List<T>();

            public CustomList() { }
            public CustomList(IEnumerable<T> collection)
            {
                _internalList = new List<T>(collection);
            }

            public T this[int index]
            {
                get => _internalList[index];
                set => _internalList[index] = value;
            }

            public int Count => _internalList.Count;
            public bool IsReadOnly => false;

            public void Add(T item) => _internalList.Add(item);
            public void Clear() => _internalList.Clear();
            public bool Contains(T item) => _internalList.Contains(item);
            public void CopyTo(T[] array, int arrayIndex) => _internalList.CopyTo(array, arrayIndex);
            public IEnumerator<T> GetEnumerator() => _internalList.GetEnumerator();
            public int IndexOf(T item) => _internalList.IndexOf(item);
            public void Insert(int index, T item) => _internalList.Insert(index, item);
            public bool Remove(T item) => _internalList.Remove(item);
            public void RemoveAt(int index) => _internalList.RemoveAt(index);
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ReverseStringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(y, x, StringComparison.Ordinal); // Reverse comparison
            }
        }

        // Test data
        private readonly List<int> _sortedIntList = new List<int> { 10, 20, 30, 40, 50 };
        private readonly CustomList<int> _sortedCustomIntList = new CustomList<int> { 10, 20, 30, 40, 50 };
        private readonly List<string> _sortedStringList = new List<string> { "apple", "banana", "cherry", "date" };
        private readonly CustomList<string> _sortedCustomStringList = new CustomList<string> { "apple", "banana", "cherry", "date" };
        private readonly List<string> _reverseSortedStringList = new List<string> { "date", "cherry", "banana", "apple" };


        // === Tests for BinarySearch<T>(this IList<T> source, T item) ===

        [TestMethod]
        public void BinarySearch_Item_Int_Found()
        {
            Assert.AreEqual(2, _sortedIntList.BinarySearch(30));
            Assert.AreEqual(2, _sortedCustomIntList.BinarySearch(30));
        }

        [TestMethod]
        public void BinarySearch_Item_Int_NotFound_Smaller()
        {
            Assert.AreEqual(~0, _sortedIntList.BinarySearch(5));
            Assert.AreEqual(~0, _sortedCustomIntList.BinarySearch(5));
        }

        [TestMethod]
        public void BinarySearch_Item_Int_NotFound_Larger()
        {
            Assert.AreEqual(~_sortedIntList.Count, _sortedIntList.BinarySearch(55));
            Assert.AreEqual(~_sortedCustomIntList.Count, _sortedCustomIntList.BinarySearch(55));
        }

        [TestMethod]
        public void BinarySearch_Item_Int_NotFound_InBetween()
        {
            Assert.AreEqual(~2, _sortedIntList.BinarySearch(25));
            Assert.AreEqual(~2, _sortedCustomIntList.BinarySearch(25));
        }

        [TestMethod]
        public void BinarySearch_Item_String_Found()
        {
            Assert.AreEqual(1, _sortedStringList.BinarySearch("banana"));
            Assert.AreEqual(1, _sortedCustomStringList.BinarySearch("banana"));
        }

        [TestMethod]
        public void BinarySearch_Item_String_NotFound()
        {
            Assert.AreEqual(~2, _sortedStringList.BinarySearch("blueberry")); // Should be before "cherry"
            Assert.AreEqual(~_sortedStringList.Count, _sortedStringList.BinarySearch("fig"));
        }

        [TestMethod]
        public void BinarySearch_Item_EmptyList()
        {
            Assert.AreEqual(~0, new List<int>().BinarySearch(100));
            Assert.AreEqual(~0, new CustomList<string>().BinarySearch("test"));
        }

        [TestMethod]
        public void BinarySearch_Item_SingleElement_Found()
        {
            Assert.AreEqual(0, new List<int> { 42 }.BinarySearch(42));
            Assert.AreEqual(0, new CustomList<string> { "hello" }.BinarySearch("hello"));
        }

        [TestMethod]
        public void BinarySearch_Item_SingleElement_NotFound()
        {
            Assert.AreEqual(~0, new List<int> { 42 }.BinarySearch(10)); // Before
            Assert.AreEqual(~1, new List<int> { 42 }.BinarySearch(50)); // After
        }

        [TestMethod]
        public void BinarySearch_Item_NullSource_ThrowsArgumentNullException()
        {
            IList<int> nullList = null;
            Assert.ThrowsException<ArgumentNullException>(() => nullList.BinarySearch(10));
        }

        // === Tests for BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer) ===

        [TestMethod]
        public void BinarySearch_ItemComparer_String_DefaultComparer_Found()
        {
            Assert.AreEqual(1, _sortedStringList.BinarySearch("banana", Comparer<string>.Default));
            Assert.AreEqual(1, _sortedCustomStringList.BinarySearch("banana", Comparer<string>.Default));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_String_DefaultComparer_NotFound()
        {
            Assert.AreEqual(~2, _sortedStringList.BinarySearch("blueberry", Comparer<string>.Default));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_String_ReverseComparer_Found()
        {
            // List is sorted "date", "cherry", "banana", "apple" for this comparer
            Assert.AreEqual(2, _reverseSortedStringList.BinarySearch("banana", new ReverseStringComparer()));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_String_ReverseComparer_NotFound()
        {
            // List is "date", "cherry", "banana", "apple"
            // "fig" would be before "date" (index 0)
            Assert.AreEqual(~0, _reverseSortedStringList.BinarySearch("fig", new ReverseStringComparer()));
            // "car" would be between "cherry" (idx 1) and "banana" (idx 2) -> ~2
            Assert.AreEqual(~2, _reverseSortedStringList.BinarySearch("car", new ReverseStringComparer()));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_EmptyList()
        {
            Assert.AreEqual(~0, new List<string>().BinarySearch("test", Comparer<string>.Default));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_NullSource_ThrowsArgumentNullException()
        {
            IList<string> nullList = null;
            Assert.ThrowsException<ArgumentNullException>(() => nullList.BinarySearch("test", Comparer<string>.Default));
        }

        [TestMethod]
        public void BinarySearch_ItemComparer_NullComparer_UsesDefault()
        {
            // Standard List<T>.BinarySearch uses Comparer<T>.Default if comparer is null.
            // Assuming the extension method behaves consistently.
            Assert.AreEqual(1, _sortedStringList.BinarySearch("banana", null));
        }

        // === Tests for BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer) ===

        [TestMethod]
        public void BinarySearch_Range_Int_FoundInRange()
        {
            // Search for 30 in {10, 20, 30, 40, 50} from index 1, count 3 (i.e., in {20, 30, 40})
            // Expected index is 2 (absolute)
            Assert.AreEqual(2, _sortedIntList.BinarySearch(1, 3, 30, Comparer<int>.Default));
            Assert.AreEqual(2, _sortedCustomIntList.BinarySearch(1, 3, 30, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_Int_FoundAtStartOfRange()
        {
            Assert.AreEqual(1, _sortedIntList.BinarySearch(1, 3, 20, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_Int_FoundAtEndOfRange()
        {
             Assert.AreEqual(3, _sortedIntList.BinarySearch(1, 3, 40, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_Int_NotFoundInRange_Smaller()
        {
            // Search for 15 in {20, 30, 40} (original indices 1,2,3)
            // 15 is smaller than 20 (original index 1). Expected: ~1
            Assert.AreEqual(~1, _sortedIntList.BinarySearch(1, 3, 15, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_Int_NotFoundInRange_Larger()
        {
            // Search for 45 in {20, 30, 40} (original indices 1,2,3)
            // 45 is larger than 40 (original index 3). Count of sublist is 3.
            // Bitwise complement of (startIndex + countInSublist) = ~(1 + 3) = ~4
            Assert.AreEqual(~(1 + 3), _sortedIntList.BinarySearch(1, 3, 45, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_Int_NotFoundInRange_InBetween()
        {
            // Search for 35 in {20, 30, 40} (original indices 1,2,3)
            // 35 is between 30 (idx 2) and 40 (idx 3). Expected: ~3
            Assert.AreEqual(~3, _sortedIntList.BinarySearch(1, 3, 35, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_ItemOutsideSpecifiedRange_ButInList()
        {
            // Search for 50 in {10, 20, 30} (index 0, count 3)
            // 50 is larger than 30. Expected: ~(0+3) = ~3
            Assert.AreEqual(~3, _sortedIntList.BinarySearch(0, 3, 50, Comparer<int>.Default));
            // Search for 10 in {20, 30, 40} (index 1, count 3)
            // 10 is smaller than 20. Expected: ~1
            Assert.AreEqual(~1, _sortedIntList.BinarySearch(1, 3, 10, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_EmptyList_ValidRangeZeroCount()
        {
            Assert.AreEqual(~0, new List<int>().BinarySearch(0, 0, 100, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_SingleElement_Found()
        {
            Assert.AreEqual(0, new List<int>{42}.BinarySearch(0, 1, 42, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_SingleElement_NotFound()
        {
            Assert.AreEqual(~0, new List<int>{42}.BinarySearch(0, 1, 10, Comparer<int>.Default)); // before
            Assert.AreEqual(~1, new List<int>{42}.BinarySearch(0, 1, 50, Comparer<int>.Default)); // after
        }

        [TestMethod]
        public void BinarySearch_Range_String_ReverseComparer_FoundInRange()
        {
            // _reverseSortedStringList = { "date", "cherry", "banana", "apple" }
            // Search for "banana" in {"cherry", "banana"} (index 1, count 2)
            // Expected index is 2 (absolute index in _reverseSortedStringList)
            Assert.AreEqual(2, _reverseSortedStringList.BinarySearch(1, 2, "banana", new ReverseStringComparer()));
        }

        [TestMethod]
        public void BinarySearch_Range_NullSource_ThrowsArgumentNullException()
        {
            IList<int> nullList = null;
            Assert.ThrowsException<ArgumentNullException>(() => nullList.BinarySearch(0, 0, 10, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_NullComparer_UsesDefault()
        {
            Assert.AreEqual(2, _sortedIntList.BinarySearch(1, 3, 30, null));
        }

        [TestMethod]
        public void BinarySearch_Range_IndexNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sortedIntList.BinarySearch(-1, 3, 30, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_CountNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sortedIntList.BinarySearch(0, -1, 30, Comparer<int>.Default));
        }

        [TestMethod]
        public void BinarySearch_Range_IndexPlusCountTooLarge_ThrowsArgumentOutOfRangeException()
        {
            // Standard List<T>.BinarySearch throws ArgumentException, but prompt specified ArgumentOutOfRangeException for source.Count - index < count
            // For _sortedIntList (count 5), index 3, count 3 means index + count = 6, which is > 5.
            // source.Count (5) - index (3) = 2, which is < count (3). This condition should throw.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sortedIntList.BinarySearch(3, 3, 40, Comparer<int>.Default));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sortedIntList.BinarySearch(0, _sortedIntList.Count + 1, 40, Comparer<int>.Default));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new List<int>().BinarySearch(0, 1, 10, Comparer<int>.Default)); // Empty list, count 1
        }

        [TestMethod]
        public void BinarySearch_Range_ValidZeroCount()
        {
            // Searching in an empty range should always result in "not found", returning ~index.
            Assert.AreEqual(~0, _sortedIntList.BinarySearch(0, 0, 10, Comparer<int>.Default));
            Assert.AreEqual(~1, _sortedIntList.BinarySearch(1, 0, 20, Comparer<int>.Default));
            Assert.AreEqual(~_sortedIntList.Count, _sortedIntList.BinarySearch(_sortedIntList.Count, 0, 60, Comparer<int>.Default));
        }
    }
}
