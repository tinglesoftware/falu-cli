namespace System.Net.Http.Headers;

internal static class HttpRequestHeadersExtensions
{
    public static void Replace(this HttpRequestHeaders headers, string key, string value)
    {
        headers.Remove(key);
        headers.Add(key, value);
    }
}
