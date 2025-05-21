using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class FindLastIndexTest
    {
        private GenerateFactory GenerateFactory { get; }

        public FindLastIndexTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        #region FindLastIndex<T>(this IList<T> source, Predicate<T> predicate)

        [Fact]
        public void FindLastIndex_List_Int_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            List<int> sourceList = new List<int> { 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 15;
            int expectedIndex = 2;

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindLastIndex_CustomList_Int_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 15;
            int expectedIndex = 2;

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindLastIndex_List_Int_PredicateMatchesMultiple_ReturnsLastIndex()
        {
            // Arrange
            List<int> sourceList = new List<int> { 5, 10, 15, 10, 20 }; // Last '10' is at index 3
            Predicate<int> predicate = x => x == 10;
            int expectedIndex = 3; 

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindLastIndex_CustomList_Int_PredicateMatchesMultiple_ReturnsLastIndex()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 5, 10, 15, 10, 20 };
            Predicate<int> predicate = x => x == 10;
            int expectedIndex = 3;

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindLastIndex_List_Int_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 50).Take(10).ToList();
            Predicate<int> predicate = x => x > 100; // No element will satisfy this

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindLastIndex_CustomList_Int_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 50).Take(10));
            Predicate<int> predicate = x => x < 0; // Assuming positive numbers

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }
        
        [Fact]
        public void FindLastIndex_List_String_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry", "banana", "date" };
            Predicate<string> predicate = s => s == "cherry";
            int expectedIndex = 2;

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindLastIndex_List_String_PredicateMatchesMultiple_ReturnsLastIndex()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry", "banana", "date" };
            Predicate<string> predicate = s => s == "banana";
            int expectedIndex = 3; // Index of the last "banana"

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }
        
        [Fact]
        public void FindLastIndex_CustomList_String_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            sourceList.Add("uniqueLast");
            Predicate<string> predicate = s => s == "nonexistent";

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindLastIndex_List_CustomObject_PredicateMatchesMultiple_ReturnsLastIndex()
        {
            // Arrange
            TestObject obj1 = new TestObject { Id = 1, Value = "A" };
            TestObject obj2 = new TestObject { Id = 2, Value = "B" };
            TestObject obj3 = new TestObject { Id = 3, Value = "A" }; // Last match
            List<TestObject> sourceList = new List<TestObject> { obj1, obj2, obj3, obj1 }; // obj3 is at index 2
            Predicate<TestObject> predicate = obj => obj.Value == "A";
            int expectedIndex = 2; 

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }
        
        [Fact]
        public void FindLastIndex_CustomList_CustomObject_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>
            {
                new TestObject { Id = 1, Value = "X" },
                new TestObject { Id = 2, Value = "Y" }
            };
            Predicate<TestObject> predicate = obj => obj.Value == "Z";

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindLastIndex_EmptyList_ReturnsMinusOne()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x == 5;

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindLastIndex_EmptyCustomList_ReturnsMinusOne()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => s == "test";

            // Act
            int resultIndex = sourceList.FindLastIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindLastIndex_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindLastIndex(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void FindLastIndex_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            // The .NET standard parameter name for the predicate in FindLastIndex is "match".
            Should.Throw<ArgumentNullException>(() => sourceList.FindLastIndex(predicate))
                  .ParamName.ShouldBe("match"); 
        }
        
        [Fact]
        public void FindLastIndex_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindLastIndex(predicate))
                  .ParamName.ShouldBe("match");
        }

        #endregion
    }
}
