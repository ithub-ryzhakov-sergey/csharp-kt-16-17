using System;

namespace Example.AddMemoryPressure;

file sealed class UnmanagedLike : IDisposable
{
    private readonly long _bytes;
    private bool _disposed;
    public UnmanagedLike(long bytes)
    {
        if (bytes <= 0) throw new ArgumentOutOfRangeException(nameof(bytes));
        _bytes = bytes;
        GC.AddMemoryPressure(_bytes);
        Console.WriteLine($"Добавили давление памяти: {_bytes} байт");
    }
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        GC.RemoveMemoryPressure(_bytes);
        Console.WriteLine($"Сняли давление памяти: {_bytes} байт");
        GC.SuppressFinalize(this);
    }
    ~UnmanagedLike() { if (!_disposed) GC.RemoveMemoryPressure(_bytes); }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Add/RemoveMemoryPressure ==");
        using var r = new UnmanagedLike(100_000_000);
        Console.WriteLine("Эффект на частоту GC не гарантирован — это корректный паттерн уведомления CLR");
    }
}
