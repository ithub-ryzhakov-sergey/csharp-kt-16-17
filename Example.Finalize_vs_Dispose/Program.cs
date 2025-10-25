using System;

namespace Example.Finalize_vs_Dispose;

file class ResourceHolder : IDisposable
{
    private bool _disposed;
    ~ResourceHolder() => Console.WriteLine("Финализатор вызван (ресурсы очищены поздно)");
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Console.WriteLine("Dispose вызван — подавляем финализацию");
        GC.SuppressFinalize(this);
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Финализатор vs Dispose ==");
        using (var h = new ResourceHolder())
        {
            Console.WriteLine("Работа с ресурсом...");
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Финализатор подавлен, если Dispose был вызван");
    }
}
