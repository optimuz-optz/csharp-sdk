namespace Optimuz.Optz.Sdk.Auth.Signin;

public record Response(
    [property: JsonPropertyName("accessToken")] string AccessToken,
    [property: JsonPropertyName("refreshToken")] string RefreshToken
);