using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class RemoveAllTest
    {
        private GenerateFactory GenerateFactory { get; }
        
        public RemoveAllTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        [Fact]
        public void RemoveAllNumberTest()
        {
            IEnumerable<int> initNumber = GenerateFactory.GenerateRandomInt(0, 20).Take(20).ToList();
            
            IList<int> ilistNumber = new List<int>(initNumber);
            CustomList<int> customListNumber = new CustomList<int>(initNumber);
            List<int> listNumber = new List<int>(initNumber);

            int ilistresult = ilistNumber.RemoveAll(o => o < 15);
            int customListResult = customListNumber.RemoveAll(o => o < 15);
            int listResult = listNumber.RemoveAll(o => o < 15);
            
            ilistNumber.ShouldBe(listNumber);
            customListNumber.ShouldBe(listNumber);
            
            ilistresult.ShouldBe(listResult);
            customListResult.ShouldBe(listResult);
        }
        
        [Fact]
        public void RemoveAllStringTest()
        {
            IEnumerable<string> initString = GenerateFactory.GenerateRandomString(50).Take(20).ToList();
            
            IList<string> ilistString = new List<string>(initString);
            CustomList<string> customListString = new CustomList<string>(initString);
            List<string> listString = new List<string>(initString);

            int ilistresult = ilistString.RemoveAll(o => o.Contains('a'));
            int customListResult = customListString.RemoveAll(o => o.Contains('a'));
            int listResult = listString.RemoveAll(o => o.Contains('a'));
            
            ilistString.ShouldBe(listString);
            customListString.ShouldBe(listString);
            
            ilistresult.ShouldBe(listResult);
            customListResult.ShouldBe(listResult);
        }
        
        [Fact]
        public void RemoveAllFloatTest()
        {
            IEnumerable<float> initFloat = GenerateFactory.GenerateRandomFloat(0, 20).Take(20).ToList();
            
            IList<float> ilistFloat = new List<float>(initFloat);
            CustomList<float> customListFloat = new CustomList<float>(initFloat);
            List<float> listFloat = new List<float>(initFloat);

            int ilistresult = ilistFloat.RemoveAll(o => o < 15);
            int customListResult = customListFloat.RemoveAll(o => o < 15);
            int listResult = listFloat.RemoveAll(o => o < 15);
            
            ilistFloat.ShouldBe(listFloat);
            customListFloat.ShouldBe(listFloat);
            
            ilistresult.ShouldBe(listResult);
            customListResult.ShouldBe(listResult);
        }
        
        [Fact]
        public void RemoveAllBoolTest()
        {
            IEnumerable<bool> initBool = GenerateFactory.GenerateRandomBool().Take(20).ToList();
            
            IList<bool> ilistBool = new List<bool>(initBool);
            CustomList<bool> customListBool = new CustomList<bool>(initBool);
            List<bool> listBool = new List<bool>(initBool);

            int ilistresult = ilistBool.RemoveAll(o => o);
            int customListResult = customListBool.RemoveAll(o => o);
            int listResult = listBool.RemoveAll(o => o);
            
            ilistBool.ShouldBe(listBool);
            customListBool.ShouldBe(listBool);
            
            ilistresult.ShouldBe(listResult);
            customListResult.ShouldBe(listResult);
        }
        
        [Fact]
        public void RemoveAllDatetimeTest()
        {
            IEnumerable<DateTime> initDatetime = GenerateFactory.GenerateRandomDate(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1)).Take(20).ToList();
            
            IList<DateTime> ilistDatetime = new List<DateTime>(initDatetime);
            CustomList<DateTime> customListDatetime = new CustomList<DateTime>(initDatetime);
            List<DateTime> listDatetime = new List<DateTime>(initDatetime);

            int ilistresult = ilistDatetime.RemoveAll(o => o < DateTime.Now);
            int customListResult = customListDatetime.RemoveAll(o => o < DateTime.Now);
            int listResult = listDatetime.RemoveAll(o => o < DateTime.Now);
            
            ilistDatetime.ShouldBe(listDatetime);
            customListDatetime.ShouldBe(listDatetime);
            
            ilistresult.ShouldBe(listResult);
            customListResult.ShouldBe(listResult);
        }
    }
}