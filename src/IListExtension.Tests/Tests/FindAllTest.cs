using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class FindAllTest
    {
        private GenerateFactory GenerateFactory { get; }

        public FindAllTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject : IEquatable<TestObject>
        {
            public int Id { get; set; }
            public string Value { get; set; }

            public bool Equals(TestObject other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id == other.Id && Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestObject)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, Value);
            }
        }

        [Fact]
        public void FindAll_List_Int_PredicateMatchesMultiple_ReturnsAllMatchingElements()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3, 4, 5, 2, 6, 2 };
            Predicate<int> predicate = x => x == 2;
            List<int> expected = new List<int> { 2, 2, 2 };

            // Act
            IList<int> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<IList<int>>();
            result.ShouldBe(expected); // Checks content and order
            result.Count.ShouldBe(3);
        }

        [Fact]
        public void FindAll_CustomList_Int_PredicateMatchesMultiple_ReturnsAllMatchingElements()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 2, 3, 4, 5, 2, 6, 2 };
            Predicate<int> predicate = x => x == 2;
            List<int> expected = new List<int> { 2, 2, 2 };

            // Act
            IList<int> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<IList<int>>();
            result.ShouldBe(expected);
            result.Count.ShouldBe(3);
        }

        [Fact]
        public void FindAll_List_Int_PredicateMatchesOne_ReturnsSingleElementList()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 10).Take(5).ToList();
            int target = sourceList[2]; // Ensure one element matches
            Predicate<int> predicate = x => x == target;
        
            // Act
            IList<int> result = sourceList.FindAll(predicate);
        
            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(sourceList.Count(x => x == target)); // Handles if target was duplicated by chance
            result.ShouldAllBe(x => x == target);
        }

        [Fact]
        public void FindAll_List_Int_PredicateNoMatch_ReturnsEmptyList()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 10).Take(5).ToList();
            Predicate<int> predicate = x => x > 100; // No match

            // Act
            IList<int> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void FindAll_CustomList_Int_PredicateNoMatch_ReturnsEmptyList()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 10).Take(5));
            Predicate<int> predicate = x => x < 0; // No match for positive numbers

            // Act
            IList<int> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void FindAll_List_String_PredicateMatchesMultiple_ReturnsAllMatchingElements()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "apricot", "blueberry", "apple" };
            Predicate<string> predicate = s => s.StartsWith("ap");
            List<string> expected = new List<string> { "apple", "apricot", "apple" };

            // Act
            IList<string> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expected);
            result.Count.ShouldBe(3);
        }
        
        [Fact]
        public void FindAll_CustomList_String_PredicateMatchesMultiple_ReturnsAllMatchingElements()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "cat", "dog", "car", "cart" };
            Predicate<string> predicate = s => s.Contains("ca");
            List<string> expected = new List<string> { "cat", "car", "cart" };

            // Act
            IList<string> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expected);
            result.Count.ShouldBe(3);
        }

        [Fact]
        public void FindAll_List_CustomObject_PredicateMatchesMultiple_ReturnsMatchingObjects()
        {
            // Arrange
            TestObject obj1 = new TestObject { Id = 1, Value = "Match" };
            TestObject obj2 = new TestObject { Id = 2, Value = "NoMatch" };
            TestObject obj3 = new TestObject { Id = 3, Value = "Match" };
            List<TestObject> sourceList = new List<TestObject> { obj1, obj2, obj3 };
            Predicate<TestObject> predicate = obj => obj.Value == "Match";
            List<TestObject> expected = new List<TestObject> { obj1, obj3 };

            // Act
            IList<TestObject> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expected); // Checks content and order (uses Equals)
            result.Count.ShouldBe(2);
            // Ensure they are the same instances
            result[0].ShouldBeSameAs(obj1);
            result[1].ShouldBeSameAs(obj3);
        }

        [Fact]
        public void FindAll_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x > 0;

            // Act
            IList<int> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void FindAll_EmptyCustomList_ReturnsEmptyList()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            IList<string> result = sourceList.FindAll(predicate);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void FindAll_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindAll(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void FindAll_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindAll(predicate))
                  .ParamName.ShouldBe("match"); // .NET standard is "match"
        }
        
        [Fact]
        public void FindAll_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindAll(predicate))
                  .ParamName.ShouldBe("match");
        }

        [Fact]
        public void FindAll_ReturnedListIsNewInstance_List()
        {
            // Arrange
            List<int> sourceList = new List<int> { 10, 20, 30 };
            Predicate<int> predicate = x => x > 15;
            List<int> originalSourceCopy = new List<int>(sourceList);

            // Act
            IList<int> result = sourceList.FindAll(predicate);
            // Try to modify the result list (assuming it's a List<T>)
            if (result is List<int> resultAsList)
            {
                resultAsList.Add(40);
            } else if (result.Count > 0) { // If it's some other IList<T> try removing
                 result.RemoveAt(0);
            }


            // Assert
            result.ShouldNotBeSameAs(sourceList); // Should be a new list instance
            sourceList.ShouldBe(originalSourceCopy); // Original list should be unchanged
        }

        [Fact]
        public void FindAll_ReturnedListIsNewInstance_CustomList()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 10, 20, 30 };
            Predicate<int> predicate = x => x > 15;
            // CustomList might not have a simple copy constructor, so check elements
            List<int> originalSourceElements = sourceList.ToList(); 
            int originalSourceCount = sourceList.Count;


            // Act
            IList<int> result = sourceList.FindAll(predicate);
            // Try to modify the result list
            if (result is List<int> resultAsList) // Standard FindAll returns List<T>
            {
                 resultAsList.Add(40);
            } else if (result.Count > 0) {
                 result.RemoveAt(0); // Generic IList modification
            }


            // Assert
            result.ShouldNotBeSameAs(sourceList); // Should be a new list instance
            sourceList.Count.ShouldBe(originalSourceCount); // Original list count should be unchanged
            sourceList.ToList().ShouldBe(originalSourceElements); // Original list elements should be unchanged
        }
    }
}
