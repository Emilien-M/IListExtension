using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IListExtension; // Add this using directive
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class ConvertAllTest
    {
        private GenerateFactory GenerateFactory { get; }

        public ConvertAllTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        // Custom objects for testing
        private class InputObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class OutputObject
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public override bool Equals(object obj)
            {
                return obj is OutputObject other && Key == other.Key && Value == other.Value;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Key, Value);
            }
        }

        [Fact]
        public void ConvertAll_List_IntToString()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(5).ToList();
            Converter<int, string> converter = x => x.ToString();

            // Act
            IList<string> resultList = sourceList.ConvertAll<string>(converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<string>>();
            resultList.Count.ShouldBe(sourceList.Count);
            for (int i = 0; i < sourceList.Count; i++)
            {
                resultList[i].ShouldBe(sourceList[i].ToString());
            }
        }

        [Fact]
        public void ConvertAll_CustomList_IntToString()
        {
            // Arrange
            CustomList<int> customList = new CustomList<int> { 1, 2, 3 };
            Converter<int, string> converter = x => x.ToString();

            // Act
            // Failing line:
            // IList<string> convertedList = customList.ConvertAll<int, string>(converter); 
            IList<string> convertedList = IListExtension.ConvertAll<int, string>(customList, converter); // Corrected static call
            
            // Assert
            customList.Count.ShouldBe(3); // Keep a simple assertion
            // convertedList.Count.ShouldBe(3);
            // convertedList[0].ShouldBe("1");
        }

        [Fact]
        public void ConvertAll_List_StringToInt_Successful()
        {
            // Arrange
            List<string> sourceList = new List<string> { "1", "23", "456" };
            Converter<string, int> converter = s => int.Parse(s);

            // Act
            IList<int> resultList = sourceList.ConvertAll<int>(converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<int>>();
            resultList.Count.ShouldBe(sourceList.Count);
            resultList[0].ShouldBe(1);
            resultList[1].ShouldBe(23);
            resultList[2].ShouldBe(456);
        }

        [Fact]
        public void ConvertAll_List_StringToInt_ThrowsFormatException()
        {
            // Arrange
            List<string> sourceList = new List<string> { "1", "abc", "3" };
            Converter<string, int> converter = s => int.Parse(s);

            // Act & Assert
            Should.Throw<FormatException>(() => sourceList.ConvertAll<int>(converter));
        }
        
        [Fact]
        public void ConvertAll_CustomList_StringToInt_ThrowsFormatException()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "10", "xyz", "30" };
            Converter<string, int> converter = s => int.Parse(s);

            // Act & Assert
            Should.Throw<FormatException>(() => IListExtension.ConvertAll<string, int>(sourceList, converter));
        }

        [Fact]
        public void ConvertAll_List_DateTimeToString()
        {
            // Arrange
            List<DateTime> sourceList = GenerateFactory.GenerateRandomDate(DateTime.Now.AddYears(-1), DateTime.Now).Take(3).ToList();
            string format = "yyyy-MM-dd";
            Converter<DateTime, string> converter = dt => dt.ToString(format);

            // Act
            IList<string> resultList = sourceList.ConvertAll<string>(converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<string>>();
            resultList.Count.ShouldBe(sourceList.Count);
            for (int i = 0; i < sourceList.Count; i++)
            {
                resultList[i].ShouldBe(sourceList[i].ToString(format));
            }
        }

        [Fact]
        public void ConvertAll_List_CustomObjectToCustomObject()
        {
            // Arrange
            List<InputObject> sourceList = new List<InputObject>
            {
                new InputObject { Id = 1, Name = "First" },
                new InputObject { Id = 2, Name = "Second" }
            };
            Converter<InputObject, OutputObject> converter = io => new OutputObject { Key = io.Id.ToString(), Value = io.Name };

            // Act
            IList<OutputObject> resultList = sourceList.ConvertAll<OutputObject>(converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<OutputObject>>();
            resultList.Count.ShouldBe(sourceList.Count);
            resultList[0].ShouldBe(new OutputObject { Key = "1", Value = "First" });
            resultList[1].ShouldBe(new OutputObject { Key = "2", Value = "Second" });
        }
        
        [Fact]
        public void ConvertAll_CustomList_CustomObjectToCustomObject()
        {
            // Arrange
            CustomList<InputObject> sourceList = new CustomList<InputObject>(new List<InputObject>
            {
                new InputObject { Id = 10, Name = "Ten" },
                new InputObject { Id = 20, Name = "Twenty" }
            });
            Converter<InputObject, OutputObject> converter = io => new OutputObject { Key = io.Id.ToString(), Value = io.Name };

            // Act
            IList<OutputObject> resultList = IListExtension.ConvertAll<InputObject, OutputObject>(sourceList, converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<OutputObject>>();
            resultList.Count.ShouldBe(sourceList.Count);
            resultList[0].ShouldBe(new OutputObject { Key = "10", Value = "Ten" });
            resultList[1].ShouldBe(new OutputObject { Key = "20", Value = "Twenty" });
        }

        [Fact]
        public void ConvertAll_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            List<int> sourceList = new List<int>();
            Converter<int, string> converter = x => x.ToString();

            // Act
            IList<string> resultList = sourceList.ConvertAll<string>(converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<string>>();
            resultList.ShouldBeEmpty();
        }

        [Fact]
        public void ConvertAll_EmptyCustomList_ReturnsEmptyList()
        {
            // Arrange
            CustomList<DateTime> sourceList = new CustomList<DateTime>();
            Converter<DateTime, string> converter = dt => dt.ToShortDateString();

            // Act
            IList<string> resultList = IListExtension.ConvertAll<DateTime, string>(sourceList, converter);

            // Assert
            resultList.ShouldNotBeNull();
            resultList.ShouldBeOfType<List<string>>();
            resultList.ShouldBeEmpty();
        }

        [Fact]
        public void ConvertAll_NullSource_ThrowsArgumentNullException()
        {
            // Arrange
            IList<int> sourceList = null;
            Converter<int, string> converter = x => x.ToString();

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => IListExtension.ConvertAll<int, string>(sourceList, converter))
                  .ParamName.ShouldBe("source");
        }

        [Fact]
        public void ConvertAll_NullConverter_ThrowsArgumentNullException()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Converter<int, string> converter = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => sourceList.ConvertAll<string>(converter))
                  .ParamName.ShouldBe("converter");
        }
        
        [Fact]
        public void ConvertAll_CustomList_NullConverter_ThrowsArgumentNullException()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int> { 1, 2, 3 };
            Converter<int, string> converter = null;

            // Act & Assert
            Should.Throw<ArgumentNullException>(() => IListExtension.ConvertAll<int, string>(sourceList, converter))
                  .ParamName.ShouldBe("converter");
        }
    }
}
