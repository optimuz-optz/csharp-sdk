using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Optimuz.Optz.Sdk.Results;

namespace Optimuz.Optz.Sdk.Auth;

internal class Client : IDisposable
{
    private readonly HttpClient _client;
    private readonly MemoryCache _cache;

    internal Client(string host)
    {
        _client = new HttpClient(host);
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public void Dispose()
    {
        _client.Dispose();
        _cache.Dispose();
    }

    internal Task<OneOf<ItemResult<Signin.Response>, ErrorResult>> Signin(Signin.Request request, CancellationToken cancellationToken)
    {
        var key = string.Format("{0}:{1}:{2}", request.Account, request.Username, request.Password);
        return _cache.GetOrCreateAsync(key, async (entry) =>
        {
            var result = await _client.Post<ItemResult<Signin.Response>>("signin", request, cancellationToken);
            return result.Match<OneOf<ItemResult<Signin.Response>, ErrorResult>>(success => OnSigninSuccess(entry, success), error => OnSigninError(entry, error));
        });
    }

    private ItemResult<Signin.Response> OnSigninSuccess(ICacheEntry entry, ItemResult<Signin.Response> success)
    {
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.ReadJwtToken(success.Data.AccessToken);
        var expiration = accessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
        entry.AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiration.Value));
        return success;
    }

    private ErrorResult OnSigninError(ICacheEntry entry, ErrorResult error)
    {
        entry.AbsoluteExpiration = DateTimeOffset.Now;
        return error;
    }
}