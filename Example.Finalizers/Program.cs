using System;

namespace Example.Finalizers;

file sealed class Finalizable
{
    private readonly int _id;
    public Finalizable(int id) => _id = id;
    ~Finalizable() => Console.WriteLine($"Финализирован объект #{_id}");
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Финализаторы ==");
        for (var i = 0; i < 20; i++) _ = new Finalizable(i);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Финализаторы вызваны (порядок недетерминирован).");
    }
}
