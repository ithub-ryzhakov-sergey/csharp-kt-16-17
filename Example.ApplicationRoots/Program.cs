using System;

namespace Example.ApplicationRoots;

internal static class Program
{
    private static object? _staticRoot; // статический корень

    private static void Main()
    {
        Console.WriteLine("== Корневые элементы (roots) ==");

        var local = new object();
        _staticRoot = new object();
        var weak = new WeakReference(new object());

        Console.WriteLine($"До GC: weak.IsAlive={weak.IsAlive}");

        // Выходим из области действия local (после этого GC может собрать объект)
        local = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine($"После GC: weak.IsAlive={weak.IsAlive} (может стать false)");
        Console.WriteLine("Статический корень удерживает объект до тех пор, пока поле не будет очищено");

        _staticRoot = null; // снимаем корень
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Статический корень снят — объект может быть собран");
    }
}
