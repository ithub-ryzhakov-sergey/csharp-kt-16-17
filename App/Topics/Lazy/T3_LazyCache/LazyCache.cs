using System.Collections.Concurrent;

namespace App.Topics.Lazy.T3_LazyCache;

// Потокобезопасный кэш на Lazy<T>.
// Требования к студенту:
// - Хранить ConcurrentDictionary<TKey, Lazy<TValue>>
// - Использовать LazyThreadSafetyMode.ExecutionAndPublication
// - GetOrAdd гарантирует одиночную инициализацию на ключ
// - Исключение фабрики кэшируется (поведение Lazy по умолчанию)
internal sealed class LazyCache<TKey, TValue>
    where TKey : notnull
{
    public TValue GetOrAdd(TKey key, Func<TValue> factory)
    {
        // TODO: реализовать через ConcurrentDictionary + Lazy<T>
        throw new NotImplementedException();
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
    {
        // TODO: перегрузка с фабрикой, принимающей ключ
        throw new NotImplementedException();
    }
}
