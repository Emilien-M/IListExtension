using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class RemoveRangeTest
    {
        private GenerateFactory GenerateFactory { get; }
        
        public RemoveRangeTest()
        {
            GenerateFactory = new GenerateFactory();
        }
        
        [Fact]
        public void RemoveRangeNumberTest()
        {
            IEnumerable<int> items = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            IList<int> ilistItems = new List<int>(items);
            
            CustomList<int> customListItems = new CustomList<int>(items);
            
            List<int> listItems = new List<int>(items);
            
            ilistItems.RemoveRange(5, 7);
            customListItems.RemoveRange(5, 7);
            listItems.RemoveRange(5, 7);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void RemoveRangeFloatTest()
        {
            IEnumerable<float> items = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            IList<float> ilistItems = new List<float>(items);
            
            CustomList<float> customListItems = new CustomList<float>(items);
            
            List<float> listItems = new List<float>(items);
            
            ilistItems.RemoveRange(5, 7);
            customListItems.RemoveRange(5, 7);
            listItems.RemoveRange(5, 7);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void RemoveRangeStringTest()
        {
            IEnumerable<string> items = GenerateFactory.GenerateRandomString(20).Take(20).ToList();
            
            IList<string> ilistItems = new List<string>(items);
            
            CustomList<string> customListItems = new CustomList<string>(items);
            
            List<string> listItems = new List<string>(items);
            
            ilistItems.RemoveRange(5, 7);
            customListItems.RemoveRange(5, 7);
            listItems.RemoveRange(5, 7);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void RemoveRangeBoolTest()
        {
            IEnumerable<bool> items = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            IList<bool> ilistItems = new List<bool>(items);
            
            CustomList<bool> customListItems = new CustomList<bool>(items);
            
            List<bool> listItems = new List<bool>(items);
            
            ilistItems.RemoveRange(5, 7);
            customListItems.RemoveRange(5, 7);
            listItems.RemoveRange(5, 7);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void RemoveRangeDateTimeTest()
        {
            IEnumerable<DateTime> items = GenerateFactory.GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(60)).Take(20).ToList();
            
            IList<DateTime> ilistItems = new List<DateTime>(items);
            
            CustomList<DateTime> customListItems = new CustomList<DateTime>(items);
            
            List<DateTime> listItems = new List<DateTime>(items);
            
            ilistItems.RemoveRange(5, 7);
            customListItems.RemoveRange(5, 7);
            listItems.RemoveRange(5, 7);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeFailTest()
        {
            IList<int> ilistNumber = null;
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.RemoveRange(0,0));

            ilistNumber = new List<int>
            {
                1,2,3,4
            };

            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(5, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(1, 6));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(-1, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(1, -1));
            
            ilistNumber = new CustomList<int>
            {
                1,2,3,4
            };
            
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(5, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(1, 6));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(-1, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.RemoveRange(1, -1));
        }
    }
}