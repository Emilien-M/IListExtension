using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq; // For SequenceEqual
using IListExtensions; // Assuming the namespace for ConvertAll is IListExtensions

namespace IListExtension.Tests
{
    [TestClass]
    public class ConvertAllTests
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

        private class SimplePoco
        {
            public int Id { get; set; }
            public string Value { get; set; }

            public override bool Equals(object obj)
            {
                return obj is SimplePoco poco && Id == poco.Id && Value == poco.Value;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, Value);
            }
        }

        // === Test Cases ===

        [TestMethod]
        public void ConvertAll_IntToString_List_CorrectConversion()
        {
            var source = new List<int> { 1, 2, 3, 4, 5 };
            var converter = new Converter<int, string>(x => x.ToString());
            var expected = new List<string> { "1", "2", "3", "4", "5" };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList()); // Convert to List for CollectionAssert
        }

        [TestMethod]
        public void ConvertAll_IntToString_CustomList_CorrectConversion()
        {
            var source = new CustomList<int> { 1, 2, 3 };
            var converter = new Converter<int, string>(x => "Num" + x);
            var expected = new List<string> { "Num1", "Num2", "Num3" };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void ConvertAll_StringToInt_List_CorrectConversion()
        {
            var source = new List<string> { "10", "20", "30" };
            var converter = new Converter<string, int>(x => int.Parse(x));
            var expected = new List<int> { 10, 20, 30 };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void ConvertAll_StringToInt_CustomList_CorrectConversion()
        {
            var source = new CustomList<string> { "100", "200" };
            var converter = new Converter<string, int>(x => int.Parse(x));
            var expected = new List<int> { 100, 200 };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void ConvertAll_StringToInt_InvalidString_ThrowsException()
        {
            var source = new List<string> { "1", "two", "3" };
            var converter = new Converter<string, int>(x => int.Parse(x));

            // The exception from int.Parse should propagate
            Assert.ThrowsException<FormatException>(() => source.ConvertAll(converter));
        }

        [TestMethod]
        public void ConvertAll_IntToDouble_List_CorrectConversion()
        {
            var source = new List<int> { 1, 2, 3 };
            var converter = new Converter<int, double>(x => (double)x * 1.5);
            var expected = new List<double> { 1.5, 3.0, 4.5 };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void ConvertAll_IntToPoco_CustomList_CorrectConversion()
        {
            var source = new CustomList<int> { 1, 2 };
            var converter = new Converter<int, SimplePoco>(x => new SimplePoco { Id = x, Value = "Item " + x });
            var expected = new List<SimplePoco>
            {
                new SimplePoco { Id = 1, Value = "Item 1" },
                new SimplePoco { Id = 2, Value = "Item 2" }
            };

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            CollectionAssert.AreEqual(expected, result.ToList());
        }

        [TestMethod]
        public void ConvertAll_EmptySource_List_ReturnsEmptyList()
        {
            var source = new List<int>();
            var converter = new Converter<int, string>(x => x.ToString());

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ConvertAll_EmptySource_CustomList_ReturnsEmptyList()
        {
            var source = new CustomList<string>();
            var converter = new Converter<string, int>(x => int.Parse(x));

            var result = source.ConvertAll(converter);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ConvertAll_NullSource_ThrowsArgumentNullException()
        {
            IList<int> source = null;
            var converter = new Converter<int, string>(x => x.ToString());

            Assert.ThrowsException<ArgumentNullException>(() => source.ConvertAll(converter));
        }

        [TestMethod]
        public void ConvertAll_NullConverter_ThrowsArgumentNullException()
        {
            var source = new List<int> { 1, 2, 3 };
            Converter<int, string> converter = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.ConvertAll(converter));
        }

        [TestMethod]
        public void ConvertAll_NullConverter_CustomList_ThrowsArgumentNullException()
        {
            var source = new CustomList<int> { 1, 2, 3 };
            Converter<int, string> converter = null;

            Assert.ThrowsException<ArgumentNullException>(() => source.ConvertAll(converter));
        }
    }
}
