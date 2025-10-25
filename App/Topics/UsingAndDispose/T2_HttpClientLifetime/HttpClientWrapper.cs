using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace App.Topics.UsingAndDispose.T2_HttpClientLifetime;

// Обёртка над HttpClient с корректной моделью владения HttpMessageHandler.
// Требования к студенту:
// - Реализовать конструкторы (с внутренним SocketsHttpHandler и с внешним HttpMessageHandler)
// - Реализовать идемпотентный Dispose(bool), учитывая владение/невладение handler
// - Реализовать GetStringAsync с поддержкой CancellationToken
// - Свойство Timeout должно проксировать таймаут клиента
internal sealed class HttpClientWrapper : IDisposable
{
    public HttpClientWrapper()
    {
        // TODO: создать SocketsHttpHandler, HttpClient поверх него, пометить владение handler=true
        throw new NotImplementedException();
    }

    public HttpClientWrapper(HttpMessageHandler externalHandler)
    {
        // TODO: создать HttpClient поверх externalHandler, пометить владение handler=false
        throw new NotImplementedException();
    }

    public TimeSpan Timeout
    {
        // TODO: проксировать свойство HttpClient.Timeout
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public Task<string> GetStringAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        // TODO: вызвать HttpClient.GetStringAsync(uri, cancellationToken)
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // TODO: идемпотентный Dispose; при владении — Disposе handler и клиент; иначе — только клиент
        throw new NotImplementedException();
    }
}
