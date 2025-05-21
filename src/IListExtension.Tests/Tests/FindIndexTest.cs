using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class FindIndexTest
    {
        private GenerateFactory GenerateFactory { get; }

        public FindIndexTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        #region FindIndex<T>(this IList<T> source, Predicate<T> predicate)

        [Fact]
        public void FindIndex_List_Int_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            List<int> sourceList = new List<int> { 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 15;
            int expectedIndex = 2;

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindIndex_CustomList_Int_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 15;
            int expectedIndex = 2;

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindIndex_List_Int_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            // Arrange
            List<int> sourceList = new List<int> { 5, 10, 15, 10, 20 };
            Predicate<int> predicate = x => x == 10;
            int expectedIndex = 1; // Index of the first '10'

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindIndex_CustomList_Int_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 5, 10, 15, 10, 20 };
            Predicate<int> predicate = x => x == 10;
            int expectedIndex = 1;

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void FindIndex_List_Int_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 50).Take(10).ToList();
            Predicate<int> predicate = x => x > 100; // No element will satisfy this

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindIndex_CustomList_Int_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 50).Take(10));
            Predicate<int> predicate = x => x < 0; // Assuming positive numbers

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }
        
        [Fact]
        public void FindIndex_List_String_PredicateMatchesOne_ReturnsCorrectIndex()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s == "banana";
            int expectedIndex = 1;

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }
        
        [Fact]
        public void FindIndex_CustomList_String_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            Predicate<string> predicate = s => s == "nonexistent";

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindIndex_List_CustomObject_PredicateMatchesMultiple_ReturnsFirstIndex()
        {
            // Arrange
            TestObject obj1 = new TestObject { Id = 1, Value = "A" };
            TestObject obj2 = new TestObject { Id = 2, Value = "B" };
            TestObject obj3 = new TestObject { Id = 3, Value = "A" }; // Second match
            List<TestObject> sourceList = new List<TestObject> { obj1, obj2, obj3 };
            Predicate<TestObject> predicate = obj => obj.Value == "A";
            int expectedIndex = 0; // Index of obj1

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(expectedIndex);
        }
        
        [Fact]
        public void FindIndex_CustomList_CustomObject_PredicateNoMatch_ReturnsMinusOne()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>
            {
                new TestObject { Id = 1, Value = "X" },
                new TestObject { Id = 2, Value = "Y" }
            };
            Predicate<TestObject> predicate = obj => obj.Value == "Z";

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindIndex_EmptyList_ReturnsMinusOne()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x == 5;

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindIndex_EmptyCustomList_ReturnsMinusOne()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => s == "test";

            // Act
            int resultIndex = sourceList.FindIndex(predicate);

            // Assert
            resultIndex.ShouldBe(-1);
        }

        [Fact]
        public void FindIndex_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindIndex(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void FindIndex_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindIndex(predicate))
                  .ParamName.ShouldBe("match"); // .NET standard is "match"
        }
        
        [Fact]
        public void FindIndex_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindIndex(predicate))
                  .ParamName.ShouldBe("match");
        }

        #endregion
    }
}
