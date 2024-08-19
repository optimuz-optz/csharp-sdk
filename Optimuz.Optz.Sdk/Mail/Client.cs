using Optimuz.Optz.Sdk.Results;

namespace Optimuz.Optz.Sdk.Mail;

internal class Client : IDisposable
{
    private readonly Email.Client _email;
    private readonly Sms.Client _sms;

    internal Client(string host)
    {
        _email = new Email.Client(host);
        _sms = new Sms.Client(host);
    }

    public void Dispose()
    {
        _email.Dispose();
        _sms.Dispose();
    }

    internal async Task<OneOf<ItemResult<Email.Queue.Response>, ErrorResult>> QueueEmail(Email.Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return await _email.Queue(request, authToken, cancellationToken);
    }

    internal async Task<OneOf<ItemResult<Sms.Queue.Response>, ErrorResult>> QueueSms(Sms.Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return await _sms.Queue(request, authToken, cancellationToken);
    }
}
