using System.Collections.Concurrent;
using Optimuz.Optz.Sdk.Exceptions;
using Optimuz.Optz.Sdk.Results;

namespace Optimuz.Optz.Sdk;

public class Optz
{
    private static readonly ConcurrentDictionary<string, Optz> _instances = new();

    private readonly Lazy<Auth.Client> _auth;
    private readonly Lazy<Mail.Client> _mail;

    private readonly string _account;
    private readonly string _username;
    private readonly string _password;

    private Optz(string host, string account, string username, string password)
    {
        _auth = new Lazy<Auth.Client>(() => new Auth.Client(host.Replace("*", "https://auth")));
        _mail = new Lazy<Mail.Client>(() => new Mail.Client(host.Replace("*", "https://mail")));

        _account = account;
        _username = username;
        _password = password;
    }

    /// <summary>
    /// Gets an instance of the Optz class. This method is thread-safe and guarantees that it returns the same instance for the same parameters.
    /// </summary>
    /// <param name="host">The wildcard host (e.g., *.any.host.com).</param>
    /// <param name="account">The account name.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>An instance of the Optz class.</returns>
    /// <exception cref="InvalidHostException">Thrown when the host does not start with a wildcard.</exception>
    public static Optz GetInstance(string host, string account, string username, string password)
    {
        if (!host.StartsWith("*")) throw new InvalidHostException();
        var key = string.Format("{0}:{1}:{2}:{3}", host, account, username, password);
        return _instances.GetOrAdd(key, new Optz(host, account, username, password));
    }

    #region Mail

    /// <summary>
    /// Queues an email for sending.
    /// </summary>
    /// <param name="request">The email queue request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains either an ItemResult or an ErrorResult.</returns>
    public async Task<OneOf<ItemResult<Mail.Email.Queue.Response>, ErrorResult>> QueueEmail(Mail.Email.Queue.Request request, CancellationToken cancellationToken = default)
    {
        return await Authenticate(accessToken => _mail.Value.QueueEmail(request, accessToken, cancellationToken), cancellationToken);
    }

    /// <summary>
    /// Queues an SMS for sending.
    /// </summary>
    /// <param name="request">The SMS queue request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains either an ItemResult or an ErrorResult.</returns>
    public async Task<OneOf<ItemResult<Mail.Sms.Queue.Response>, ErrorResult>> QueueSms(Mail.Sms.Queue.Request request, CancellationToken cancellationToken = default)
    {
        return await Authenticate(accessToken => _mail.Value.QueueSms(request, accessToken, cancellationToken), cancellationToken);
    }

    /// <summary>
    /// Queues a WhatsApp message for sending.
    /// </summary>
    /// <param name="request">The WhatsApp queue request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains either an ItemResult or an ErrorResult.</returns>
    public async Task<OneOf<ItemResult<Mail.WhatsApp.Queue.Response>, ErrorResult>> QueueWhatsApp(Mail.WhatsApp.Queue.Request request, CancellationToken cancellationToken = default)
    {
        return await Authenticate(accessToken => _mail.Value.QueueWhatsApp(request, accessToken, cancellationToken), cancellationToken);
    }

    #endregion

    #region Auth

    /// <summary>
    /// Authenticates the user and executes the specified action.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="action">The action to execute after authentication.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains either the result of the action or an ErrorResult.</returns>
    internal async Task<OneOf<T, ErrorResult>> Authenticate<T>(Func<string, Task<OneOf<T, ErrorResult>>> action, CancellationToken cancellationToken = default)
    {
        var request = new Auth.Signin.Request(_account, _username, _password);
        var signin = await _auth.Value.Signin(request, cancellationToken);
        return await signin.Match(success => action(success.Data.AccessToken), error => Task.FromResult<OneOf<T, ErrorResult>>(error));
    }

    #endregion
}
