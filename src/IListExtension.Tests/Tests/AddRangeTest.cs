using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class AddRangeTest
    {
        private GenerateFactory GenerateFactory { get; }
        
        public AddRangeTest()
        {
            GenerateFactory = new GenerateFactory();
        }
        
        [Fact]
        public void AddRangeNumberTest()
        {
            IList<int> ilistNumber = new List<int>();
            
            List<int> listNumber = new List<int>();

            IEnumerable<int> numberToAdd = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            ilistNumber.AddRange(numberToAdd);
            listNumber.AddRange(numberToAdd);
            
            ilistNumber.ShouldBe(listNumber);
        }
        
        [Fact]
        public void AddRangeStringTest()
        {
            IList<string> ilistString = new List<string>();
            
            List<string> listString = new List<string>();

            IEnumerable<string> stringToAdd = GenerateFactory.GenerateRandomString(10).Take(20).ToList();
            
            ilistString.AddRange(stringToAdd);
            listString.AddRange(stringToAdd);
            
            ilistString.ShouldBe(listString);
        }
        
        [Fact]
        public void AddRangeFloatTest()
        {
            IList<float> ilistFloat = new List<float>();
            
            List<float> listFloat = new List<float>();

            IEnumerable<float> floatToAdd = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            ilistFloat.AddRange(floatToAdd);
            listFloat.AddRange(floatToAdd);
            
            ilistFloat.ShouldBe(listFloat);
        }
        
        [Fact]
        public void AddRangeBoolTest()
        {
            IList<bool> ilistBool = new List<bool>();
            
            List<bool> listBool = new List<bool>();

            IEnumerable<bool> boolToAdd = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            ilistBool.AddRange(boolToAdd);
            listBool.AddRange(boolToAdd);
            
            ilistBool.ShouldBe(listBool);
        }
        
        [Fact]
        public void AddRangeDateTest()
        {
            IList<DateTime> ilistDatetime = new List<DateTime>();
            
            List<DateTime> listDatetime = new List<DateTime>();

            IEnumerable<DateTime> datetimeToAdd = GenerateFactory.GenerateRandomDate(DateTime.Now.AddYears(-1), DateTime.Now).Take(20).ToList();
            
            ilistDatetime.AddRange(datetimeToAdd);
            listDatetime.AddRange(datetimeToAdd);
            
            ilistDatetime.ShouldBe(listDatetime);
        }
        
        [Fact]
        public void AddRangeFailTest()
        {
            IList<int> ilistNumber = null;
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.AddRange(null));

            ilistNumber = new List<int>();
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.AddRange(null));
        }
    }
}