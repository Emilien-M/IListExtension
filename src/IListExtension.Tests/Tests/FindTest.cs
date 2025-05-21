using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class FindTest
    {
        private GenerateFactory GenerateFactory { get; }

        public FindTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject : IEquatable<TestObject>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }

            public bool Equals(TestObject other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id == other.Id && Name == other.Name && Category == other.Category;
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
                return HashCode.Combine(Id, Name, Category);
            }
        }

        [Fact]
        public void Find_List_Int_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 10;
            int expected = 10;

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void Find_CustomList_Int_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 10;
            int expected = 10;

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void Find_List_Int_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            // Arrange
            List<int> sourceList = new List<int> { 2, 8, 12, 5, 12, 7 };
            Predicate<int> predicate = x => x == 12;
            int expected = 12; // The first 12

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(expected);
            sourceList.IndexOf(result).ShouldBe(2); // Ensure it is the first occurrence
        }
        
        [Fact]
        public void Find_CustomList_Int_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 2, 8, 12, 5, 12, 7 };
            Predicate<int> predicate = x => x == 12;
            int expected = 12;

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(expected);
            sourceList.IndexOf(result).ShouldBe(2);
        }


        [Fact]
        public void Find_List_Int_PredicateNoMatch_ReturnsDefault()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 50).Take(10).ToList();
            Predicate<int> predicate = x => x > 100; // No element will satisfy this

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(default(int)); // 0 for int
        }
        
        [Fact]
        public void Find_CustomList_Int_PredicateNoMatch_ReturnsDefault()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 50).Take(10));
            Predicate<int> predicate = x => x < 0; // Assuming positive numbers from factory

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(default(int));
        }

        [Fact]
        public void Find_List_String_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s == "banana";
            string expected = "banana";

            // Act
            string result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void Find_List_String_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            List<string> sourceList = GenerateFactory.GenerateRandomString(5).Take(5).ToList();
            Predicate<string> predicate = s => s == "nonexistent";

            // Act
            string result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeNull(); // Default for reference types
        }
        
        [Fact]
        public void Find_CustomList_String_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            Predicate<string> predicate = s => s.Length > 100; // Unlikely match

            // Act
            string result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Find_List_CustomObject_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            // Arrange
            TestObject obj1 = new TestObject { Id = 1, Name = "Obj1", Category = "A" };
            TestObject obj2 = new TestObject { Id = 2, Name = "Obj2", Category = "B" };
            TestObject obj3 = new TestObject { Id = 3, Name = "Obj3", Category = "A" }; // Second match
            List<TestObject> sourceList = new List<TestObject> { obj1, obj2, obj3 };
            Predicate<TestObject> predicate = obj => obj.Category == "A";
            TestObject expected = obj1; // Should be the first object with Category "A"

            // Act
            TestObject result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeSameAs(expected); // Check for reference equality for the first match
        }
        
        [Fact]
        public void Find_CustomList_CustomObject_PredicateMatchesMultiple_ReturnsFirstElement()
        {
            // Arrange
            TestObject objA = new TestObject { Id = 10, Name = "ObjA", Category = "X" };
            TestObject objB = new TestObject { Id = 20, Name = "ObjB", Category = "Y" };
            TestObject objC = new TestObject { Id = 30, Name = "ObjC", Category = "X" }; 
            CustomList<TestObject> sourceList = new CustomList<TestObject> { objA, objB, objC };
            Predicate<TestObject> predicate = obj => obj.Category == "X";
            TestObject expected = objA;

            // Act
            TestObject result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeSameAs(expected);
        }

        [Fact]
        public void Find_List_CustomObject_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Obj1", Category = "A" },
                new TestObject { Id = 2, Name = "Obj2", Category = "B" }
            };
            Predicate<TestObject> predicate = obj => obj.Category == "NonExistent";

            // Act
            TestObject result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Find_EmptyList_Int_ReturnsDefault()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x == 5;

            // Act
            int result = sourceList.Find(predicate);

            // Assert
            result.ShouldBe(default(int));
        }

        [Fact]
        public void Find_EmptyCustomList_String_ReturnsNull()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => s == "test";

            // Act
            string result = sourceList.Find(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Find_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Find(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void Find_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Find(predicate))
                  .ParamName.ShouldBe("match"); // .NET standard is "match"
        }
        
        [Fact]
        public void Find_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.Find(predicate))
                  .ParamName.ShouldBe("match");
        }
    }
}
