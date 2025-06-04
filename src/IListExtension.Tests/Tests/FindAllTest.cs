using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq; // For ToList() and SequenceEqual()
using IListExtensions; // Assuming the namespace for FindAll is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class FindAllTests
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

        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }

            public TestObject(int id, string name, bool isActive = false)
            {
                Id = id;
                Name = name;
                IsActive = isActive;
            }
             // Equals and GetHashCode can be useful for comparing TestObject instances if needed,
             // but for FindAll, we usually check properties or reference for the returned items.
        }

        // === Test Cases ===

        // --- Integer Tests ---
        [TestMethod]
        public void FindAll_Int_List_PredicateMatchesMultiple_ReturnsAllMatching()
        {
            var source = new List<int> { 1, 2, 3, 4, 5, 6 };
            Predicate<int> predicate = x => x % 2 == 0; // Even numbers
            var expected = new List<int> { 2, 4, 6 };
            var result = source.FindAll(predicate);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void FindAll_Int_CustomList_PredicateMatchesSingle_ReturnsMatching()
        {
            var source = new CustomList<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 3;
            var expected = new List<int> { 3 };
            var result = source.FindAll(predicate);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void FindAll_Int_List_PredicateDoesNotMatch_ReturnsEmptyList()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 10;
            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        // --- String Tests ---
        [TestMethod]
        public void FindAll_String_List_PredicateMatchesMultiple_ReturnsAllMatchingInOrder()
        {
            var source = new List<string> { "apple", "banana", "apricot", "cherry", "avocado" };
            Predicate<string> predicate = s => s.StartsWith("a");
            var expected = new List<string> { "apple", "apricot", "avocado" };
            var result = source.FindAll(predicate);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void FindAll_String_CustomList_PredicateDoesNotMatch_ReturnsEmptyList()
        {
            var source = new CustomList<string> { "one", "two", "three" };
            Predicate<string> predicate = s => s.Length > 5;
            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        // --- Custom Object Tests ---
        [TestMethod]
        public void FindAll_CustomObject_List_PredicateMatchesMultiple_ReturnsAllMatching()
        {
            var obj1 = new TestObject(1, "Obj1", true);
            var obj2 = new TestObject(2, "Obj2", false);
            var obj3 = new TestObject(3, "Obj3", true);
            var obj4 = new TestObject(4, "Obj4", false);
            var source = new List<TestObject> { obj1, obj2, obj3, obj4 };
            Predicate<TestObject> predicate = obj => obj.IsActive;
            var expected = new List<TestObject> { obj1, obj3 }; // Expecting references
            var result = source.FindAll(predicate);

            Assert.AreEqual(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreSame(expected[i], result[i], $"Element at index {i} is not the same instance.");
            }
        }

        [TestMethod]
        public void FindAll_CustomObject_CustomList_PredicateMatchesSingle_ReturnsMatching()
        {
            var obj1 = new TestObject(1, "Target");
            var obj2 = new TestObject(2, "Other");
            var source = new CustomList<TestObject> { obj1, obj2 };
            Predicate<TestObject> predicate = obj => obj.Name == "Target";
            var expected = new List<TestObject> { obj1 };
            var result = source.FindAll(predicate);

            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreSame(expected[0], result[0]);
        }


        // --- Empty List Tests ---
        [TestMethod]
        public void FindAll_EmptyList_Int_ReturnsEmptyList()
        {
            var source = new List<int>();
            Predicate<int> predicate = x => x == 5;
            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FindAll_EmptyCustomList_TestObject_ReturnsEmptyList()
        {
            var source = new CustomList<TestObject>();
            Predicate<TestObject> predicate = obj => obj.IsActive;
            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        // --- Returned List is New Test ---
        [TestMethod]
        public void FindAll_ModifyingReturnedList_DoesNotAffectSourceList_List()
        {
            var originalSource = new List<int> { 1, 2, 3, 4, 5 };
            var source = new List<int>(originalSource); // Work with a copy for the test
            Predicate<int> predicate = x => x % 2 == 0; // Even numbers {2, 4}

            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // {2, 4}

            // Try to modify the result list
            result.Add(6);
            result.RemoveAt(0); // result is now {4, 6}

            // Verify original source list is unchanged
            CollectionAssert.AreEqual(originalSource, source, "Source list should not be modified.");
        }

        [TestMethod]
        public void FindAll_ModifyingReturnedList_DoesNotAffectSourceList_CustomList()
        {
            var originalSourceData = new List<TestObject> { new TestObject(1, "A"), new TestObject(2, "B", true) };
            var source = new CustomList<TestObject>(originalSourceData);
            Predicate<TestObject> predicate = obj => obj.IsActive; // { ObjB }

            var result = source.FindAll(predicate);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            result.Add(new TestObject(3, "C", true)); // result is now { ObjB, ObjC }

            Assert.AreEqual(originalSourceData.Count, source.Count, "Source list count should not change.");
            Assert.AreSame(originalSourceData[0], source[0]);
            Assert.AreSame(originalSourceData[1], source[1]);
        }


        // --- ArgumentNullException Tests ---
        [TestMethod]
        public void FindAll_NullSource_ThrowsArgumentNullException()
        {
            IList<int> source = null;
            Predicate<int> predicate = x => x > 0;
            Assert.ThrowsException<ArgumentNullException>(() => source.FindAll(predicate));
        }

        [TestMethod]
        public void FindAll_NullPredicate_List_ThrowsArgumentNullException()
        {
            var source = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.FindAll(predicate));
        }

        [TestMethod]
        public void FindAll_NullPredicate_CustomList_ThrowsArgumentNullException()
        {
            var source = new CustomList<string> { "a", "b" };
            Predicate<string> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.FindAll(predicate));
        }
    }
}
