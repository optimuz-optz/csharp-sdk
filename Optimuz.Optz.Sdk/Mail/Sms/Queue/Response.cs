namespace Optimuz.Optz.Sdk.Mail.Sms.Queue;

public record Response(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("de")] string From,
    [property: JsonPropertyName("para")] string To,
    [property: JsonPropertyName("mensagem")] string Message
);