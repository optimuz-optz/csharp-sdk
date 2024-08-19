using System.Net;

namespace Optimuz.Optz.Sdk.Exceptions;

public class InternalServerErrorException(HttpStatusCode statusCode) : Exception(string.Format(template, statusCode))
{
    private const string template = "The server responded with status {0}";
}
