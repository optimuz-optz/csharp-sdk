namespace Optimuz.Optz.Sdk.Results;

public record ErrorResult(
    [property: JsonPropertyName("errors")] IEnumerable<string> Errors,
    [property: JsonPropertyName("warnings")] IEnumerable<string> Warnings
);