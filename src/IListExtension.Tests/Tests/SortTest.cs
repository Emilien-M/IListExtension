using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class SortTest
    {
        private GenerateFactory GenerateFactory { get; }

        public SortTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject : IComparable<TestObject>
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int CompareTo(TestObject other)
            {
                if (other == null) return 1;
                return Id.CompareTo(other.Id);
            }

            public override string ToString() => $"ID: {Id}, Name: {Name}";
        }

        private class TestObjectNameComparer : IComparer<TestObject>
        {
            public int Compare(TestObject x, TestObject y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }
        }
        
        private class ReverseComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(y, x);
            }
        }

        #region Sort<T>(this IList<T> source)

        [Fact]
        public void Sort_Default_List_Int_SortsCorrectly()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(10).ToList();
            List<int> expectedList = new List<int>(sourceList);
            expectedList.Sort();

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_CustomList_Int_SortsCorrectly()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(10));
            List<int> expectedList = sourceList.ToList(); // Use ToList() for comparison copy
            expectedList.Sort();

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Default_List_String_SortsCorrectly()
        {
            // Arrange
            List<string> sourceList = GenerateFactory.GenerateRandomString(5).Take(10).ToList();
            List<string> expectedList = new List<string>(sourceList);
            expectedList.Sort();

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_List_DateTime_SortsCorrectly()
        {
            // Arrange
            List<DateTime> sourceList = GenerateFactory.GenerateRandomDate(DateTime.MinValue, DateTime.MaxValue).Take(10).ToList();
            List<DateTime> expectedList = new List<DateTime>(sourceList);
            expectedList.Sort();
            
            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_List_CustomObject_SortsCorrectly()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 3, Name = "Charlie" },
                new TestObject { Id = 1, Name = "Alice" },
                new TestObject { Id = 2, Name = "Bob" }
            };
            List<TestObject> expectedList = new List<TestObject>(sourceList);
            expectedList.Sort(); // Uses TestObject.CompareTo by Id

            // Act
            sourceList.Sort();

            // Assert
            sourceList.Select(o => o.Id).ShouldBe(expectedList.Select(o => o.Id));
        }

        [Fact]
        public void Sort_Default_List_AlreadySorted_RemainsSorted()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3, 4, 5 };
            List<int> expectedList = new List<int>(sourceList);

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_List_ReverseSorted_SortsCorrectly()
        {
            // Arrange
            List<int> sourceList = new List<int> { 5, 4, 3, 2, 1 };
            List<int> expectedList = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_List_WithDuplicates_SortsCorrectly()
        {
            // Arrange
            List<int> sourceList = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
            List<int> expectedList = new List<int>(sourceList);
            expectedList.Sort();

            // Act
            sourceList.Sort();

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Default_EmptyList_DoesNotThrow()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            CustomList<string> customList = new CustomList<string>();

            // Act & Assert
            Should.NotThrow(() => sourceList.Sort());
            sourceList.ShouldBeEmpty();
            
            Should.NotThrow(() => customList.Sort());
            customList.ShouldBeEmpty();
        }

        [Fact]
        public void Sort_Default_SingleElementList_DoesNotThrow()
        {
            // Arrange
            List<int> sourceList = new List<int> { 42 };
            List<int> expectedList = new List<int> { 42 };
            CustomList<string> customList = new CustomList<string> { "test" };
            CustomList<string> expectedCustomList = new CustomList<string> { "test" };


            // Act & Assert
            Should.NotThrow(() => sourceList.Sort());
            sourceList.ShouldBe(expectedList);

            Should.NotThrow(() => customList.Sort());
            customList.ShouldBe(expectedCustomList);
        }
        
        [Fact]
        public void Sort_Default_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort())
                .ParamName.ShouldBe("source");
        }

        #endregion

        #region Sort<T>(this IList<T> source, IComparer<T> comparer)

        [Fact]
        public void Sort_Comparer_List_Int_CustomComparer_SortsCorrectly()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(10).ToList();
            List<int> expectedList = new List<int>(sourceList);
            expectedList.Sort(new ReverseComparer<int>());

            // Act
            sourceList.Sort(new ReverseComparer<int>());

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Comparer_CustomList_Int_CustomComparer_SortsCorrectly()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(10));
            List<int> expectedList = sourceList.ToList();
            expectedList.Sort(new ReverseComparer<int>());
            
            // Act
            sourceList.Sort(new ReverseComparer<int>());

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Comparer_List_String_CaseInsensitiveComparer_SortsCorrectly()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "Banana", "Cherry", "banana" };
            List<string> expectedList = new List<string>(sourceList);
            expectedList.Sort(StringComparer.OrdinalIgnoreCase);

            // Act
            sourceList.Sort(StringComparer.OrdinalIgnoreCase);

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Comparer_List_CustomObject_NameComparer_SortsCorrectly()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 3, Name = "Charlie" },
                new TestObject { Id = 1, Name = "Alice" },
                new TestObject { Id = 2, Name = "Bob" }
            };
            List<TestObject> expectedList = new List<TestObject>(sourceList);
            expectedList.Sort(new TestObjectNameComparer());

            // Act
            sourceList.Sort(new TestObjectNameComparer());

            // Assert
            sourceList.Select(o=>o.Name).ShouldBe(expectedList.Select(o=>o.Name));
        }


        [Fact]
        public void Sort_Comparer_List_NullComparer_UsesDefaultSort()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(10).ToList();
            List<int> expectedList = new List<int>(sourceList);
            expectedList.Sort(); // Default sort

            // Act
            sourceList.Sort((IComparer<int>)null);

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Comparer_CustomList_NullComparer_UsesDefaultSort()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(10));
            List<int> expectedList = sourceList.ToList();
            expectedList.Sort(Comparer<int>.Default);

            // Act
            sourceList.Sort(Comparer<int>.Default); // Explicitly pass Comparer<T>.Default

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Comparer_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort(Comparer<int>.Default))
                .ParamName.ShouldBe("source");
        }

        #endregion

        #region Sort<T>(this IList<T> source, int index, int count, IComparer<T> comparer)

        [Fact]
        public void Sort_IndexCountComparer_List_Int_SortsSubSection()
        {
            // Arrange
            List<int> sourceList = new List<int> { 0, 1, 5, 2, 4, 3, 6 }; // Sort {5,2,4} -> {2,4,5}
            List<int> expectedList = new List<int> { 0, 1, 2, 4, 5, 3, 6 };
            int index = 2;
            int count = 3;

            // Act
            sourceList.Sort(index, count, null); // Null comparer for default

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_IndexCountComparer_CustomList_Int_SortsSubSection()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 0, 1, 5, 2, 4, 3, 6 };
            List<int> expectedList = new List<int> { 0, 1, 2, 4, 5, 3, 6 };
            int index = 2;
            int count = 3;

            // Act
            sourceList.Sort(index, count, Comparer<int>.Default);

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_IndexCountComparer_List_String_ReverseSortSubSection()
        {
            // Arrange
            List<string> sourceList = new List<string> { "z", "a", "d", "b", "c", "y" }; // Sort {"a","d","b","c"} -> {"d","c","b","a"} (reverse)
            List<string> expectedList = new List<string> { "z", "d", "c", "b", "a", "y" };
            int index = 1;
            int count = 4;

            // Act
            sourceList.Sort(index, count, new ReverseComparer<string>());

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_IndexCountComparer_List_CountZero_DoesNotChangeList()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(5).ToList();
            List<int> expectedList = new List<int>(sourceList);
            int index = 1;
            int count = 0;

            // Act
            sourceList.Sort(index, count, null);

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_IndexCountComparer_CustomList_CountZero_DoesNotChangeList()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(5));
            List<int> expectedList = sourceList.ToList();
            int index = 1;
            int count = 0;

            // Act
            sourceList.Sort(index, count, Comparer<int>.Default);

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Theory]
        [InlineData(-1, 2)] // index < 0
        [InlineData(0, -1)] // count < 0
        [InlineData(3, 4)]  // index + count > list.Count (list size 5)
        public void Sort_IndexCountComparer_List_InvalidRange_ThrowsArgumentExceptionOrOutOfRange(int index, int count)
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 10).Take(5).ToList();
            
            // Act & Assert
            // Behavior of List<T>.Sort for range errors can be ArgumentException or ArgumentOutOfRangeException
            // depending on the specific invalidity.
            // index < 0 -> ArgumentOutOfRangeException (index)
            // count < 0 -> ArgumentOutOfRangeException (count)
            // index + count > Length -> ArgumentException
            if (index < 0)
                Should.Throw<ArgumentOutOfRangeException>(() => sourceList.Sort(index, count, null))
                    .ParamName.ShouldBe("index");
            else if (count < 0)
                 Should.Throw<ArgumentOutOfRangeException>(() => sourceList.Sort(index, count, null))
                    .ParamName.ShouldBe("count");
            else // index + count > Length
                 Should.Throw<ArgumentException>(() => sourceList.Sort(index, count, null));
        }
        
        [Theory]
        [InlineData(-1, 2)] 
        [InlineData(0, -1)] 
        [InlineData(3, 4)]  
        public void Sort_IndexCountComparer_CustomList_InvalidRange_ThrowsException(int index, int count)
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 10).Take(5));
            
            // Act & Assert
            // The extension for CustomList might throw ArgumentOutOfRangeException for all these due to its ToArray logic
            if (index < 0)
                Should.Throw<ArgumentOutOfRangeException>(() => sourceList.Sort(index, count, null))
                    .ParamName.ShouldBe("index");
            else if (count < 0)
                 Should.Throw<ArgumentOutOfRangeException>(() => sourceList.Sort(index, count, null))
                    .ParamName.ShouldBe("count");
            else // index + count > Length for CustomList might throw different things based on Array.Sort
                 Should.Throw<ArgumentException>(() => sourceList.Sort(index, count, null));
        }
        
        [Fact]
        public void Sort_IndexCountComparer_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort(0,0,Comparer<int>.Default))
                .ParamName.ShouldBe("source");
        }

        #endregion

        #region Sort<T>(this IList<T> source, Comparison<T> comparison)

        [Fact]
        public void Sort_Comparison_List_Int_CustomComparison_SortsCorrectly()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(10).ToList();
            List<int> expectedList = new List<int>(sourceList);
            Comparison<int> reverseComparison = (x, y) => y.CompareTo(x);
            expectedList.Sort(reverseComparison);

            // Act
            sourceList.Sort(reverseComparison);

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Comparison_CustomList_Int_CustomComparison_SortsCorrectly()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(10));
            List<int> expectedList = sourceList.ToList();
            Comparison<int> reverseComparison = (x, y) => y.CompareTo(x);
            expectedList.Sort(reverseComparison);
            
            // Act
            sourceList.Sort(reverseComparison);

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Comparison_List_String_LengthComparison_SortsCorrectly()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "kiwi", "fig", "elderberry" };
            List<string> expectedList = new List<string>(sourceList);
            Comparison<string> lengthComparison = (x, y) => x.Length.CompareTo(y.Length);
            expectedList.Sort(lengthComparison);

            // Act
            sourceList.Sort(lengthComparison);

            // Assert
            sourceList.ShouldBe(expectedList);
        }
        
        [Fact]
        public void Sort_Comparison_List_CustomObject_NameLengthComparison_SortsCorrectly()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Charles" }, // 7
                new TestObject { Id = 2, Name = "Al" },      // 2
                new TestObject { Id = 3, Name = "Bobby" }    // 5
            };
            List<TestObject> expectedList = new List<TestObject>
            {
                sourceList[1], // Al (2)
                sourceList[2], // Bobby (5)
                sourceList[0]  // Charles (7)
            };
            Comparison<TestObject> nameLengthComparison = (x, y) => x.Name.Length.CompareTo(y.Name.Length);

            // Act
            sourceList.Sort(nameLengthComparison);

            // Assert
            sourceList.ShouldBe(expectedList);
        }

        [Fact]
        public void Sort_Comparison_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Comparison<int> comparison = (x, y) => x.CompareTo(y);

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort(comparison))
                .ParamName.ShouldBe("source");
        }

        [Fact]
        public void Sort_Comparison_NullComparison_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(5).ToList();
            Comparison<int> comparison = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort(comparison))
                .ParamName.ShouldBe("comparison");
        }
        
        [Fact]
        public void Sort_Comparison_CustomList_NullComparison_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 100).Take(5));
            Comparison<int> comparison = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Sort(comparison))
                .ParamName.ShouldBe("comparison");
        }

        #endregion
    }
}
