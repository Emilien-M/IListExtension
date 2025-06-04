using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using IListExtensions; // Assuming the namespace for Exists is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class ExistsTests
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
        }

        // === Test Cases ===

        // --- Integer Tests ---
        [TestMethod]
        public void Exists_Int_List_PredicateMatches_ReturnsTrue()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 3;
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_Int_CustomList_PredicateMatches_ReturnsTrue()
        {
            var source = new CustomList<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x > 4;
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_Int_List_PredicateDoesNotMatch_ReturnsFalse()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            Predicate<int> predicate = x => x == 10;
            Assert.IsFalse(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_Int_CustomList_PredicateDoesNotMatch_ReturnsFalse()
        {
            var source = new CustomList<int> { 1, 2, 3 };
            Predicate<int> predicate = x => x < 0;
            Assert.IsFalse(source.Exists(predicate));
        }

        // --- String Tests ---
        [TestMethod]
        public void Exists_String_List_PredicateMatches_ReturnsTrue()
        {
            var source = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s.StartsWith("b");
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_String_CustomList_PredicateMatches_ReturnsTrue()
        {
            var source = new CustomList<string> { "one", "two", "three" };
            Predicate<string> predicate = s => s.Length == 3;
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_String_List_PredicateDoesNotMatch_ReturnsFalse()
        {
            var source = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s == "grape";
            Assert.IsFalse(source.Exists(predicate));
        }

        // --- Bool Tests ---
        [TestMethod]
        public void Exists_Bool_List_PredicateMatches_ReturnsTrue()
        {
            var source = new List<bool> { false, true, false };
            Predicate<bool> predicate = b => b == true;
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_Bool_CustomList_PredicateDoesNotMatch_ReturnsFalse()
        {
            var source = new CustomList<bool> { false, false, false };
            Predicate<bool> predicate = b => b == true;
            Assert.IsFalse(source.Exists(predicate));
        }

        // --- Custom Object Tests ---
        [TestMethod]
        public void Exists_CustomObject_List_PredicateMatches_ReturnsTrue()
        {
            var source = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Obj1", IsActive = true },
                new TestObject { Id = 2, Name = "Obj2", IsActive = false },
                new TestObject { Id = 3, Name = "Obj3", IsActive = true }
            };
            Predicate<TestObject> predicate = obj => obj.Id == 2 && !obj.IsActive;
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_CustomObject_CustomList_PredicateMatches_ReturnsTrue()
        {
            var source = new CustomList<TestObject>
            {
                new TestObject { Id = 1, Name = "A" },
                new TestObject { Id = 2, Name = "B" }
            };
            Predicate<TestObject> predicate = obj => obj.Name == "B";
            Assert.IsTrue(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_CustomObject_List_PredicateDoesNotMatch_ReturnsFalse()
        {
            var source = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Obj1", IsActive = true },
                new TestObject { Id = 2, Name = "Obj2", IsActive = false }
            };
            Predicate<TestObject> predicate = obj => obj.Name == "Obj3";
            Assert.IsFalse(source.Exists(predicate));
        }

        // --- Empty List Tests ---
        [TestMethod]
        public void Exists_EmptyList_Int_ReturnsFalse()
        {
            var source = new List<int>();
            Predicate<int> predicate = x => x == 5;
            Assert.IsFalse(source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_EmptyCustomList_String_ReturnsFalse()
        {
            var source = new CustomList<string>();
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);
            Assert.IsFalse(source.Exists(predicate));
        }

        // --- ArgumentNullException Tests ---
        [TestMethod]
        public void Exists_NullSource_ThrowsArgumentNullException()
        {
            IList<int> source = null;
            Predicate<int> predicate = x => x > 0;
            Assert.ThrowsException<ArgumentNullException>(() => source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_NullPredicate_List_ThrowsArgumentNullException()
        {
            var source = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.Exists(predicate));
        }

        [TestMethod]
        public void Exists_NullPredicate_CustomList_ThrowsArgumentNullException()
        {
            var source = new CustomList<string> { "a", "b" };
            Predicate<string> predicate = null;
            Assert.ThrowsException<ArgumentNullException>(() => source.Exists(predicate));
        }
    }
}
