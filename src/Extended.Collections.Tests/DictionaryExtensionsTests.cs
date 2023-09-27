using Extended.Collections.Exceptions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Extended.Collections.Tests
{
    public class DictionaryExtensionsTests : BaseTest
    {

        public DictionaryExtensionsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        { }

        [Theory]
        [InlineData(MergeMethod.KeepLast)]
        [InlineData(MergeMethod.KeepFirst)]
        public void Merge_Single_KeepsExpectedValue(MergeMethod mergeMethod)
        {
            Dictionary<string, MergeMethod> source = new() { ["Key"] = MergeMethod.KeepFirst };
            Dictionary<string, MergeMethod> target = new() { ["Key"] = MergeMethod.KeepLast };

            source.Merge(mergeMethod, target)
                .Should()
                .HaveCount(1)
                .And.Contain("Key", mergeMethod);
        }

        [Theory]
        [InlineData(MergeMethod.KeepLast)]
        [InlineData(MergeMethod.KeepFirst)]
        public void Merge_Multiple_KeepsExpectedValue(MergeMethod mergeMethod)
        {
            Dictionary<string, string> source = Create("Key", "First");
            Dictionary<string, string>[] targets = new[] {
                Create("Key", "Second"),
                Create("Key", "Third"),
            };

            source.Merge(mergeMethod, targets)
                .Should()
                .HaveCount(1)
                .And.Contain("Key", mergeMethod == MergeMethod.KeepFirst ? "First" : "Third");
        }

        [Fact]
        public void Merge_WithDuplicateKeysAndThrowMethod_ThrowsDuplicateKeyException()
        {
            Dictionary<string, string> source = Create("key", "First");

            Assert.Throws<DuplicateKeyException>(
                () => source.Merge(MergeMethod.Throw, source));
        }


        private Dictionary<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) where TKey : notnull
        {
            return new Dictionary<TKey, TValue>
            {
                [key] = value
            };
        }
    }
}
