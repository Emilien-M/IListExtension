using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IListExtensions; // Assuming the namespace for Find is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class FindTests
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
        }

        // === Test Cases ===

        // --- Integer Tests ---
        [TestMethod]
        public void Find_Int_List_PredicateMatches_ReturnsElement()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 3;
            Assert.AreEqual(3, source.Find(predicate));
        }

        [TestMethod]
        public void Find_Int_CustomList_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            var source = new CustomList<int> { 1, 2, 3, 4, 3, 5 };
            Predicate<int> predicate = x => x == 3;
            Assert.AreEqual(3, source.Find(predicate)); // Should be the first 3 at index 2
        }

        [TestMethod]
        public void Find_Int_List_PredicateDoesNotMatch_ReturnsDefault()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 10;
            Assert.AreEqual(default(int), source.Find(predicate)); // 0 for int
        }

        // --- String Tests ---
        [TestMethod]
        public void Find_String_List_PredicateMatches_ReturnsElement()
        {
            var source = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s.StartsWith("b");
            Assert.AreEqual("banana", source.Find(predicate));
        }

        [TestMethod]
        public void Find_String_CustomList_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            var source = new CustomList<string> { "cat", "car", "cart" };
            Predicate<string> predicate = s => s.Contains("ca");
            Assert.AreEqual("cat", source.Find(predicate));
        }

        [TestMethod]
        public void Find_String_List_PredicateDoesNotMatch_ReturnsDefault()
        {
            var source = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s == "grape";
            Assert.AreEqual(default(string), source.Find(predicate)); // null for string
        }

        // --- Bool Tests ---
        [TestMethod]
        public void Find_Bool_List_PredicateMatches_ReturnsElement()
        {
            var source = new List<bool> { false, true, false };
            Predicate<bool> predicate = b => b == true;
            Assert.AreEqual(true, source.Find(predicate));
        }

        [TestMethod]
        public void Find_Bool_CustomList_PredicateDoesNotMatch_ReturnsDefault()
        {
            var source = new CustomList<bool> { false, false, false };
            Predicate<bool> predicate = b => b == true;
            Assert.AreEqual(default(bool), source.Find(predicate)); // false for bool
        }

        // --- Custom Object Tests ---
        [TestMethod]
        public void Find_CustomObject_List_PredicateMatches_ReturnsElement()
        {
            var obj1 = new TestObject(1, "Obj1", true);
            var obj2 = new TestObject(2, "Obj2", false);
            var source = new List<TestObject> { obj1, obj2 };
            Predicate<TestObject> predicate = obj => obj.Id == 2;
            Assert.AreSame(obj2, source.Find(predicate)); // Check for reference equality
        }

        [TestMethod]
        public void Find_CustomObject_CustomList_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            var obj1 = new TestObject(1, "SameName");
            var obj2 = new TestObject(2, "DifferentName");
            var obj3 = new TestObject(3, "SameName");
            var source = new CustomList<TestObject> { obj1, obj2, obj3 };
            Predicate<TestObject> predicate = obj => obj.Name == "SameName";
            Assert.AreSame(obj1, source.Find(predicate));
        }

        [TestMethod]
        public void Find_CustomObject_List_PredicateDoesNotMatch_ReturnsDefault()
        {
            var source = new List<TestObject>
            {
                new TestObject(1, "Obj1"), new TestObject(2, "Obj2")
            };
            Predicate<TestObject> predicate = obj => obj.Name == "Obj3";
            Assert.AreEqual(default(TestObject), source.Find(predicate)); // null for TestObject
        }

        // --- Empty List Tests ---
        [TestMethod]
        public void Find_EmptyList_Int_ReturnsDefault()
        {
            var source = new List<int>();
            Predicate<int> predicate = x => x == 5;
            Assert.AreEqual(default(int), source.Find(predicate));
        }

        [TestMethod]
        public void Find_EmptyCustomList_String_ReturnsDefault()
        {
            var source = new CustomList<string>();
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);
            Assert.AreEqual(default(string), source.Find(predicate));
        }

        [TestMethod]
        public void Find_EmptyList_CustomObject_ReturnsDefault()
        {
            var source = new List<TestObject>();
            Predicate<TestObject> predicate = obj => obj.Id == 1;
            Assert.AreEqual(default(TestObject), source.Find(predicate));
        }

        // --- ArgumentNullException Tests ---
        [TestMethod]
        public void Find_NullSource_ThrowsArgumentNullException()
        {
            IList<int> source = null;
            Predicate<int> predicate = x => x > 0;
            Assert.ThrowsException<ArgumentNullException>(() => source.Find(predicate));
        }

        [TestMethod]
        public void Find_NullPredicate_List_ThrowsArgumentNullException()
        {
            var source = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.Find(predicate));
        }

        [TestMethod]
        public void Find_NullPredicate_CustomList_ThrowsArgumentNullException()
        {
            var source = new CustomList<string> { "a", "b" };
            Predicate<string> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.Find(predicate));
        }
    }
}
