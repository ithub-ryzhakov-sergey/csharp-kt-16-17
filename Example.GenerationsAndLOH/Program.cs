using System;

namespace Example.GenerationsAndLOH;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Поколения и LOH ==");
        Console.WriteLine($"Счётчики до: Gen0={GC.CollectionCount(0)}, Gen1={GC.CollectionCount(1)}, Gen2={GC.CollectionCount(2)}");

        for (var i = 0; i < 50_000; i++) _ = new object(); // шум аллокаций

        var loh = new byte[100_000]; // > 85_000 байт → LOH
        Console.WriteLine($"Поколение большого массива: {GC.GetGeneration(loh)} (обычно 2)");

        GC.Collect();
        Console.WriteLine($"Счётчики после: Gen0={GC.CollectionCount(0)}, Gen1={GC.CollectionCount(1)}, Gen2={GC.CollectionCount(2)}");
    }
}
