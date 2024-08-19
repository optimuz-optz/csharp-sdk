namespace Optimuz.Optz.Sdk.Mail.Email.Queue;

public record Request(
    [property: JsonPropertyName("para")] string To,
    [property: JsonPropertyName("assunto")] string Subject,
    [property: JsonPropertyName("mensagem")] string Message
);