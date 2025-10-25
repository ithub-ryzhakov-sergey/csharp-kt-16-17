namespace App.Topics.UsingAndDispose.T1_UsingTryFinally;

// Простой объект-ресурс. Требования к студенту:
// - Реализовать идемпотентный Dispose (многократные вызовы безопасны)
// - Бросать ObjectDisposedException при использовании после Dispose
internal sealed class SampleResource : IDisposable
{
    private bool _disposed;

    public void Open()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(SampleResource));
        // здесь могла бы быть инициализация ресурса
    }

    public void DoWork()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(SampleResource));
        // полезная работа
    }

    public void Dispose()
    {
        // TODO: реализуйте идемпотентно
        throw new NotImplementedException();
    }
}
