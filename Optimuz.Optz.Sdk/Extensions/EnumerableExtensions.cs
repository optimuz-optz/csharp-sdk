namespace Optimuz.Optz.Sdk.Extensions;

internal static class EnumerableExtensions
{
    internal static string Join(this IEnumerable<string> value, string separator)
    {
        return string.Join(separator, value);
    }
}
