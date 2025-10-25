using System;
using System.Diagnostics;
using System.Runtime;

namespace Example.GC_LatencyModes;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Режимы латентности GC ==");
        Console.WriteLine($"ServerGC: {GCSettings.IsServerGC}");
        Console.WriteLine($"LatencyMode (старт): {GCSettings.LatencyMode}");

        var baseTime = MeasureAllocations();
        Console.WriteLine($"Базовое время: {baseTime} мс");

        var prev = GCSettings.LatencyMode;
        try
        {
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            var lowLatency = MeasureAllocations();
            Console.WriteLine($"SustainedLowLatency: {lowLatency} мс");
        }
        finally { GCSettings.LatencyMode = prev; }

        Console.WriteLine($"LatencyMode (конец): {GCSettings.LatencyMode}");
        Console.WriteLine("Примечание: эффект зависит от среды и нагрузки — это иллюстрация.");
    }

    private static long MeasureAllocations()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 2_000_000; i++) _ = new byte[64];
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
}
