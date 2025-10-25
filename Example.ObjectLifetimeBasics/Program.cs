using System;

namespace Example.ObjectLifetimeBasics;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Базовая информация о памяти ==");
        Console.WriteLine($"Поколений поддерживается: {GC.MaxGeneration + 1}");
        Console.WriteLine($"Память (байт) до: {GC.GetTotalMemory(false)}");

        var obj = new object();
        Console.WriteLine($"Поколение нового объекта: {GC.GetGeneration(obj)}");

        obj = null; // Обнуляем ссылку, объект станет недостижимым
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine($"Память (байт) после GC: {GC.GetTotalMemory(true)}");

        Console.WriteLine("Подсказка: установка ссылки в null не форсирует сборку, это лишь разрыв связи.");
    }
}
