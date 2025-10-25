# Практика по .NET 9: управление памятью, ресурсы и отложенная инициализация

Набор демонстрационных консольных приложений и 3 практических задачи для понимания:
- времени жизни объектов и корней (roots), поколений и LOH;
- финализаторов и высвобождаемых объектов (`IDisposable`, `using`, `try/finally`);
- практического использования `System.GC` и связанных API;
- отложенной инициализации через `Lazy<T>`;
- корректного жизненного цикла `HttpClient` в продакшен-коде.

Все примеры и задания ориентированы на .NET 9 и современный C# (file-scoped namespaces, лаконичные выражения, современные конструкции языка).

---

## Как запускать примеры
Каждый пример — отдельный проект `Example.*` с собственным `README.md`.
- Перейдите в папку нужного проекта и выполните:
```
dotnet run
```
- В консоли вы увидите понятный лог «что происходит и почему».

Список примеров:
- `Example.ObjectLifetimeBasics`
- `Example.ApplicationRoots`
- `Example.GenerationsAndLOH`
- `Example.GC_API`
- `Example.Finalizers`
- `Example.IDisposable_Using`
- `Example.Finalize_vs_Dispose`
- `Example.AddMemoryPressure`
- `Example.GC_LatencyModes`
- `Example.LazyInitialization`

У каждого примера есть краткая теория, что смотреть в выводе, и ссылки «что почитать» (официальная документация .NET).

---

## Практические задачи (App)
Задачи расположены в проекте `App`, тесты — в `App.Tests`. Изначально тесты падают — это ожидаемо. Цель — реализовать пропущенную логику и добиться «зелёных» тестов.

- **UsingAndDispose/T1_UsingTryFinally — `using` ↔ `try/finally` и шаблон `IDisposable`**
  - **Где код:** `App/Topics/UsingAndDispose/T1_UsingTryFinally/`
  - **Нужно реализовать:**
    - `SampleResource.Dispose()` — идемпотентно. Повторные вызовы безопасны; после `Dispose` все методы должны кидать `ObjectDisposedException`.
    - `UsingTryFinallyRunner.RunWithUsing(ILog log, bool throwInWork)` — реализация через `using`.
    - `UsingTryFinallyRunner.RunWithTryFinally(ILog log, bool throwInWork)` — эквивалент вручную через `try/finally`.
    - При нормальном сценарии лог должен быть в порядке: `OPEN`, `WORK`, `DISPOSE`.
    - Если `throwInWork=true` — «работа» кидает исключение, но `DISPOSE` всё равно пишется последним.
  - **Что проверяют тесты:** `App.Tests/Topics/UsingAndDispose/T1_UsingTryFinally/RunnerTests.cs`
    - `Using_and_TryFinally_produce_same_order_without_exception` — оба пути дают одинаковый порядок: `OPEN`, `WORK`, `DISPOSE`.
    - `Using_calls_Dispose_even_if_work_throws` — при исключении из «работы» последний лог — `DISPOSE`.
    - `SampleResource_Dispose_is_idempotent` — двойной `Dispose` не падает; последующие вызовы `Open/DoWork` → `ObjectDisposedException`.

- **UsingAndDispose/T2_HttpClientLifetime — жизненный цикл `HttpClient`**
  - **Где код:** `App/Topics/UsingAndDispose/T2_HttpClientLifetime/`
  - **Нужно реализовать:**
    - Конструктор без параметров — создать `SocketsHttpHandler` и `HttpClient`, пометить «владение handler=true`.
    - Конструктор с `HttpMessageHandler externalHandler` — создать `HttpClient` поверх внешнего handler, пометить «владение handler=false`.
    - Свойство `Timeout` — проксировать к `HttpClient.Timeout`.
    - `GetStringAsync(Uri, CancellationToken)` — проксировать к `HttpClient.GetStringAsync`.
    - `Dispose()` — идемпотентно. Освобождать клиент всегда; освобождать handler только если «владеем» им. После `Dispose` любые вызовы API → `ObjectDisposedException`.
  - **Что проверяют тесты:** `App.Tests/Topics/UsingAndDispose/T2_HttpClientLifetime/HttpClientWrapperTests.cs`
    - `Should_use_external_handler_and_not_dispose_it` — при внешнем handler контент возвращается, handler-вызовов 2, после `Dispose` обёртки сам handler не освобождён.
    - `Should_throw_after_dispose` — после `Dispose` любой вызов API бросает `ObjectDisposedException`.
    - `Should_apply_timeout_property` — установка/чтение `Timeout` работает.
    - `Should_support_cancellation` — при отменённом `CancellationToken` вызывается `OperationCanceledException`.
    - `Should_reuse_client_for_multiple_requests` — 10 вызовов подряд без пересоздания клиента; счётчик вызовов handler равен 10.

- **Lazy/T3_LazyCache — потокобезопасный кэш на `Lazy<T>`**
  - **Где код:** `App/Topics/Lazy/T3_LazyCache/`
  - **Нужно реализовать:**
    - Внутри — `ConcurrentDictionary<TKey, Lazy<TValue>>`.
    - `GetOrAdd(TKey, Func<TValue>)` — один вызов фабрики на ключ (использовать `LazyThreadSafetyMode.ExecutionAndPublication`).
    - `GetOrAdd(TKey, Func<TKey, TValue>)` — перегрузка, прокидывающая ключ в фабрику.
    - Исключения фабрики по умолчанию кэшируются и повторно бросаются при следующем доступе к тому же ключу.
  - **Что проверяют тесты:** `App.Tests/Topics/Lazy/T3_LazyCache/LazyCacheTests.cs`
    - `Same_key_initialized_once_concurrently` — 20 параллельных обращений к одному ключу вызывают фабрику ровно 1 раз; все получают одинаковое значение.
    - `Different_keys_initialized_separately` — разные ключи инициализируются независимо; счётчик фабрики равен 2.
    - `Exception_is_cached_by_default` — первое обращение кидает `InvalidOperationException("boom")`, последующее к тому же ключу кидает то же исключение (даже если фабрика «здоровая»).

### Как работать с задачами
1. Откройте проект `App` и изучите заготовки (в них оставлены «дырки» с `NotImplementedException`).
2. Запустите тесты:
```
dotnet test App.Tests/App.Tests.csproj
```
3. Реализуйте пропущенную логику и доводите тесты до зелёного состояния по одной задаче.

> Замечание: В ряде тестов используется `GC.Collect()` и `GC.WaitForPendingFinalizers()` — это учебные сценарии для детерминизации примеров. В продакшене вызывать `GC.Collect()` вручную не рекомендуется.

---

## CI
- В репозитории настроен GitHub Actions workflow `.github/workflows/ci.yml`:
  - сборка всех проектов под .NET 9;
  - запуск `App.Tests`.
- На любой push/PR выполняется `restore`, `build`, `test`.

---

## Полезные рекомендации
- Не создавайте и не уничтожайте `HttpClient` на каждый запрос. Используйте долгоживущий экземпляр или `IHttpClientFactory` (ASP.NET Core).
- Реализуйте `IDisposable` идемпотентно и вызывайте `Dispose` даже при исключениях (конструкция `using` автоматически разворачивается в `try/finally`).
- `Lazy<T>` удобен для отложенной инициализации тяжёлых зависимостей или кэширования инициализации.
- Финализаторы нужны только для неуправляемых ресурсов; при наличии `Dispose` подавляйте финализацию через `GC.SuppressFinalize(this)`.
- Избегайте ручного вызова `GC.Collect()` — полагайтесь на CLR.
