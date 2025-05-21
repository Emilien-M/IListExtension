using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class GetRangeTest
    {
        private GenerateFactory GenerateFactory { get; }

        public GetRangeTest()
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
        public void GetRange_List_Int_SubListFromMiddle()
        {
            // Arrange
            List<int> sourceList = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            int index = 2;
            int count = 3;
            List<int> expected = new List<int> { 2, 3, 4 };

            // Act
            IList<int> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<IList<int>>(); // Should be List<T> in practice
            result.ShouldBe(expected);
        }

        [Fact]
        public void GetRange_CustomList_Int_SubListFromMiddle()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 0, 1, 2, 3, 4, 5, 6 };
            int index = 2;
            int count = 3;
            CustomList<int> expected = new CustomList<int> { 2, 3, 4 }; // Assuming CustomList can be compared

            // Act
            IList<int> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<IList<int>>();
            result.ShouldBe(expected); // Shouldly will compare elements
        }

        [Fact]
        public void GetRange_List_String_SubListFromBeginning()
        {
            // Arrange
            List<string> sourceList = new List<string> { "a", "b", "c", "d", "e" };
            int index = 0;
            int count = 3;
            List<string> expected = new List<string> { "a", "b", "c" };

            // Act
            IList<string> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void GetRange_CustomList_String_SubListFromBeginning()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b", "c", "d", "e" };
            int index = 0;
            int count = 3;
            List<string> expected = new List<string> { "a", "b", "c" };

            // Act
            IList<string> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void GetRange_List_TestObject_SubListUntilEnd()
        {
            // Arrange
            TestObject t1 = new TestObject { Id = 1, Value = "V1" };
            TestObject t2 = new TestObject { Id = 2, Value = "V2" };
            TestObject t3 = new TestObject { Id = 3, Value = "V3" };
            TestObject t4 = new TestObject { Id = 4, Value = "V4" };
            List<TestObject> sourceList = new List<TestObject> { t1, t2, t3, t4 };
            int index = 1;
            int count = 3; // From t2 to t4
            List<TestObject> expected = new List<TestObject> { t2, t3, t4 };

            // Act
            IList<TestObject> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldBe(expected);
            result.Count.ShouldBe(3);
            result[0].ShouldBeSameAs(t2); // Check instance if objects are complex
        }

        [Fact]
        public void GetRange_List_CountZero_ReturnsEmptyList()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(5).ToList();
            int index = 2;
            int count = 0;

            // Act
            IList<int> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }
        
        [Fact]
        public void GetRange_CustomList_CountZero_ReturnsEmptyList()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(5));
            int index = 0;
            int count = 0;

            // Act
            IList<string> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }


        [Fact]
        public void GetRange_List_CountEqualsRemaining_ReturnsAllRemaining()
        {
            // Arrange
            List<int> sourceList = new List<int> { 10, 20, 30, 40, 50 };
            int index = 2; // Start from 30
            int count = 3; // 30, 40, 50
            List<int> expected = new List<int> { 30, 40, 50 };

            // Act
            IList<int> result = sourceList.GetRange(index, count);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void GetRange_ReturnedListIsNewInstance_List()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3, 4, 5 };
            List<int> originalSourceCopy = new List<int>(sourceList);

            // Act
            IList<int> result = sourceList.GetRange(1, 2); // Get {2, 3}
            // Modify result (assuming it's a List<T> for Add, or just clear)
            if (result is List<int> listResult) listResult.Add(100);
            else result.Clear();


            // Assert
            result.ShouldNotBeSameAs(sourceList);
            sourceList.ShouldBe(originalSourceCopy); // Original list should be unchanged
        }
        
        [Fact]
        public void GetRange_ReturnedListIsNewInstance_CustomList()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 2, 3, 4, 5 };
            List<int> originalSourceElements = sourceList.ToList();

            // Act
            IList<int> result = sourceList.GetRange(1, 2); // Get {2, 3}
            if (result is List<int> listResult) listResult.Add(100);
            else result.Clear();


            // Assert
            result.ShouldNotBeSameAs(sourceList);
            sourceList.ToList().ShouldBe(originalSourceElements); 
        }

        [Fact]
        public void GetRange_EmptySourceList_IndexZeroCountZero_ReturnsEmptyList()
        {
            // Arrange
            List<int> sourceList = new List<int>();

            // Act
            IList<int> result = sourceList.GetRange(0, 0);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }
        
        [Fact]
        public void GetRange_EmptyCustomSourceList_IndexZeroCountZero_ReturnsEmptyList()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>();

            // Act
            IList<string> result = sourceList.GetRange(0, 0);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }


        [Theory]
        [InlineData(1, 0)] // Index > 0 on empty list
        [InlineData(0, 1)] // Count > 0 on empty list
        public void GetRange_EmptySourceList_InvalidIndexOrCount_ThrowsArgumentException(int index, int count)
        {
            // Arrange
            List<int> sourceList = new List<int>();

            // Act & Assert
            // List<T>.GetRange throws ArgumentException if index and count don't denote a valid range
            // Or ArgumentOutOfRangeException if index or count is negative.
            // For empty list, index=0, count=0 is valid. Any other means index > count or index+count > list.Count.
            // List.GetRange specific: if index=0, count=1 on empty list -> ArgumentException "not enough elements"
            // if index=1, count=0 on empty list -> ArgumentOutOfRangeException for index
            if (index > 0) {
                Should.Throw<ArgumentOutOfRangeException>(() => sourceList.GetRange(index, count))
                    .ParamName.ShouldBe("index");
            } else { // index == 0, count > 0
                 Should.Throw<ArgumentException>(() => sourceList.GetRange(index, count));
            }
        }
        
        [Theory]
        [InlineData(1, 0)] 
        [InlineData(0, 1)] 
        public void GetRange_EmptyCustomSourceList_InvalidIndexOrCount_ThrowsAppropriateException(int index, int count)
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>();

            // Act & Assert
            // Expecting behavior similar to List<T>
            if (index > 0) { // index out of range for empty list (if not 0)
                Should.Throw<ArgumentOutOfRangeException>(() => sourceList.GetRange(index, count))
                    .ParamName.ShouldBe("index");
            } else { // index is 0, count must be > 0 to be invalid here
                 Should.Throw<ArgumentException>(() => sourceList.GetRange(index, count));
            }
        }


        [Fact]
        public void GetRange_IndexNegative_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 10).Take(5).ToList();

            // Act & Assert
            Should.Throw<ArgumentOutOfRangeException>(() => sourceList.GetRange(-1, 2))
                  .ParamName.ShouldBe("index");
                  
            CustomList<int> customList = new CustomList<int>(sourceList);
            Should.Throw<ArgumentOutOfRangeException>(() => customList.GetRange(-1, 2))
                  .ParamName.ShouldBe("index");
        }

        [Fact]
        public void GetRange_CountNegative_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 10).Take(5).ToList();

            // Act & Assert
            Should.Throw<ArgumentOutOfRangeException>(() => sourceList.GetRange(0, -1))
                  .ParamName.ShouldBe("count");

            CustomList<int> customList = new CustomList<int>(sourceList);
            Should.Throw<ArgumentOutOfRangeException>(() => customList.GetRange(0, -1))
                .ParamName.ShouldBe("count");
        }

        [Fact]
        public void GetRange_IndexPlusCountExceedsSourceCount_ThrowsArgumentException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3, 4, 5 }; // Count = 5

            // Act & Assert
            // index = 3 (value 4), count = 3. 3+3=6 > 5
            Should.Throw<ArgumentException>(() => sourceList.GetRange(3, 3))
                  .Message.ShouldStartWith("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            
            CustomList<string> customList = new CustomList<string> { "a", "b", "c" }; // Count = 3
            // index = 1 ("b"), count = 3. 1+3=4 > 3
            Should.Throw<ArgumentException>(() => customList.GetRange(1, 3));
        }

        [Fact]
        public void GetRange_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.GetRange(0, 0))
                  .ParamName.ShouldBe("source");
        }
    }
}
