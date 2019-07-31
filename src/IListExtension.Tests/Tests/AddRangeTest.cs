using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
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
            
            CustomList<int> customListNumber = new CustomList<int>();
            
            List<int> listNumber = new List<int>();

            IEnumerable<int> numberToAdd = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            ilistNumber.AddRange(numberToAdd);
            customListNumber.AddRange(numberToAdd);
            listNumber.AddRange(numberToAdd);
            
            ilistNumber.ShouldBe(listNumber);
            customListNumber.ShouldBe(listNumber);
        }
        
        [Fact]
        public void AddRangeStringTest()
        {
            IList<string> ilistString = new List<string>();
            
            CustomList<string> customListString = new CustomList<string>();
            
            List<string> listString = new List<string>();

            IEnumerable<string> stringToAdd = GenerateFactory.GenerateRandomString(10).Take(20).ToList();
            
            ilistString.AddRange(stringToAdd);
            customListString.AddRange(stringToAdd);
            listString.AddRange(stringToAdd);
            
            ilistString.ShouldBe(listString);
            customListString.ShouldBe(listString);
        }
        
        [Fact]
        public void AddRangeFloatTest()
        {
            IList<float> ilistFloat = new List<float>();
            
            CustomList<float> customListFloat = new CustomList<float>();
            
            List<float> listFloat = new List<float>();

            IEnumerable<float> floatToAdd = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            ilistFloat.AddRange(floatToAdd);
            customListFloat.AddRange(floatToAdd);
            listFloat.AddRange(floatToAdd);
            
            ilistFloat.ShouldBe(listFloat);
            customListFloat.ShouldBe(listFloat);
        }
        
        [Fact]
        public void AddRangeBoolTest()
        {
            IList<bool> ilistBool = new List<bool>();
            
            CustomList<bool> customListBool = new CustomList<bool>();
            
            List<bool> listBool = new List<bool>();

            IEnumerable<bool> boolToAdd = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            ilistBool.AddRange(boolToAdd);
            customListBool.AddRange(boolToAdd);
            listBool.AddRange(boolToAdd);
            
            ilistBool.ShouldBe(listBool);
            customListBool.ShouldBe(listBool);
        }
        
        [Fact]
        public void AddRangeDatetimeTest()
        {
            IList<DateTime> ilistDatetime = new List<DateTime>();
            
            CustomList<DateTime> customListDatetime = new CustomList<DateTime>();
            
            List<DateTime> listDatetime = new List<DateTime>();

            IEnumerable<DateTime> datetimeToAdd = GenerateFactory.GenerateRandomDate(DateTime.Now.AddYears(-1), DateTime.Now).Take(20).ToList();
            
            ilistDatetime.AddRange(datetimeToAdd);
            customListDatetime.AddRange(datetimeToAdd);
            listDatetime.AddRange(datetimeToAdd);
            
            ilistDatetime.ShouldBe(listDatetime);
            customListDatetime.ShouldBe(listDatetime);
        }
        
        [Fact]
        public void AddRangeFailTest()
        {
            IList<int> ilistNumber = null;
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.AddRange(null));

            ilistNumber = new List<int>();
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.AddRange(null));
            
            ilistNumber = new CustomList<int>();

            Should.Throw<ArgumentNullException>(() => ilistNumber.AddRange(null));
        }
    }
}