using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class FindLastTest
    {
        private GenerateFactory GenerateFactory { get; }

        public FindLastTest()
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
        public void FindLast_List_Int_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 10;
            int expected = 10;

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void FindLast_CustomList_Int_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 5, 10, 15, 20 };
            Predicate<int> predicate = x => x == 10;
            int expected = 10;

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void FindLast_List_Int_PredicateMatchesMultiple_ReturnsLastElement()
        {
            // Arrange
            List<int> sourceList = new List<int> { 2, 12, 8, 5, 12, 7 }; // Last '12' is at index 4
            Predicate<int> predicate = x => x == 12;
            int expected = 12; 

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(expected);
            // Verify it's the last occurrence by checking its index using List.LastIndexOf
            sourceList.LastIndexOf(result).ShouldBe(4);
        }
        
        [Fact]
        public void FindLast_CustomList_Int_PredicateMatchesMultiple_ReturnsLastElement()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 2, 12, 8, 5, 12, 7 };
            Predicate<int> predicate = x => x == 12;
            int expected = 12;

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(expected);
            // Verify it's the last occurrence by checking its index using a manual loop for CustomList
            int lastIndex = -1;
            for(int i = 0; i < sourceList.Count; i++)
            {
                if (predicate(sourceList[i])) lastIndex = i;
            }
            lastIndex.ShouldBe(4); // Verify the manually found last index is correct
            sourceList[lastIndex].ShouldBe(result); // Verify the item at the last index is the one found by FindLast
        }


        [Fact]
        public void FindLast_List_Int_PredicateNoMatch_ReturnsDefault()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 50).Take(10).ToList();
            Predicate<int> predicate = x => x > 100; // No element will satisfy this

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(default(int)); // 0 for int
        }
        
        [Fact]
        public void FindLast_CustomList_Int_PredicateNoMatch_ReturnsDefault()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>(GenerateFactory.GenerateRandomInt(1, 50).Take(10));
            Predicate<int> predicate = x => x < 0; // Assuming positive numbers from factory

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(default(int));
        }

        [Fact]
        public void FindLast_List_String_PredicateMatchesOne_ReturnsElement()
        {
            // Arrange
            List<string> sourceList = new List<string> { "apple", "banana", "cherry" };
            Predicate<string> predicate = s => s == "banana";
            string expected = "banana";

            // Act
            string result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void FindLast_List_String_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            List<string> sourceList = GenerateFactory.GenerateRandomString(5).Take(5).ToList();
            Predicate<string> predicate = s => s == "nonexistent";

            // Act
            string result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeNull(); // Default for reference types
        }
        
        [Fact]
        public void FindLast_CustomList_String_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            Predicate<string> predicate = s => s.Length > 100; // Unlikely match

            // Act
            string result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void FindLast_List_CustomObject_PredicateMatchesMultiple_ReturnsLastElement()
        {
            // Arrange
            TestObject obj1 = new TestObject { Id = 1, Name = "Obj1", Category = "A" };
            TestObject obj2 = new TestObject { Id = 2, Name = "Obj2", Category = "B" };
            TestObject obj3 = new TestObject { Id = 3, Name = "Obj3", Category = "A" }; // Last match
            List<TestObject> sourceList = new List<TestObject> { obj1, obj2, obj3 };
            Predicate<TestObject> predicate = obj => obj.Category == "A";
            TestObject expected = obj3; // Should be the last object with Category "A"

            // Act
            TestObject result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeSameAs(expected); // Check for reference equality for the last match
        }
        
        [Fact]
        public void FindLast_CustomList_CustomObject_PredicateMatchesMultiple_ReturnsLastElement()
        {
            // Arrange
            TestObject objA = new TestObject { Id = 10, Name = "ObjA", Category = "X" };
            TestObject objB = new TestObject { Id = 20, Name = "ObjB", Category = "Y" };
            TestObject objC = new TestObject { Id = 30, Name = "ObjC", Category = "X" }; 
            CustomList<TestObject> sourceList = new CustomList<TestObject> { objA, objB, objC };
            Predicate<TestObject> predicate = obj => obj.Category == "X";
            TestObject expected = objC;

            // Act
            TestObject result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeSameAs(expected);
        }

        [Fact]
        public void FindLast_List_CustomObject_PredicateNoMatch_ReturnsNull()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Name = "Obj1", Category = "A" },
                new TestObject { Id = 2, Name = "Obj2", Category = "B" }
            };
            Predicate<TestObject> predicate = obj => obj.Category == "NonExistent";

            // Act
            TestObject result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void FindLast_EmptyList_Int_ReturnsDefault()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Predicate<int> predicate = x => x == 5;

            // Act
            int result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBe(default(int));
        }

        [Fact]
        public void FindLast_EmptyCustomList_String_ReturnsNull()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();
            Predicate<string> predicate = s => s == "test";

            // Act
            string result = sourceList.FindLast(predicate);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void FindLast_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Predicate<int> predicate = x => x > 0;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindLast(predicate))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void FindLast_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Predicate<int> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindLast(predicate))
                  .ParamName.ShouldBe("match"); // .NET standard is "match"
        }
        
        [Fact]
        public void FindLast_CustomList_NullPredicate_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject> { new TestObject() };
            Predicate<TestObject> predicate = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.FindLast(predicate))
                  .ParamName.ShouldBe("match");
        }
    }
}
