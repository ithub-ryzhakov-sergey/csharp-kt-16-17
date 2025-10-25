using System;

namespace Example.GC_API;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Обзор полезных API GC ==");
        Console.WriteLine($"Память (прибл.): {GC.GetTotalMemory(false)}");
        var o = new object();
        Console.WriteLine($"Поколение объекта: {GC.GetGeneration(o)}");

        GC.Collect(0, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();
        GC.KeepAlive(o); // предотвращает сборку o до этой строки

        Console.WriteLine("Избегайте ручного Collect в продакшене — это учебная демонстрация.");
    }
}
