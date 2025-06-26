using Extended.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Extended.Collections.Tests.Generic
{
    public static class CollectionMutations
    {
        public static Action<TCollection> AssertContains<TCollection, TValue>(TValue value) where TCollection : ICollection<TValue>
        {
            return collection =>
            {
                Assert.Contains(value, collection);
            };
        }

    }

    public class RingBufferTests : BaseTest
    {
        public RingBufferTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Capacity_MatchesConstructorValue()
            => Compose<string>(
                    capacity: 10,
                    assertCapacity: () => 10);

        [Fact]
        public void Index_With_Empty_Collection_ThrowsIndexOutOfRangeException()
        {
            RingBuffer<int> buffer = new RingBuffer<int>(10);
            Assert.Throws<IndexOutOfRangeException>(() => buffer[0]);
        }

        [Theory]
        [InlineData(15, 2, 2)]
        [InlineData(15, 3, 3)]
        [InlineData(15, -5, 5)]
        [InlineData(15, -1, 9)]
        [InlineData(10, 0, 0)]
        [InlineData(2, -1, 9)]
        public void Index_Returns_Expected(int size, int index, int expected)
        {
            RingBuffer<int> buffer = new RingBuffer<int>(size);
            buffer.AddRange(Enumerable.Range(0, 10));
            Assert.Equal(expected, buffer[index]);
        }

        [Fact]
        public void AddItem_IncreaseCount()
            => Compose(
                assertCount: () => 3,
                mutations: new[]
                {
                    Add("Item One"),
                    AddItems("Item Two", "Item Three"),
                });

        [Fact]
        public void AddItem_ThenClear_ResetsCount()
            => Compose(
                assertCount: () => 0,
                mutations: new[]
                {
                    Add("Item"),
                    Clear<string>()
                });

        [Fact]
        public void AddItems_ReturnInAddedOrder()
            => Compose(
                    capacity: 10,
                    assertOrder: () => new[] { 1, 2, 3 },
                    mutations: new[]
                    {
                        AddItems(1),
                        AddItems(2),
                        AddItems(3),
                    }
                );

        [Fact]
        public void AddSingleItem_BeyondCapacity_FirstItemIsReplaced()
            => Compose(
                    capacity: 3,
                    assertOrder: () => new[] { 2, 3, 4 },
                    mutations: new[]
                    {
                        AddItems(1, 2, 3, 4)
                    });

        [Fact]
        public void AddMultipleItems_BeyondCapacity_OveridesValues()
            => Compose(
                    capacity: 2,
                    assertOrder: () => new[] { 5, 6 },
                    mutations: new[]
                    {
                        AddItems(1, 2, 3, 4, 5, 6)
                    });

        [Fact]
        public void AddItem_ThenRemove_SetsCountToZero()
            => Compose(
                capacity: 2,
                assertCount: () => 0,
                mutations: new[]
                {
                    Add(1),
                    Remove(1),
                });

        [Fact]
        public void AddItems_RemoveMiddle_ShiftsValues()
            => Compose(
                    capacity: 3,
                    assertOrder: () => new[] { 1, 3 },
                    mutations: new[]
                    {
                        AddItems(1, 2, 3),
                        Remove(2)
                    });

        [Fact]
        public void Remove_ThatDoesNotExist_ReturnsFalse()
            => Compose(
                mutations: new[]
                {
                    AssertRemove("DoesNotExist", false),
                });

        [Fact]
        public void Remove_ThatDoesExist_ReturnsTrue()
            => Compose(
                mutations: new[]
                {
                    Add("Exist"),
                    AssertRemove("Exist", true)
                });

        [Fact]
        public void Contains_ItemThatExists_ShouldReturnTrue()
            => Compose(
                assertContains: () => 10,
                mutations: new[]
                {
                    Add(10),
                });

        [Fact]
        public void Contains_ItemThatDoesNotExist_ShouldReturnFalse()
            => Compose(
                assertDoesNotContain: () => 10);


        private void Compose<T>(
                int capacity = 10,
                Func<int>? assertCount = null,
                Func<int>? assertCapacity = null,
                Func<T>? assertContains = null,
                Func<T>? assertDoesNotContain = null,
                Func<IEnumerable<T>>? assertOrder = null,
                params Action<RingBuffer<T>>[] mutations)
        {
            // Create
            RingBuffer<T> subject = new RingBuffer<T>(capacity);

            // Mutate 
            foreach (Action<RingBuffer<T>> mutation in mutations)
            {
                mutation(subject);
            }

            // Log 
            LogInfo("Expected Values:");
            if (assertCount != null) LogInfo($" Count: {assertCount()}");
            if (assertCapacity != null) LogInfo($" Capacity: {assertCapacity()}");
            if (assertOrder != null) LogInfo($" Order: {string.Join(", ", assertOrder())}");

            // Assert
            if (assertCount != null) Assert.Equal(assertCount(), subject.Count);
            if (assertCapacity != null) Assert.Equal(assertCapacity(), subject.Capacity);
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection 
            // Disabled asserts because we are testing this spesefic funciton 
            if (assertContains != null) Assert.True(subject.Contains(assertContains()));
            if (assertDoesNotContain != null) Assert.False(subject.Contains(assertDoesNotContain()));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection
            if (assertOrder != null)
            {
                T[] items = subject.ToArray();
                Assert.Equal(assertOrder(), items);
            }
        }


        // Helpers 

        private Action<RingBuffer<T>> Clear<T>()
        {
            return (RingBuffer<T> buffer) => buffer.Clear();
        }

        private Action<RingBuffer<T>> Add<T>(T item)
        {
            return (RingBuffer<T> buffer) => buffer.Add(item);
        }

        private Action<RingBuffer<T>> AssertRemove<T>(T item, bool expectedReturn)
        {
            return (RingBuffer<T> buffer) => Assert.Equal(expectedReturn, buffer.Remove(item));
        }

        private Action<RingBuffer<T>> AddItems<T>(params T[] items)
        {
            return (RingBuffer<T> buffer) =>
            {
                foreach (T item in items)
                {
                    buffer.Add(item);
                }
            };
        }

        public Action<RingBuffer<T>> Remove<T>(T item)
        {
            return (RingBuffer<T> buffer) =>
            {
                buffer.Remove(item);
            };
        }

        public Action<RingBuffer<T>> RemoveItems<T>(params T[] items)
        {
            return (RingBuffer<T> buffer) =>
            {
                foreach (T item in items)
                {
                    buffer.Remove(item);
                }
            };
        }
    }

}
