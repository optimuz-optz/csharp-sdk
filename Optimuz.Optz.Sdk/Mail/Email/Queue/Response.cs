namespace Optimuz.Optz.Sdk.Mail.Email.Queue;

public record Response(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("para")] string To,
    [property: JsonPropertyName("assunto")] string Subject,
    [property: JsonPropertyName("mensagem")] string Message
);