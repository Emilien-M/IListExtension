using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IListExtension.Tests.Factories;
using IListExtension.Tests.List;
using Shouldly;
using Xunit;

namespace IListExtension.Tests.Tests
{
    public class ForEachTest
    {
        private GenerateFactory GenerateFactory { get; }

        public ForEachTest()
        {
            GenerateFactory = new GenerateFactory();
        }

        private class TestObject
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public bool Processed { get; set; }
            public string AsyncResult { get; set; }
        }

        #region Synchronous ForEach<T>(this IList<T> source, Action<T> action)

        [Fact]
        public void ForEach_Sync_List_ActionPerformsOnEachElement()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1, Value = "A" },
                new TestObject { Id = 2, Value = "B" },
                new TestObject { Id = 3, Value = "C" }
            };
            Action<TestObject> action = obj => obj.Processed = true;

            // Act
            sourceList.ForEach(action);

            // Assert
            sourceList.ShouldAllBe(obj => obj.Processed);
        }

        [Fact]
        public void ForEach_Sync_CustomList_ActionPerformsOnEachElement()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>
            {
                new TestObject { Id = 1, Value = "A" },
                new TestObject { Id = 2, Value = "B" }
            };
            Action<TestObject> action = obj => obj.Value += "_processed";

            // Act
            sourceList.ForEach(action);

            // Assert
            sourceList[0].Value.ShouldBe("A_processed");
            sourceList[1].Value.ShouldBe("B_processed");
        }

        [Fact]
        public void ForEach_Sync_List_ActionAddsToExternalCollection()
        {
            // Arrange
            List<int> sourceList = GenerateFactory.GenerateRandomInt(1, 100).Take(5).ToList();
            List<int> targetList = new List<int>();
            Action<int> action = item => targetList.Add(item * 2);

            // Act
            sourceList.ForEach(action);

            // Assert
            targetList.Count.ShouldBe(sourceList.Count);
            for (int i = 0; i < sourceList.Count; i++)
            {
                targetList[i].ShouldBe(sourceList[i] * 2);
            }
        }
        
        [Fact]
        public void ForEach_Sync_CustomList_ActionAddsToExternalCollection()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string>(GenerateFactory.GenerateRandomString(5).Take(3));
            List<string> targetList = new List<string>();
            Action<string> action = item => targetList.Add(item.ToUpper());

            // Act
            sourceList.ForEach(action);

            // Assert
            targetList.Count.ShouldBe(sourceList.Count);
            for (int i = 0; i < sourceList.Count; i++)
            {
                targetList[i].ShouldBe(sourceList[i].ToUpper());
            }
        }

        [Fact]
        public void ForEach_Sync_List_ActionThrowsException_ExceptionPropagates()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Action<int> action = item =>
            {
                if (item == 2) throw new InvalidOperationException("Test exception");
            };

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => sourceList.ForEach(action))
                  .Message.ShouldBe("Test exception");
        }

        [Fact]
        public void ForEach_Sync_CustomList_ActionThrowsException_ExceptionPropagates()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b", "c" };
            Action<string> action = item =>
            {
                if (item == "b") throw new ArgumentException("Bad argument");
                item.ToString(); // Dummy operation
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => sourceList.ForEach(action))
                .Message.ShouldBe("Bad argument");
        }


        [Fact]
        public void ForEach_Sync_EmptyList_ActionNotCalled()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>();
            bool actionCalled = false;
            Action<TestObject> action = obj => actionCalled = true;

            // Act
            sourceList.ForEach(action);

            // Assert
            actionCalled.ShouldBeFalse();
        }
        
        [Fact]
        public void ForEach_Sync_EmptyCustomList_ActionNotCalled()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>();
            int callCount = 0;
            Action<int> action = item => callCount++;

            // Act
            sourceList.ForEach(action);

            // Assert
            callCount.ShouldBe(0);
        }

        [Fact]
        public void ForEach_Sync_NullList_ReturnsSilently()
        {
            // Arrange
            IList<int> sourceList = null;
            Action<int> action = item => item.ToString(); // Dummy action

            // Act & Assert
            Should.NotThrow(() => sourceList.ForEach(action));
        }

        [Fact]
        public void ForEach_Sync_NullAction_ReturnsSilently()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Action<int> action = null;

            // Act & Assert
            Should.NotThrow(() => sourceList.ForEach(action));
        }
        
        [Fact]
        public void ForEach_Sync_CustomList_NullAction_ReturnsSilently()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b" };
            Action<string> action = null;

            // Act & Assert
            Should.NotThrow(() => sourceList.ForEach(action));
        }

        #endregion

        #region Asynchronous ForEach<T>(this IList<T> source, Func<T, Task> func)

        [Fact]
        public async Task ForEach_Async_List_FuncAwaitedForEachElement()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>
            {
                new TestObject { Id = 1 }, new TestObject { Id = 2 }, new TestObject { Id = 3 }
            };
            Func<TestObject, Task> func = async obj =>
            {
                await Task.Delay(10); // Simulate async work
                obj.AsyncResult = $"Processed {obj.Id}";
            };

            // Act
            await sourceList.ForEach(func);

            // Assert
            sourceList[0].AsyncResult.ShouldBe("Processed 1");
            sourceList[1].AsyncResult.ShouldBe("Processed 2");
            sourceList[2].AsyncResult.ShouldBe("Processed 3");
        }

        [Fact]
        public async Task ForEach_Async_CustomList_FuncAwaitedForEachElement()
        {
            // Arrange
            CustomList<TestObject> sourceList = new CustomList<TestObject>
            {
                new TestObject { Id = 10 }, new TestObject { Id = 20 }
            };
            var processedIds = new List<int>();
            Func<TestObject, Task> func = async obj =>
            {
                await Task.Delay(5);
                obj.Processed = true;
                lock(processedIds) { processedIds.Add(obj.Id); }
            };

            // Act
            await sourceList.ForEach(func);

            // Assert
            sourceList.ShouldAllBe(obj => obj.Processed);
            processedIds.ShouldBe(new List<int> { 10, 20 }, ignoreOrder: true); // Order might vary due to async
        }

        [Fact]
        public async Task ForEach_Async_List_FuncThrowsException_ExceptionPropagatesAndTaskFaults()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Func<int, Task> func = async item =>
            {
                await Task.Delay(1);
                if (item == 2) throw new InvalidOperationException("Async test exception");
            };

            // Act
            Task resultTask = sourceList.ForEach(func);

            // Assert
            var ex = await Should.ThrowAsync<InvalidOperationException>(resultTask);
            ex.Message.ShouldBe("Async test exception");
            resultTask.IsFaulted.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ForEach_Async_CustomList_FuncThrowsException_ExceptionPropagatesAndTaskFaults()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "x", "y", "z" };
            Func<string, Task> func = async item =>
            {
                await Task.Yield(); // Ensure it's async
                if (item == "y") throw new FormatException("Bad format async");
            };

            // Act
            Task resultTask = sourceList.ForEach(func);

            // Assert
            var ex = await Should.ThrowAsync<FormatException>(resultTask);
            ex.Message.ShouldBe("Bad format async");
            resultTask.IsFaulted.ShouldBeTrue();
        }


        [Fact]
        public async Task ForEach_Async_EmptyList_FuncNotCalled_ReturnsCompletedTask()
        {
            // Arrange
            List<TestObject> sourceList = new List<TestObject>();
            bool funcCalled = false;
            Func<TestObject, Task> func = async obj =>
            {
                funcCalled = true;
                await Task.Delay(1);
            };

            // Act
            Task resultTask = sourceList.ForEach(func);
            await resultTask; // Await to ensure completion

            // Assert
            funcCalled.ShouldBeFalse();
            resultTask.IsCompletedSuccessfully.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ForEach_Async_EmptyCustomList_FuncNotCalled_ReturnsCompletedTask()
        {
            // Arrange
            CustomList<int> sourceList = new CustomList<int>();
            int callCount = 0;
            Func<int, Task> func = async item => { callCount++; await Task.CompletedTask; };

            // Act
            Task resultTask = sourceList.ForEach(func);
            await resultTask;

            // Assert
            callCount.ShouldBe(0);
            resultTask.IsCompletedSuccessfully.ShouldBeTrue();
        }


        [Fact]
        public async Task ForEach_Async_NullList_ReturnsCompletedTaskSilently()
        {
            // Arrange
            IList<int> sourceList = null;
            Func<int, Task> func = async item => await Task.Delay(1);

            // Act
            Task resultTask = sourceList.ForEach(func);
            await resultTask; // Await to ensure completion

            // Assert
            resultTask.IsCompletedSuccessfully.ShouldBeTrue(); // Should not throw
        }

        [Fact]
        public async Task ForEach_Async_NullFunc_ReturnsCompletedTaskSilently()
        {
            // Arrange
            List<int> sourceList = new List<int> { 1, 2, 3 };
            Func<int, Task> func = null;

            // Act
            Task resultTask = sourceList.ForEach(func);
            await resultTask; // Await to ensure completion

            // Assert
            resultTask.IsCompletedSuccessfully.ShouldBeTrue(); // Should not throw
        }
        
        [Fact]
        public async Task ForEach_Async_CustomList_NullFunc_ReturnsCompletedTaskSilently()
        {
            // Arrange
            CustomList<string> sourceList = new CustomList<string> { "a", "b" };
            Func<string, Task> func = null;

            // Act
            Task resultTask = sourceList.ForEach(func);
            await resultTask;

            // Assert
            resultTask.IsCompletedSuccessfully.ShouldBeTrue();
        }

        #endregion
    }
}
