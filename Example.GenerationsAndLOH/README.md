# Example.GenerationsAndLOH

- **Что показывает:** поколения 0/1/2 и Large Object Heap (LOH).
- **Ключевые API:** `GC.CollectionCount`, `GC.GetGeneration`.
- **Подсказка:** массивы > ~85_000 байт попадают в LOH и обычно принадлежат поколению 2.
- **Что почитать:** Large Object Heap, Generations в .NET.

Запуск:
```
dotnet run
```
