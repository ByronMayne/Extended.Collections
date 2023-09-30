using Extended.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Extended.Collections.Tests.Generic
{
    public class DequeTests : BaseTest
    {
        public DequeTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void PushFirst_IncreasesCount()
            => Compose<string>(
                    assertCount: () => 1,
                    mutations: new[]
                    {
                        PushFirst("One")
                    });
        [Fact]
        public void PushLast_IncreasesCount()
            => Compose<string>(
                    assertCount: () => 2,
                    mutations: new[]
                    {
                        PushLast("One"),
                        PushLast("Two"),
                    });

        [Fact]
        public void PushFirst_Peek_ReturnsExpected()
            => Compose<string>(
                    assertPeekFirst: () => "Item",
                    assertPeekLast: () => "Item",
                    mutations: new[]
                    {
                        PushFirst("Item")
                    });
        [Fact]
        public void PushLast_Peek_ReturnsExpected()
            => Compose<string>(
                    assertPeekFirst: () => "Item",
                    assertPeekLast: () => "Item",
                    mutations: new[]
                    {
                                PushFirst("Item")
                    });

        [Fact]
        public void Default_IsEmpty_IsTrue()
            => Compose<string>(
                assertIsEmpty: () => true);

        [Fact]
        public void PushFirst_BeyondHalfCapacity_DoublesCapacity()
            => Compose<string>(
                capacity: 2,
                assertCapacity: () => 8,
                mutations: new[]
                {
                    PushFirst("One"), // 2
                    PushFirst("Two"), // 2
                    PushFirst("Three"), // 4
                    PushFirst("Four"),  // 4
                    PushFirst("Five"), // 8
                });

        [Fact]
        public void PushFirst_PopFirst_ReturnsExpected()
            => Compose<string>(
                    mutations: new[]
                    {
                        PushFirst("Value"),
                        AssertPopFirst("Value")
                    });
        [Fact]
        public void PushFirst_PopLast_ReturnsExpected()
            => Compose<string>(
                    mutations: new[]
                    {
                        PushFirst("Value"),
                        AssertPopLast("Value")
                    });
        [Fact]
        public void PushLast_PopFirst_ReturnsExpected()
            => Compose<string>(
                    assertCount: () => 0,
                    mutations: new[]
                    {
                        PushLast("Value"),
                        AssertPopFirst("Value")
                    });

        [Fact]
        public void PushLast_PopLast_ReturnsExpected()
            => Compose<string>(
                    assertCount: () => 0,
                    mutations: new[]
                    {
                        PushLast("Value"),
                        AssertPopLast("Value")
                    });

        [Fact]
        public void PushFirst_Enumerate_ReturnsFirstInLastOut()
            => Compose(
                    mutations: new[] { PushFirst(1, 2, 3, 4, 5) },
                    assertCollection: sub => sub
                        .HaveCount(5)
                        .And.BeInDescendingOrder());
        [Fact]
        public void PushFirst_Enumerate_ReturnsFirstInFirstOut()
            => Compose(
                mutations: new[] { PushLast(1, 2, 3, 4, 5) },
                assertCollection: col => col
                    .HaveCount(5)
                    .And.BeInDescendingOrder());

        [Fact]
        public void PushItems_ThenClear_RemovesItems()
            => Compose(
                    assertCount: () => 0,
                    mutations: new[]
                    {
                        PushFirst(1, 2, 3, 4),
                        Clear<int>()
                    });

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PushToOneSide_RebalancesTree_InsteadOfResizing(bool pushFirst)
        {
            int count = 15;
            Deque<int> subject = new Deque<int>();

            for (int i = 0; i < count; i++)
            {
                if (pushFirst)
                {
                    subject.PushFirst(i + 1);
                }
                else
                {
                    subject.PushLast(i + 1);
                }
            }
            // If we did not reblance the capacity would have to be 30 (15 on the left and 15 on the right)
            subject.Capacity.Should().Be(20);
            subject.Count.Should().Be(count);
        }

        private void Compose<T>(
                int capacity = 10,
                Func<int>? assertCount = null,
                Func<int>? assertCapacity = null,
                Func<T>? assertPeekFirst = null,
                Func<T>? assertPeekLast = null,
                Func<bool>? assertIsEmpty = null,
                Action<GenericCollectionAssertions<T>>? assertCollection = null,
                params Action<Deque<T>>[] mutations)
        {
            // Create
            Deque<T> subject = new Deque<T>(capacity);

            try
            {
                // Mutate 
                foreach (Action<Deque<T>> mutation in mutations)
                {
                    mutation(subject);
                }
            }
            finally
            {
                LogInfo("Value");
                LogInfo($" Count: {subject.Count}");
                LogInfo($" Capacity: {subject.Capacity}");
                LogInfo($" Items:");
                int i = 0;
                foreach (T item in subject)
                {
                    LogInfo($" {i}. {item}");
                    i++;
                }
            }

            // Assert
            if (assertCount != null) Assert.Equal(assertCount(), subject.Count);
            if (assertCapacity != null) Assert.Equal(assertCapacity(), subject.Capacity);
            if (assertPeekFirst != null) Assert.Equal(assertPeekFirst(), subject.PeekFirst());
            if (assertPeekLast != null) Assert.Equal(assertPeekLast(), subject.PeekLast());
            if (assertIsEmpty != null) Assert.Equal(assertIsEmpty(), subject.IsEmpty);
            if (assertCollection != null) assertCollection(subject.Should());
        }
        private Action<Deque<T>> Clear<T>()
            => (Deque<T> subject) => subject.Clear();

        private Action<Deque<T>> PushFirst<T>(T item)
            => (Deque<T> subject) => subject.PushFirst(item);

        private Action<Deque<T>> PushFirst<T>(params T[] items)
         => (Deque<T> subject) =>
         {
             foreach (T item in items)
             {
                 subject.PushFirst(item);
             }
         };

        private Action<Deque<T>> PushLast<T>(T item)
            => (Deque<T> subject) => subject.PushLast(item);

        private Action<Deque<T>> PushLast<T>(params T[] items)
            => (Deque<T> subject) =>
            {
                foreach (T item in items)
                {
                    subject.PushLast(item);
                }
            };

        private Action<Deque<T>> AssertPopLast<T>(T expected)
            => (Deque<T> subject) => subject.PopLast().Should().Be(expected);

        private Action<Deque<T>> AssertPopFirst<T>(T expected)
        => (Deque<T> subject) => subject.PopFirst().Should().Be(expected);
    }
}
