using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IListExtensions; // Assuming the namespace for FindIndex is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class FindIndexTests
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
            public TestObject(int id, string name) { Id = id; Name = name; }
        }

        // === Test Cases ===

        // --- Integer Tests ---
        [TestMethod]
        public void FindIndex_Int_List_PredicateMatches_ReturnsCorrectIndex()
        {
            var source = new List<int> { 10, 20, 30, 40, 50 };
            Predicate<int> predicate = x => x == 30;
            Assert.AreEqual(2, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_Int_CustomList_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            var source = new CustomList<int> { 10, 25, 30, 25, 40 };
            Predicate<int> predicate = x => x == 25;
            Assert.AreEqual(1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_Int_List_PredicateDoesNotMatch_ReturnsMinusOne()
        {
            var source = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = x => x == 10;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_Int_List_MatchesFirstElement_ReturnsZero()
        {
            var source = new List<int> { 5, 10, 15 };
            Predicate<int> predicate = x => x == 5;
            Assert.AreEqual(0, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_Int_CustomList_MatchesLastElement_ReturnsCorrectIndex()
        {
            var source = new CustomList<int> { 5, 10, 15 };
            Predicate<int> predicate = x => x == 15;
            Assert.AreEqual(2, source.FindIndex(predicate));
        }


        // --- String Tests ---
        [TestMethod]
        public void FindIndex_String_List_PredicateMatches_ReturnsCorrectIndex()
        {
            var source = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s.StartsWith("b");
            Assert.AreEqual(1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_String_CustomList_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            var source = new CustomList<string> { "one", "two", "three", "two again" };
            Predicate<string> predicate = s => s == "two";
            // Note: This will fail if the extension method is not implemented correctly for CustomList
            // and relies on List<T>.FindIndex directly without iterating.
            // Assuming the extension iterates for IList<T>.
            Assert.AreEqual(1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_String_List_PredicateDoesNotMatch_ReturnsMinusOne()
        {
            var source = new List<string> { "apple", "banana" };
            Predicate<string> predicate = s => s == "grape";
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        // --- Bool Tests ---
        [TestMethod]
        public void FindIndex_Bool_List_PredicateMatches_ReturnsCorrectIndex()
        {
            var source = new List<bool> { false, false, true, false };
            Predicate<bool> predicate = b => b == true;
            Assert.AreEqual(2, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_Bool_CustomList_PredicateDoesNotMatch_ReturnsMinusOne()
        {
            var source = new CustomList<bool> { false, false, false };
            Predicate<bool> predicate = b => b == true;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        // --- Custom Object Tests ---
        [TestMethod]
        public void FindIndex_CustomObject_List_PredicateMatches_ReturnsCorrectIndex()
        {
            var obj1 = new TestObject(1, "Obj1");
            var obj2 = new TestObject(2, "Obj2");
            var source = new List<TestObject> { obj1, obj2, new TestObject(3, "Obj3") };
            Predicate<TestObject> predicate = obj => obj.Id == 2;
            Assert.AreEqual(1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_CustomObject_CustomList_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            var source = new CustomList<TestObject>
            {
                new TestObject(1, "Same"),
                new TestObject(2, "Different"),
                new TestObject(3, "Same")
            };
            Predicate<TestObject> predicate = obj => obj.Name == "Same";
            Assert.AreEqual(0, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_CustomObject_List_PredicateDoesNotMatch_ReturnsMinusOne()
        {
            var source = new List<TestObject> { new TestObject(1, "A"), new TestObject(2, "B") };
            Predicate<TestObject> predicate = obj => obj.Id == 3;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        // --- Empty List Tests ---
        [TestMethod]
        public void FindIndex_EmptyList_Int_ReturnsMinusOne()
        {
            var source = new List<int>();
            Predicate<int> predicate = x => x == 5;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_EmptyCustomList_String_ReturnsMinusOne()
        {
            var source = new CustomList<string>();
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        // --- Null Handling Tests (Expecting -1 as per spec) ---
        [TestMethod]
        public void FindIndex_NullSource_ReturnsMinusOne()
        {
            IList<int> source = null;
            Predicate<int> predicate = x => x > 0;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_NullPredicate_List_ReturnsMinusOne()
        {
            var source = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }

        [TestMethod]
        public void FindIndex_NullPredicate_CustomList_ReturnsMinusOne()
        {
            var source = new CustomList<string> { "a", "b" };
            Predicate<string> predicate = null;
            Assert.AreEqual(-1, source.FindIndex(predicate));
        }
    }
}
