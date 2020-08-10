using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class InsertRangeTest
    {
        private GenerateFactory GenerateFactory { get; }
        
        public InsertRangeTest()
        {
            GenerateFactory = new GenerateFactory();
        }
        
        [Fact]
        public void InsertRangeNumberTest()
        {
            IEnumerable<int> items = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            IList<int> ilistItems = new List<int>(items);
            
            CustomList<int> customListItems = new CustomList<int>(items);
            
            List<int> listItems = new List<int>(items);

            IEnumerable<int> itemsToAdd = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            ilistItems.InsertRange(2, itemsToAdd);
            customListItems.InsertRange(2, itemsToAdd);
            listItems.InsertRange(2, itemsToAdd);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeStringTest()
        {
            IEnumerable<string> items = GenerateFactory.GenerateRandomString(20).Take(20).ToList();
            
            IList<string> ilistItems = new List<string>(items);
            
            CustomList<string> customListItems = new CustomList<string>(items);
            
            List<string> listItems = new List<string>(items);

            IEnumerable<string> itemsToAdd = GenerateFactory.GenerateRandomString(20).Take(20).ToList();
            
            ilistItems.InsertRange(2, itemsToAdd);
            customListItems.InsertRange(2, itemsToAdd);
            listItems.InsertRange(2, itemsToAdd);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeFloatTest()
        {
            IEnumerable<float> items = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            IList<float> ilistItems = new List<float>(items);
            
            CustomList<float> customListItems = new CustomList<float>(items);
            
            List<float> listItems = new List<float>(items);

            IEnumerable<float> itemsToAdd = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            ilistItems.InsertRange(2, itemsToAdd);
            customListItems.InsertRange(2, itemsToAdd);
            listItems.InsertRange(2, itemsToAdd);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeBoolTest()
        {
            IEnumerable<bool> items = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            IList<bool> ilistItems = new List<bool>(items);
            
            CustomList<bool> customListItems = new CustomList<bool>(items);
            
            List<bool> listItems = new List<bool>(items);

            IEnumerable<bool> itemsToAdd = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            ilistItems.InsertRange(2, itemsToAdd);
            customListItems.InsertRange(2, itemsToAdd);
            listItems.InsertRange(2, itemsToAdd);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeDatetimeTest()
        {
            IEnumerable<DateTime> items = GenerateFactory.GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(60)).Take(20).ToList();
            
            IList<DateTime> ilistItems = new List<DateTime>(items);
            
            CustomList<DateTime> customListItems = new CustomList<DateTime>(items);
            
            List<DateTime> listItems = new List<DateTime>(items);

            IEnumerable<DateTime> itemsToAdd = GenerateFactory.GenerateRandomDate(DateTime.Now, DateTime.Now.AddDays(60)).Take(20).ToList();
            
            ilistItems.InsertRange(2, itemsToAdd);
            customListItems.InsertRange(2, itemsToAdd);
            listItems.InsertRange(2, itemsToAdd);
            
            ilistItems.ShouldBe(listItems);
            customListItems.ShouldBe(listItems);
        }
        
        [Fact]
        public void InsertRangeFailTest()
        {
            IList<int> ilistNumber = null;
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.InsertRange(0,null));

            ilistNumber = new List<int>
            {
                1
            };
            
            Should.Throw<ArgumentNullException>(() => ilistNumber.InsertRange(0, null));
            
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.InsertRange(4, null));
            
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.InsertRange(-1, null));
            
            ilistNumber = new CustomList<int>
            {
                1
            };

            Should.Throw<ArgumentNullException>(() => ilistNumber.InsertRange(0,null));

            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.InsertRange(4, null));
            
            Should.Throw<ArgumentOutOfRangeException>(() => ilistNumber.InsertRange(-1, null));
        }
    }
}