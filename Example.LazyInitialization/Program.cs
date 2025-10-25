using System;

namespace Example.LazyInitialization;

file sealed class Heavy
{
    public Heavy() { Console.WriteLine("Heavy создан"); }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Lazy<T> ==");
        var lazy = new Lazy<Heavy>(() => new Heavy());
        Console.WriteLine("До обращения к Value объект не создаётся");
        _ = lazy.Value; // создаётся здесь
        _ = lazy.Value; // повторный доступ — тот же экземпляр
        Console.WriteLine("Исключения из фабрики по умолчанию кэшируются");
    }
}
