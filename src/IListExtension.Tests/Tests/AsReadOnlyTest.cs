using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using IListExtensions; // Assuming the namespace for AsReadOnly is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class AsReadOnlyTests
    {
        // Helper class for testing with a custom IList<T> implementation
        private class CustomList<T> : IList<T>
        {
            private readonly List<T> _internalList = new List<T>();

            public T this[int index]
            {
                get => _internalList[index];
                set => _internalList[index] = value;
            }

            public int Count => _internalList.Count;
            public bool IsReadOnly => false; // Underlying list is modifiable

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

        // Tests for List<T>
        [TestMethod]
        public void AsReadOnly_ListInt_ShouldBeReadOnly()
        {
            var originalList = new List<int> { 1, 2, 3 };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add(4));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove(1));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, 0));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = 10);

            // Verify original list is not modified
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, originalList, "Original list should not be modified.");
        }

        [TestMethod]
        public void AsReadOnly_ListString_ShouldBeReadOnly()
        {
            var originalList = new List<string> { "a", "b", "c" };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add("d"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove("a"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, "z"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = "x");

            // Verify original list is not modified
            CollectionAssert.AreEqual(new List<string> { "a", "b", "c" }, originalList, "Original list should not be modified.");
        }

        [TestMethod]
        public void AsReadOnly_ListBool_ShouldBeReadOnly()
        {
            var originalList = new List<bool> { true, false };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add(true));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove(true));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, false));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = false);

            // Verify original list is not modified
            CollectionAssert.AreEqual(new List<bool> { true, false }, originalList, "Original list should not be modified.");
        }

        // Tests for CustomList<T>
        [TestMethod]
        public void AsReadOnly_CustomListInt_ShouldBeReadOnly()
        {
            var originalList = new CustomList<int> { 1, 2, 3 };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add(4));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove(1));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, 0));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = 10);

            // Verify original list is not modified
            // For CustomList, we need to compare element by element or implement a proper comparison
            Assert.AreEqual(3, originalList.Count, "Original list count should not change.");
            Assert.AreEqual(1, originalList[0]);
            Assert.AreEqual(2, originalList[1]);
            Assert.AreEqual(3, originalList[2]);
        }

        [TestMethod]
        public void AsReadOnly_CustomListString_ShouldBeReadOnly()
        {
            var originalList = new CustomList<string> { "x", "y", "z" };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add("a"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove("x"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, "w"));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = "v");

            Assert.AreEqual(3, originalList.Count, "Original list count should not change.");
            Assert.AreEqual("x", originalList[0]);
            Assert.AreEqual("y", originalList[1]);
            Assert.AreEqual("z", originalList[2]);
        }

        [TestMethod]
        public void AsReadOnly_CustomListBool_ShouldBeReadOnly()
        {
            var originalList = new CustomList<bool> { false, true, false };
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add(true));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Remove(false));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Insert(0, true));
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Clear());
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList[0] = true);

            Assert.AreEqual(3, originalList.Count, "Original list count should not change.");
            Assert.AreEqual(false, originalList[0]);
            Assert.AreEqual(true, originalList[1]);
            Assert.AreEqual(false, originalList[2]);
        }

        [TestMethod]
        public void AsReadOnly_EmptyList_ShouldBeReadOnlyAndEmpty()
        {
            var originalList = new List<int>();
            var readOnlyList = originalList.AsReadOnly();

            Assert.IsTrue(readOnlyList.IsReadOnly, "Collection should be read-only.");
            Assert.AreEqual(0, readOnlyList.Count, "Read-only list should be empty.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyList.Add(1));

            var originalCustomList = new CustomList<string>();
            var readOnlyCustomList = originalCustomList.AsReadOnly();

            Assert.IsTrue(readOnlyCustomList.IsReadOnly, "Custom collection should be read-only.");
            Assert.AreEqual(0, readOnlyCustomList.Count, "Read-only custom list should be empty.");
            Assert.ThrowsException<NotSupportedException>(() => readOnlyCustomList.Add("test"));
        }

        [TestMethod]
        public void AsReadOnly_ModifyingOriginalList_ShouldReflectInReadOnlyWrapper()
        {
            var originalList = new List<int> { 1, 2, 3 };
            var readOnlyList = originalList.AsReadOnly();

            CollectionAssert.AreEqual(originalList, readOnlyList, "Read-only list should reflect original list.");

            originalList.Add(4);
            CollectionAssert.AreEqual(originalList, readOnlyList, "Read-only list should reflect additions to original list.");
            Assert.AreEqual(4, readOnlyList.Count);
            Assert.AreEqual(4, readOnlyList[3]);


            originalList.RemoveAt(0);
            CollectionAssert.AreEqual(originalList, readOnlyList, "Read-only list should reflect removals from original list.");
            Assert.AreEqual(3, readOnlyList.Count);
            Assert.AreEqual(2, readOnlyList[0]);


            originalList[0] = 99;
            CollectionAssert.AreEqual(originalList, readOnlyList, "Read-only list should reflect modifications to original list.");
            Assert.AreEqual(99, readOnlyList[0]);

            originalList.Clear();
            CollectionAssert.AreEqual(originalList, readOnlyList, "Read-only list should reflect clearing of original list.");
            Assert.AreEqual(0, readOnlyList.Count);
        }
    }
}
