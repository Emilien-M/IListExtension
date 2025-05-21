using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class AsReadOnlyTest
    {
        private GenerateFactory GenerateFactory { get; }

        public AsReadOnlyTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        [Fact]
        public void AsReadOnly_WithList_ReturnsReadOnlyCollectionWithSameElements()
        {
            // Arrange
            List<int> originalList = GenerateFactory.GenerateRandomInt(0, 100).Take(10).ToList();

            // Act
            IReadOnlyCollection<int> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<int>>();
            readOnlyList.Count.ShouldBe(originalList.Count);
            for (int i = 0; i < originalList.Count; i++)
            {
                readOnlyList.ElementAt(i).ShouldBe(originalList[i]);
            }
        }

        [Fact]
        public void AsReadOnly_WithCustomList_ReturnsReadOnlyCollectionWithSameElements()
        {
            // Arrange
            CustomList<string> originalList = new CustomList<string>(GenerateFactory.GenerateRandomString(10).Take(5));

            // Act
            IReadOnlyCollection<string> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<string>>();
            readOnlyList.Count.ShouldBe(originalList.Count);
            for (int i = 0; i < originalList.Count; i++)
            {
                readOnlyList.ElementAt(i).ShouldBe(originalList[i]);
            }
        }

        [Fact]
        public void AsReadOnly_WithEmptyList_ReturnsEmptyReadOnlyCollection()
        {
            // Arrange
            List<object> originalList = new List<object>();

            // Act
            IReadOnlyCollection<object> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<object>>();
            readOnlyList.ShouldBeEmpty();
        }

        [Fact]
        public void AsReadOnly_WithEmptyCustomList_ReturnsEmptyReadOnlyCollection()
        {
            // Arrange
            CustomList<int> originalList = new CustomList<int>();

            // Act
            IReadOnlyCollection<int> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<int>>();
            readOnlyList.ShouldBeEmpty();
        }

        [Fact]
        public void AsReadOnly_AttemptToModifyReadOnlyCollection_ThrowsNotSupportedException()
        {
            // Arrange
            List<int> originalList = GenerateFactory.GenerateRandomInt(0, 100).Take(3).ToList();
            IReadOnlyCollection<int> iReadOnlyList = originalList.AsReadOnly(); 
            IList<int> readOnlyAsIList = (IList<int>)iReadOnlyList; // Cast to IList<T> for modification attempts

            // Act & Assert
            Should.Throw<NotSupportedException>(() => readOnlyAsIList.Add(4));
            Should.Throw<NotSupportedException>(() => readOnlyAsIList.Insert(0, 0));
            Should.Throw<NotSupportedException>(() => readOnlyAsIList.Remove(originalList[0]));
            Should.Throw<NotSupportedException>(() => readOnlyAsIList.RemoveAt(0));
            Should.Throw<NotSupportedException>(() => readOnlyAsIList.Clear());
            Should.Throw<NotSupportedException>(() => readOnlyAsIList[0] = 10);
        }
        
        [Fact]
        public void AsReadOnly_WithListOfDifferentDataTypes_ReturnsReadOnlyCollection()
        {
            // Arrange
            List<DateTime> originalList = GenerateFactory.GenerateRandomDate(DateTime.Now.AddYears(-1), DateTime.Now).Take(5).ToList();

            // Act
            IReadOnlyCollection<DateTime> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<DateTime>>();
            readOnlyList.Count.ShouldBe(originalList.Count);
            for (int i = 0; i < originalList.Count; i++)
            {
                readOnlyList.ElementAt(i).ShouldBe(originalList[i]);
            }
        }
        
        [Fact]
        public void AsReadOnly_WithCustomListOfDifferentDataTypes_ReturnsReadOnlyCollection()
        {
            // Arrange
            CustomList<bool> originalList = new CustomList<bool>(GenerateFactory.GenerateRandomBool().Take(5));

            // Act
            IReadOnlyCollection<bool> readOnlyList = originalList.AsReadOnly();

            // Assert
            readOnlyList.ShouldNotBeNull();
            readOnlyList.ShouldBeAssignableTo<IReadOnlyCollection<bool>>();
            readOnlyList.Count.ShouldBe(originalList.Count);
            for (int i = 0; i < originalList.Count; i++)
            {
                readOnlyList.ElementAt(i).ShouldBe(originalList[i]);
            }
        }

        [Fact]
        public void AsReadOnly_NullList_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> nullList = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => nullList.AsReadOnly());
        }
    }
}
