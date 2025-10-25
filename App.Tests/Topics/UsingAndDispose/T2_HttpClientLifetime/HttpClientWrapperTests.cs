using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Topics.UsingAndDispose.T2_HttpClientLifetime;
using Xunit;

namespace App.Tests.Topics.UsingAndDispose.T2_HttpClientLifetime;

public class HttpClientWrapperTests
{
    [Fact]
    public async Task Should_use_external_handler_and_not_dispose_it()
    {
        var handler = new StubHandler { ResponseContent = "A" };
        var w = new HttpClientWrapper(handler);
        var s1 = await w.GetStringAsync(new Uri("http://example.org"));
        var s2 = await w.GetStringAsync(new Uri("http://example.org"));
        Assert.Equal("A", s1);
        Assert.Equal("A", s2);
        Assert.Equal(2, handler.Calls);
        w.Dispose();
        Assert.False(handler.Disposed);
    }

    [Fact]
    public void Should_throw_after_dispose()
    {
        var handler = new StubHandler();
        var w = new HttpClientWrapper(handler);
        w.Dispose();
        Assert.Throws<ObjectDisposedException>(() => w.GetStringAsync(new Uri("http://example.org")).GetAwaiter().GetResult());
    }

    [Fact]
    public void Should_apply_timeout_property()
    {
        var handler = new StubHandler();
        var w = new HttpClientWrapper(handler);
        var ts = TimeSpan.FromSeconds(5);
        w.Timeout = ts;
        Assert.Equal(ts, w.Timeout);
    }

    [Fact]
    public async Task Should_support_cancellation()
    {
        var handler = new StubHandler();
        var w = new HttpClientWrapper(handler);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => w.GetStringAsync(new Uri("http://example.org"), cts.Token));
    }

    [Fact]
    public async Task Should_reuse_client_for_multiple_requests()
    {
        var handler = new StubHandler { ResponseContent = "OK" };
        var w = new HttpClientWrapper(handler);
        for (int i = 0; i < 10; i++)
        {
            var s = await w.GetStringAsync(new Uri("http://example.org"));
            Assert.Equal("OK", s);
        }
        Assert.Equal(10, handler.Calls);
    }
}
