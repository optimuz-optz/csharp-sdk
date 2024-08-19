namespace Optimuz.Optz.Sdk.Results;

public record ItemResult<T>(
    [property: JsonPropertyName("data")] T Data,
    [property: JsonPropertyName("warnings")] IList<string> Warnings
);