using Optimuz.Optz.Sdk.Results;

namespace Optimuz.Optz.Sdk.Mail.Sms;

internal class Client : IDisposable
{
    private readonly HttpClient _client;

    internal Client(string host)
    {
        _client = new HttpClient(host, "sms");
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    internal Task<OneOf<ItemResult<Queue.Response>, ErrorResult>> Queue(Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return _client.Post<ItemResult<Queue.Response>>("queue", request, authToken, cancellationToken);
    }
}
