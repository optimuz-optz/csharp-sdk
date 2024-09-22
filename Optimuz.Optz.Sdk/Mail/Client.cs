using Optimuz.Optz.Sdk.Results;

namespace Optimuz.Optz.Sdk.Mail;

internal class Client
{
    private readonly Lazy<Email.Client> _email;
    private readonly Lazy<Sms.Client> _sms;
    private readonly Lazy<WhatsApp.Client> _whatsApp;

    internal Client(string host)
    {
        _email = new Lazy<Email.Client>(() => new Email.Client(host));
        _sms = new Lazy<Sms.Client>(() => new Sms.Client(host));
        _whatsApp = new Lazy<WhatsApp.Client>(() => new WhatsApp.Client(host));
    }

    internal async Task<OneOf<ItemResult<Email.Queue.Response>, ErrorResult>> QueueEmail(Email.Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return await _email.Value.Queue(request, authToken, cancellationToken);
    }

    internal async Task<OneOf<ItemResult<Sms.Queue.Response>, ErrorResult>> QueueSms(Sms.Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return await _sms.Value.Queue(request, authToken, cancellationToken);
    }

    internal async Task<OneOf<ItemResult<WhatsApp.Queue.Response>, ErrorResult>> QueueWhatsApp(WhatsApp.Queue.Request request, string authToken, CancellationToken cancellationToken)
    {
        return await _whatsApp.Value.Queue(request, authToken, cancellationToken);
    }
}
