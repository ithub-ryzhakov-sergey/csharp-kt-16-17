using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace App.Topics.UsingAndDispose.T2_HttpClientLifetime;

// Тестовый обработчик для имитации ответов и отслеживания Dispose
internal sealed class StubHandler : HttpMessageHandler
{
    public bool Disposed { get; private set; }
    public int Calls { get; private set; }
    public string ResponseContent { get; init; } = "OK";
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Calls++;
        var message = new HttpResponseMessage(StatusCode)
        {
            Content = new StringContent(ResponseContent)
        };
        return Task.FromResult(message);
    }

    protected override void Dispose(bool disposing)
    {
        Disposed = true;
        base.Dispose(disposing);
    }
}
