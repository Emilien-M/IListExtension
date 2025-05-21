using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class ExistsTest
    {
        private GenerateFactory GenerateFactory { get; }

        public ExistsTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Category { get; set; }
        }

        [Fact]
        public void Exists_List_Int_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 20).Take(10).ToList(); // e.g., [5, 12, 8, 15]
            sourceList.Add(15); // Ensure at least one element > 10
            Predicate<int> predicate = x => x > 10;

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Exists_CustomList_Int_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 20).Take(10));
            sourceList.Add(18); // Ensure at least one element > 10
            Predicate<int> predicate = x => x > 10;

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Exists_List_Int_PredicateFalse_ReturnsFalse()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 9).Take(5).ToList(); // All elements <= 10
            Predicate<int> predicate = x => x > 10;

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_CustomList_Int_PredicateFalse_ReturnsFalse()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 5).Take(5)); // All elements <= 10
            Predicate<int> predicate = x => x > 10;

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_List_String_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            List<string> sourceList = GenerateFactory.GenerateRandomString(5).Take(5).ToList();
            sourceList.Add("testValue"); // Ensure one starts with "test"
            Predicate<string> predicate = s => s.StartsWith("test");

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }
        
        [Fact]
        public void Exists_CustomList_String_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            sourceList.Add("testSample");
            Predicate<string> predicate = s => s.StartsWith("test");

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Exists_List_String_PredicateFalse_ReturnsFalse()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s.StartsWith("test");

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }
        
        [Fact]
        public void Exists_CustomList_String_PredicateFalse_ReturnsFalse()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "orange", "grape", "kiwi" };
            Predicate<string> predicate = s => s.StartsWith("test");

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_List_CustomObject_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Category = "A" },
                new TestObject { Id = 2, Category = "B" },
                new TestObject { Id = 3, Category = "target" }
            };
            Predicate<TestObject> predicate = obj => obj.Category == "target";

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Exists_CustomList_CustomObject_PredicateTrue_ReturnsTrue()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>(new List<TestObject>
            {
                new TestObject { Id = 1, Category = "X" },
                new TestObject { Id = 2, Category = "target" },
                new TestObject { Id = 3, Category = "Y" }
            });
            Predicate<TestObject> predicate = obj => obj.Category == "target";

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeTrue();
        }


        [Fact]
        public void Exists_List_CustomObject_PredicateFalse_ReturnsFalse()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Category = "A" },
                new TestObject { Id = 2, Category = "B" }
            };
            Predicate<TestObject> predicate = obj => obj.Category == "target";

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_EmptyList_ReturnsFalse()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x > 0;

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_EmptyCustomList_ReturnsFalse()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.Exists(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Exists_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Exists(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void Exists_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Exists(predicate))
                  .ParamName.ShouldBe("match"); // .NET standard is "match" for predicate
        }
        
        [Fact]
        public void Exists_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b" };
            Predicate<string> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Exists(predicate))
                  .ParamName.ShouldBe("match");
        }
    }
}
