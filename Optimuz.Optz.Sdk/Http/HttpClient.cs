using Optimuz.Optz.Sdk.Exceptions;
using Optimuz.Optz.Sdk.Results;
using Optimuz.Optz.Sdk.Extensions;
using System.Net.Http.Headers;

namespace Optimuz.Optz.Sdk.Http;

internal class HttpClient
{
    private readonly System.Net.Http.HttpClient _client;

    internal HttpClient(params string[] path)
    {
        _client = new System.Net.Http.HttpClient() { BaseAddress = BuildBaseAddress(path) };
    }

    private static Uri BuildBaseAddress(string[] parts)
    {
        return new Uri(parts.Select(part => part.Trim('/')).Join("/").Concat('/'));
    }

    internal async Task<OneOf<T, ErrorResult>> Get<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Get, endpoint, cancellationToken: cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Get<T>(string endpoint, string accessToken, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Get, endpoint, accessToken: accessToken, cancellationToken: cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Post<T>(string endpoint, object payload, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Post, endpoint, payload, accessToken: null, cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Post<T>(string endpoint, object payload, string accessToken, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Post, endpoint, payload, accessToken, cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Put<T>(string endpoint, object payload, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Put, endpoint, payload, cancellationToken: cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Put<T>(string endpoint, object payload, string accessToken, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Put, endpoint, payload, accessToken, cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Delete<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Delete, endpoint, cancellationToken: cancellationToken);
    }

    internal async Task<OneOf<T, ErrorResult>> Delete<T>(string endpoint, string accessToken, CancellationToken cancellationToken = default)
    {
        return await Request<T>(HttpMethod.Delete, endpoint, accessToken: accessToken, cancellationToken: cancellationToken);
    }

    private async Task<OneOf<T, ErrorResult>> Request<T>(HttpMethod method, string endpoint, object? payload = null, string? accessToken = null, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(method, endpoint, payload, accessToken);
        var response = await _client.SendAsync(request, cancellationToken);
        if ((int)response.StatusCode >= 500) throw new InternalServerErrorException(response.StatusCode);
        if ((int)response.StatusCode == 401) throw new NotAuthorizedException();
        var content = await response.Content.ReadAsStringAsync();
        return response.IsSuccessStatusCode ? JsonSerializer.Deserialize<T>(content)! : JsonSerializer.Deserialize<ErrorResult>(content)!;
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, object? payload = null, string? accessToken = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = method,
            RequestUri = new Uri(endpoint, UriKind.Relative),
        };

        if (payload != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        }

        if (accessToken != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return request;
    }
}
