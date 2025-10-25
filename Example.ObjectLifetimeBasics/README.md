# Example.ObjectLifetimeBasics

- **Что показывает:** размещение объектов, отличие ссылки от объекта, поколение объекта, эффект `GC.Collect`.
- **Ключевые API:** `System.GC.GetTotalMemory`, `System.GC.MaxGeneration`, `System.GC.GetGeneration`, `System.GC.Collect`, `System.GC.WaitForPendingFinalizers`.
- **На что обратить внимание:** установка ссылки в `null` не заставляет GC немедленно собирать объект; это лишь делает объект недостижимым.
- **Что почитать:**
  - Документация: Garbage Collection (основы)
  - Класс `System.GC`

Запуск:
```
dotnet run
```
