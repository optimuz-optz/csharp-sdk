namespace Optimuz.Optz.Sdk.Mail.Sms.Queue;

public record Request(
    [property: JsonPropertyName("de")] string From,
    [property: JsonPropertyName("para")] string To,
    [property: JsonPropertyName("mensagem")] string Message
);