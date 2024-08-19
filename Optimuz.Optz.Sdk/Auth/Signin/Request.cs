namespace Optimuz.Optz.Sdk.Auth.Signin;

public record Request(
    [property: JsonPropertyName("conta")] string Account,
    [property: JsonPropertyName("usuario")] string Username,
    [property: JsonPropertyName("senha")] string Password
);