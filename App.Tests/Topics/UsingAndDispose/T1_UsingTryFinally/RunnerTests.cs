using System;
using System.Collections.Generic;
using App.Topics.UsingAndDispose.T1_UsingTryFinally;
using Xunit;

namespace App.Tests.Topics.UsingAndDispose.T1_UsingTryFinally;

file sealed class ListLog : ILog
{
    public List<string> Events { get; } = new();
    public void Write(string message) => Events.Add(message);
}

public class RunnerTests
{
    [Fact]
    public void Using_and_TryFinally_produce_same_order_without_exception()
    {
        var log1 = new ListLog();
        var log2 = new ListLog();

        UsingTryFinallyRunner.RunWithUsing(log1);
        UsingTryFinallyRunner.RunWithTryFinally(log2);
        Assert.Equal(new[] { LogEvents.Open, LogEvents.Work, LogEvents.Dispose }, log1.Events);
        Assert.Equal(new[] { LogEvents.Open, LogEvents.Work, LogEvents.Dispose }, log2.Events);
    }

    [Fact]
    public void Using_calls_Dispose_even_if_work_throws()
    {
        var log = new ListLog();
        Assert.ThrowsAny<Exception>(() => UsingTryFinallyRunner.RunWithUsing(log, throwInWork: true));
        // При реализации ожидается, что последний элемент — DISPOSE
        // Здесь тест падал бы до реализации; оставляем проверку ожидаемого результата
        Assert.Equal(LogEvents.Dispose, log.Events[^1]);
    }

    [Fact]
    public void SampleResource_Dispose_is_idempotent()
    {
        var r = new SampleResource();
        r.Open();
        r.DoWork();
        r.Dispose();
        r.Dispose(); // второй вызов не должен падать
        Assert.Throws<ObjectDisposedException>(() => r.Open());
        Assert.Throws<ObjectDisposedException>(() => r.DoWork());
    }
}
