using System;

namespace Example.IDisposable_Using;

file sealed class DemoResource : IDisposable
{
    private bool _disposed;
    public void Use() { if (_disposed) throw new ObjectDisposedException(nameof(DemoResource)); Console.WriteLine("Работаем с ресурсом"); }
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Console.WriteLine("Ресурс освобождён");
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== using и try/finally ==");
        using (var r = new DemoResource())
        {
            r.Use();
        }
        Console.WriteLine("Конструкция using разворачивается в try/finally с вызовом Dispose()");
    }
}
