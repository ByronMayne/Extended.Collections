using Extended.Collections.Generic.Specialized;
using Xunit;

namespace Extended.Collections.Tests.Generic.Specialized
{
    public class OrderedDictionaryTests
    {
        [Fact]
        public void AddItem_IncreasesCount()
        {
            Compose(
                        assertCount: () => 1,
                        mutations: new[]
                        {
                    Add("Key", "Value")
                        });
        }

        [Fact]
        public void AddItem_ThatAlreadyExists_ThrowsException()
        {
            Compose(
                        expectedException: typeof(ArgumentException),
                        mutations: new[]
                        {
                    Add("Key", "Value"),
                    Add("Key", "Value2")
                        });
        }

        [Fact]
        public void SetItem_ThatAlreadyExists_ReplacesValue()
        {
            Compose(
                           assertCount: () => 1,
                           assertValue: () => new("Key", "OtherValue"),
                           mutations: new[]
                           {
                       Set("Key", "Value"),
                       Set("Key", "OtherValue")
                           });
        }

        [Fact]
        public void SetItem_Value_IsAssignedFirstIndex()
        {
            Compose(
                        assertValueAt: () => new(0, "Value"),
                        mutations: new[]
                        {
                    Set("Key", "Value")
                        });
        }

        [Fact]
        public void AddMultiple_ValuesAndKeys_ReturnInInserationOrder()
        {
            Compose(
                              assertValueOrder: () => new[] { 1, 2, 3, 4 },
                              assertKeyOrder: () => new[] { 1, 2, 3, 4 },
                              mutations: new[]
                              {
                        Add(1, 1),
                        Add(2, 2),
                        Add(3, 3),
                        Add(4, 4),
                              });
        }

        [Fact]
        public void SetValueAtIndex_NotInRange_ThrowsArgumentOutOfRangeException()
        {
            Compose<string, string>(
                            expectedException: typeof(ArgumentOutOfRangeException),
                            mutations: new[]
                            {
                        Set<string, string>(10, "value")
                            });
        }

        [Fact]
        public void AddItem_ThenSetFirstIndex_OverridesValue()
        {
            Compose(
                            assertValueAt: () => (0, "Super"),
                            mutations: new[]
                            {
                        Add("Base", "Base"),
                        Set<string, string>(0, "Super"),
                            });
        }

        [Fact]
        public void IsReadOnly_IsFalse()
        {
            Assert.False(new OrderedDictionary<string, string>().IsReadOnly);
        }

        [Fact]
        public void IsFixedSize_IsFalse()
        {
            Assert.False(new OrderedDictionary<string, string>().IsFixedSize);
        }

        [Fact]
        public void ItemsAdded_ReturnBackAsEnumerator()
        {
            OrderedDictionary<int, int> subject = new()
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 }
            };
            int index = 1;
            foreach (KeyValuePair<int, int> pair in subject)
            {
                Assert.Equal(index, pair.Key);
                Assert.Equal(index, pair.Value);
                index++;
            }
        }

        [Fact]
        public void Remove_UpdatesCollection()
        {
            OrderedDictionary<string, string> subject = new()
            {
                { "A", "A" },
                { "B", "B" }
            };

            Assert.True(subject.Remove("A"));
            Assert.Equal("B", subject[0]);
            _ = Assert.Single(subject);
        }

        [Fact]
        public void RemoveAt_FirstOfTwo_AdjustCollection()
        {
            OrderedDictionary<string, string> subject = new()
            {
                { "A", "A" },
                { "B", "B" }
            };

            subject.RemoveAt(0);
            Assert.Equal("B", subject[0]);
            _ = Assert.Single(subject);
        }

        [Fact]
        public void Insert_InMiddleOfTwo_AdjustsCollection()
        {
            OrderedDictionary<int, int> subject = new()
            {
                { 1, 1 },
                { 3, 3 }
            };

            subject.Insert(1, new KeyValuePair<int, int>(2, 2));
            Assert.Equal(2, subject[1]);
        }

        [Fact]
        public void TryGetValue_ReturnsTrue_IfValueExists()
        {
            OrderedDictionary<int, int> subject = new()
            {
                { 1, 101 },
            };
            Assert.True(subject.TryGetValue(1, out int value));
            Assert.Equal(101, value);
        }

        [Fact]
        public void TryGetValue_ReturnsFalse_IfValueDoesNotExist()
        {
            OrderedDictionary<int, int> subject = new();
            Assert.False(subject.TryGetValue(1, out _));
        }

        [Fact]
        public void CLear_RemovesAllValues()
        {
            OrderedDictionary<string, string> subject = new()
            { {"A", "A" }, {"B", "B" } };
            subject.Clear();
            Assert.Empty(subject);
            Assert.Equal(0, subject.Count);
        }

        private void Compose<TKey, TValue>(
           Func<int>? assertCount = null,
           Type? expectedException = null,
           Func<IEnumerable<TKey>>? assertKeyOrder = null,
           Func<IEnumerable<TValue>>? assertValueOrder = null,
           Func<(int Index, TValue Value)>? assertValueAt = null,
           Func<(int Index, TKey Key)>? assertKeyAt = null,
           Func<(TKey Key, TValue Value)>? assertValue = null,
           params Action<OrderedDictionary<TKey, TValue>>[] mutations)
        {
            OrderedDictionary<TKey, TValue>? subject = null;

            try
            {
                subject = new OrderedDictionary<TKey, TValue>();

                foreach (Action<OrderedDictionary<TKey, TValue>> mustation in mutations)
                {
                    mustation(subject);
                }
            }
            catch (Exception exception)
            {
                if (expectedException != null)
                {
                    Assert.IsType(expectedException, exception);
                    expectedException = null;
                }
                else
                {
                    Assert.Fail($"Unexpected exception of type {exception.GetType().FullName} was thrown.");
                    return;
                }
            }

            if (expectedException != null)
            {
                Assert.Fail($"The code did not throw the expected exception {expectedException.FullName}");
            }

            if (subject != null)
            {
                if (assertCount != null)
                {
                    Assert.Equal(assertCount(), subject.Count);
                }

                if (assertValue != null)
                {
                    (TKey Key, TValue Value) = assertValue();
                    Assert.Equal(Value, subject[Key]);
                }
                if (assertValueOrder != null)
                {
                    Assert.Equal(assertValueOrder(), subject.Values);
                }

                if (assertKeyOrder != null)
                {
                    Assert.Equal(assertKeyOrder(), subject.Keys);
                }

                if (assertKeyAt != null)
                {
                    (int Index, TKey Key) pair = assertKeyAt();
                    Assert.Equal(pair.Key, subject.Keys.ToArray()[pair.Index]);
                }
                if (assertValueAt != null)
                {
                    (int Index, TValue Value) pair = assertValueAt();
                    Assert.Equal(pair.Value, subject[pair.Index]);

                }
            }
        }

        private Action<OrderedDictionary<TKey, TValue>> Add<TKey, TValue>(TKey key, TValue value)
        {
            return target => target.Add(key, value);
        }

        private Action<OrderedDictionary<TKey, TValue>> Set<TKey, TValue>(TKey key, TValue value)
        {
            return target => target[key] = value;
        }

        private Action<OrderedDictionary<TKey, TValue>> Set<TKey, TValue>(int index, TValue value)
        {
            return target => target[index] = value;
        }
    }

}
