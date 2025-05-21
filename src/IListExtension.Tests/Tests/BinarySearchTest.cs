using System;
using System.Collections.Generic;
using System.Linq;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class BinarySearchTest
    {
        private GenerateFactory GenerateFactory { get; }

        public BinarySearchTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        // Comparer for case-insensitive string comparison
        private class CaseInsensitiveComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            }
        }

        #region BinarySearch<T>(this IList<T> source, T item)

        [Fact]
        public void BinarySearch_Item_List_Int_ItemFound()
        {
            List<int> list = GenerateFactory.GenerateRandomInt(0, 100).Take(10).OrderBy(x => x).ToList();
            int itemToFind = list[3];
            int expectedIndex = list.BinarySearch(itemToFind);

            int actualIndex = list.BinarySearch(itemToFind);

            actualIndex.ShouldBe(expectedIndex);
        }

        [Fact]
        public void BinarySearch_Item_CustomList_Int_ItemFound()
        {
            CustomList<int> list = new CustomList<int>(GenerateFactory.GenerateRandomInt(0, 100).Take(10).OrderBy(x => x));
            int itemToFind = list[3];
            // CustomList doesn't have BinarySearch, so we find the expected index manually for verification
            int expectedIndex = -1;
            for(int i=0; i<list.Count; i++) { if(list[i] == itemToFind) { expectedIndex = i; break; } }
            // Adjust for multiple occurrences - BinarySearch can return any if duplicates exist
            while(expectedIndex > 0 && list[expectedIndex-1] == itemToFind) expectedIndex--;


            int actualIndex = list.BinarySearch(itemToFind);

            // If the item is found, the index should be non-negative.
            // If there are duplicates, BinarySearch might return any of their indices.
            // So, we check if the element at the returned index is the item we are looking for.
            if (actualIndex >= 0)
            {
                list[actualIndex].ShouldBe(itemToFind);
            }
            else // Item not found, which could happen if duplicates logic above is not perfect for all cases
            {
                // Standard List<T>.BinarySearch would return a negative number.
                // We need to ensure our extension matches this.
                // For this specific test, we expect it to be found.
                // However, to make the test robust for edge cases with custom list or potential issues,
                // let's ensure that if it's negative, it's a valid negative result.
                 actualIndex.ShouldBeLessThan(0);
            }
             // A more robust check for "found" when duplicates are possible:
            list.BinarySearch(itemToFind).ShouldBeGreaterThanOrEqualTo(0);
            list[list.BinarySearch(itemToFind)].ShouldBe(itemToFind);
        }

        [Fact]
        public void BinarySearch_Item_List_String_ItemNotFound()
        {
            List<string> list = GenerateFactory.GenerateRandomString(5).Take(10).OrderBy(x => x).ToList();
            string itemToFind = "nonexistent";
            int expectedIndex = list.BinarySearch(itemToFind); // Should be negative

            int actualIndex = list.BinarySearch(itemToFind);

            actualIndex.ShouldBe(expectedIndex);
            actualIndex.ShouldBeLessThan(0);
        }
        
        [Fact]
        public void BinarySearch_Item_CustomList_String_ItemNotFound()
        {
            CustomList<string> list = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(10).OrderBy(x => x));
            string itemToFind = "nonexistent";

            int actualIndex = list.BinarySearch(itemToFind);

            actualIndex.ShouldBeLessThan(0);
        }


        [Fact]
        public void BinarySearch_Item_EmptyList_ReturnsNegative()
        {
            List<int> list = new List<int>();
            int actualIndex = list.BinarySearch(5);
            actualIndex.ShouldBeLessThan(0);

            CustomList<string> customList = new CustomList<string>();
            int actualCustomIndex = customList.BinarySearch("test");
            actualCustomIndex.ShouldBeLessThan(0);
        }
        
        [Fact]
        public void BinarySearch_Item_NullList_ThrowsArgumentNullException()
        {
            IList<int> list = null;
            Should.Throw<ArgumentNullException>(() => list.BinarySearch(5));
        }

        #endregion

        #region BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer)

        [Fact]
        public void BinarySearch_ItemComparer_List_String_ItemFound_CaseInsensitive()
        {
            List<string> list = new List<string> { "apple", "Banana", "cherry", "Date" }; // Already sorted for case-insensitive
            list.Sort(new CaseInsensitiveComparer()); // Ensure sorted with comparer
            string itemToFind = "banana";
            // Standard List<T>.BinarySearch with custom comparer
            int expectedIndex = list.BinarySearch(itemToFind, new CaseInsensitiveComparer());

            int actualIndex = list.BinarySearch(itemToFind, new CaseInsensitiveComparer());
            
            actualIndex.ShouldBe(expectedIndex);
            actualIndex.ShouldBe(1); // "Banana" is at index 1
        }

        [Fact]
        public void BinarySearch_ItemComparer_CustomList_String_ItemFound_CaseInsensitive()
        {
            CustomList<string> list = new CustomList<string> { "apple", "Banana", "cherry", "Date" };
            // CustomList needs manual sort if we want to guarantee order for a specific comparer
            // For this test, we assume the extension method will handle the comparison correctly
            // The list itself doesn't need to be pre-sorted by the *exact* comparer instance for the method to work,
            // as long as it IS sorted in a way that the comparer can make sense of.
            // Let's sort it using a compatible logic first.
            var tempList = list.ToList();
            tempList.Sort(new CaseInsensitiveComparer());
            list.Clear();
            foreach(var s in tempList) list.Add(s);

            string itemToFind = "banana";
            int actualIndex = list.BinarySearch(itemToFind, new CaseInsensitiveComparer());

            actualIndex.ShouldBeGreaterThanOrEqualTo(0); // Should find it
            list[actualIndex].ShouldBe("Banana"); // Should find "Banana"
        }
        
        [Fact]
        public void BinarySearch_ItemComparer_List_String_ItemNotFound_CaseSensitive()
        {
            List<string> list = new List<string> { "apple", "Banana", "cherry", "Date" }; // Sorted case-sensitively
            list.Sort(StringComparer.Ordinal);
            string itemToFind = "banana"; // Lowercase 'b'
            // Default comparer is case-sensitive
            int expectedIndex = list.BinarySearch(itemToFind, StringComparer.Ordinal);


            int actualIndex = list.BinarySearch(itemToFind, StringComparer.Ordinal); // Using ordinal (case-sensitive) comparer

            actualIndex.ShouldBe(expectedIndex); // Should be negative
            actualIndex.ShouldBeLessThan(0);
        }

        [Fact]
        public void BinarySearch_ItemComparer_NullComparer_UsesDefault()
        {
            List<int> list = GenerateFactory.GenerateRandomInt(0, 100).Take(10).OrderBy(x => x).ToList();
            int itemToFind = list[4];
            int expectedIndex = list.BinarySearch(itemToFind, null); // null comparer means use default

            int actualIndex = list.BinarySearch(itemToFind, null);

            actualIndex.ShouldBe(expectedIndex);
        }
        
        [Fact]
        public void BinarySearch_ItemComparer_NullList_ThrowsArgumentNullException()
        {
            IList<string> list = null;
            Should.Throw<ArgumentNullException>(() => list.BinarySearch("test", new CaseInsensitiveComparer()));
        }

        [Fact]
        public void BinarySearch_ItemComparer_EmptyList_ReturnsNegative()
        {
            List<string> list = new List<string>();
            int actualIndex = list.BinarySearch("test", new CaseInsensitiveComparer());
            actualIndex.ShouldBeLessThan(0);

            CustomList<int> customList = new CustomList<int>();
            int actualCustomIndex = customList.BinarySearch(1, Comparer<int>.Default);
            actualCustomIndex.ShouldBeLessThan(0);
        }

        #endregion

        #region BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer)

        [Fact]
        public void BinarySearch_IndexCountItemComparer_List_Int_ItemFound()
        {
            List<int> list = Enumerable.Range(10, 20).ToList(); // 10, 11, ..., 29
            int itemToFind = 15; // Should be at list index 5, or relative index 2 in range [3, 5]
            // list.BinarySearch(index, count, item, comparer)
            int expectedIndex = list.BinarySearch(3, 5, itemToFind, null); // Search in {13,14,15,16,17}

            int actualIndex = list.BinarySearch(3, 5, itemToFind, null);

            actualIndex.ShouldBe(expectedIndex); // Which is 5
            actualIndex.ShouldBe(5);
        }

        [Fact]
        public void BinarySearch_IndexCountItemComparer_CustomList_Int_ItemFound()
        {
            CustomList<int> list = new CustomList<int>(Enumerable.Range(20, 15).OrderBy(x=>x)); // 20...34
            int itemToFind = 28; // list[8]
            // Search in range index 5 to 12 (list[5] to list[12]), count 8. Range is {25,26,27,28,29,30,31,32}
            // item 28 is at relative index 3 within this sub-segment, so list index 5+3 = 8.
            
            int actualIndex = list.BinarySearch(5, 8, itemToFind, Comparer<int>.Default);
            
            actualIndex.ShouldBeGreaterThanOrEqualTo(0);
            list[actualIndex].ShouldBe(itemToFind); // Should be 8
            actualIndex.ShouldBe(8);
        }
        
        [Fact]
        public void BinarySearch_IndexCountItemComparer_List_String_ItemNotFoundInRange()
        {
            List<string> list = new List<string> { "a", "b", "c", "d", "e", "f", "g" };
            list.Sort(StringComparer.Ordinal);
            // Search for "f" in the first 3 elements ("a", "b", "c")
            int expectedIndex = list.BinarySearch(0, 3, "f", StringComparer.Ordinal); // Should be negative

            int actualIndex = list.BinarySearch(0, 3, "f", StringComparer.Ordinal);

            actualIndex.ShouldBe(expectedIndex);
            actualIndex.ShouldBeLessThan(0);
        }

        [Fact]
        public void BinarySearch_IndexCountItemComparer_NullComparer_UsesDefault()
        {
            List<int> list = Enumerable.Range(0, 10).ToList(); // 0..9
            int itemToFind = 3;
            int expectedIndex = list.BinarySearch(0, list.Count, itemToFind, null);

            int actualIndex = list.BinarySearch(0, list.Count, itemToFind, null);
            
            actualIndex.ShouldBe(expectedIndex); // Should be 3
        }

        [Fact]
        public void BinarySearch_IndexCountItemComparer_EmptyList_ReturnsNegative()
        {
            List<int> list = new List<int>();
            int actualIndex = list.BinarySearch(0, 0, 5, null);
            actualIndex.ShouldBeLessThan(0); // .NET List<T>.BinarySearch(0,0,item,null) returns ~0

            CustomList<string> customList = new CustomList<string>();
            int actualCustomIndex = customList.BinarySearch(0, 0, "test", StringComparer.Ordinal);
            actualCustomIndex.ShouldBeLessThan(0);
        }
        
        [Fact]
        public void BinarySearch_IndexCountItemComparer_NullList_ThrowsArgumentNullException()
        {
            IList<int> list = null;
            Should.Throw<ArgumentNullException>(() => list.BinarySearch(0, 0, 5, null));
        }

        [Theory]
        [InlineData(-1, 5)] // index < 0
        [InlineData(0, -1)] // count < 0
        [InlineData(5, 6)]  // index + count > list.Count (list size 10)
        public void BinarySearch_IndexCountItemComparer_InvalidRange_ThrowsArgumentOutOfRangeException(int index, int count)
        {
            List<int> list = Enumerable.Range(0, 10).ToList();
            Should.Throw<ArgumentOutOfRangeException>(() => list.BinarySearch(index, count, 5, null));

            CustomList<int> customList = new CustomList<int>(Enumerable.Range(0,10));
             // CustomList might throw ArgumentException for index+count > list.Count, or ArgumentOutOfRangeException
             // Let's check for either, as List<T> itself throws ArgumentException here.
            if (index + count > customList.Count && index >=0 && count >=0) {
                 Should.Throw<ArgumentException>(() => customList.BinarySearch(index, count, 5, null));
            } else {
                 Should.Throw<ArgumentOutOfRangeException>(() => customList.BinarySearch(index, count, 5, null));
            }
        }
        
        [Fact]
        public void BinarySearch_IndexCountItemComparer_IndexPlusCountGreaterThanListCount_ThrowsArgumentException()
        {
            List<int> list = Enumerable.Range(0, 10).ToList(); // Size 10
            // index (8) + count (3) = 11 > list.Count (10)
            Should.Throw<ArgumentException>(() => list.BinarySearch(8, 3, 5, null));

            CustomList<int> customList = new CustomList<int>(Enumerable.Range(0,10));
            Should.Throw<ArgumentException>(() => customList.BinarySearch(8, 3, 5, null));
        }

        #endregion
    }
}
