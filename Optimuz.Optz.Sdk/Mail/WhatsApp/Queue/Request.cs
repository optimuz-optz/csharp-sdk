namespace Optimuz.Optz.Sdk.Mail.WhatsApp.Queue;

public record Request(
    [property: JsonPropertyName("template")] string Template,
    [property: JsonPropertyName("de")] string From,
    [property: JsonPropertyName("para")] string To,
    [property: JsonPropertyName("parametros")] string[] Parameters,
    [property: JsonPropertyName("idioma")] string Language
);