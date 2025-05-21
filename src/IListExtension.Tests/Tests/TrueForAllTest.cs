using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class TrueForAllTest
    {
        private GenerateFactory GenerateFactory { get; }

        public TrueForAllTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public bool IsValid { get; set; }
        }

        [Fact]
        public void TrueForAll_List_Int_AllPositive_ReturnsTrue()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 5, 10, 23, 100 };
            Predicate<int> predicate = x => x > 0;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_CustomList_Int_AllPositive_ReturnsTrue()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 5, 10, 23, 100 };
            Predicate<int> predicate = x => x > 0;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_List_Int_ContainsNegative_ReturnsFalse()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 5, -10, 23, 100 };
            Predicate<int> predicate = x => x > 0;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void TrueForAll_CustomList_Int_ContainsZero_ReturnsFalseForPredicateGreaterThanZero()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 0, 10, 23 };
            Predicate<int> predicate = x => x > 0;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse();
        }
        
        [Fact]
        public void TrueForAll_List_String_AllNonEmpty_ReturnsTrue()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_CustomList_String_AllNonEmpty_ReturnsTrue()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b", " " }; // " " is not empty
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_List_String_ContainsEmpty_ReturnsFalse()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "", "cherry" };
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse();
        }
        
        [Fact]
        public void TrueForAll_List_String_ContainsNull_ReturnsFalse()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", null, "cherry" };
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse();
        }
        
        [Fact]
        public void TrueForAll_CustomList_String_ContainsNullOrEmpty_ReturnsFalse()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "test", null, "" };
            Predicate<string> predicate = s => !string.IsNullOrEmpty(s);

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse(); // Stops at null
        }

        [Fact]
        public void TrueForAll_List_CustomObject_AllValid_ReturnsTrue()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, IsValid = true },
                new TestObject { Id = 2, IsValid = true },
                new TestObject { Id = 3, IsValid = true }
            };
            Predicate<TestObject> predicate = obj => obj.IsValid;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_CustomList_CustomObject_AllValid_ReturnsTrue()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>
            {
                new TestObject { Id = 1, IsValid = true },
                new TestObject { Id = 2, IsValid = true }
            };
            Predicate<TestObject> predicate = obj => obj.IsValid;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }
        
        [Fact]
        public void TrueForAll_List_CustomObject_OneInvalid_ReturnsFalse()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, IsValid = true },
                new TestObject { Id = 2, IsValid = false }, // This one is invalid
                new TestObject { Id = 3, IsValid = true }
            };
            Predicate<TestObject> predicate = obj => obj.IsValid;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void TrueForAll_EmptyList_ReturnsTrue()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x > 0; // Predicate doesn't matter for empty list

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_EmptyCustomList_ReturnsTrue()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => s.Length > 5;

            // Act
            bool result = sourceList.TrueForAll(predicate);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void TrueForAll_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.TrueForAll(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void TrueForAll_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            // .NET standard parameter name for predicate in TrueForAll is "match"
            Should.Throw<ArgumentNullException>(() => sourceList.TrueForAll(predicate))
                  .ParamName.ShouldBe("match"); 
        }
        
        [Fact]
        public void TrueForAll_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.TrueForAll(predicate))
                  .ParamName.ShouldBe("match");
        }
    }
}
