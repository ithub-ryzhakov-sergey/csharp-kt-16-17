using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Topics.Lazy.T3_LazyCache;
using Xunit;

namespace App.Tests.Topics.Lazy.T3_LazyCache;

public class LazyCacheTests
{
    [Fact]
    public async Task Same_key_initialized_once_concurrently()
    {
        var cache = new LazyCache<string, int>();
        var calls = 0;
        var tasks = Enumerable.Range(0, 20)
            .Select(_ => Task.Run(() => cache.GetOrAdd("k", () => ++calls)))
            .ToArray();
        var results = await Task.WhenAll(tasks);

        Assert.All(results, v => Assert.Equal(1, v));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void Different_keys_initialized_separately()
    {
        var cache = new LazyCache<string, int>();
        var calls = 0;
        var a = cache.GetOrAdd("a", () => ++calls);
        var b = cache.GetOrAdd("b", () => ++calls);
        Assert.NotEqual(a, b);
        Assert.Equal(2, calls);
    }

    [Fact]
    public void Exception_is_cached_by_default()
    {
        var cache = new LazyCache<string, int>();
        var ex1 = Assert.Throws<InvalidOperationException>(() => cache.GetOrAdd("e", () => throw new InvalidOperationException("boom")));
        var ex2 = Assert.Throws<InvalidOperationException>(() => cache.GetOrAdd("e", () => 42));
        Assert.Equal("boom", ex1.Message);
        Assert.Equal("boom", ex2.Message);
    }
}
