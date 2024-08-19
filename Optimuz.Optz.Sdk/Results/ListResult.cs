namespace Optimuz.Optz.Sdk.Results;

public record ListResult<T>(
    [property: JsonPropertyName("data")] IEnumerable<T> Data,
    [property: JsonPropertyName("warnings")] IList<string> Warnings,
    [property: JsonPropertyName("total")] int Total
);