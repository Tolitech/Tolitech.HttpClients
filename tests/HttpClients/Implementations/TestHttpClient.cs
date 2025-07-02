using Tolitech.Results;

namespace Tolitech.HttpClients.UnitTests.Implementations;

internal class TestHttpClient(HttpClient client) : BaseHttpClient(client)
{
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
#pragma warning disable CA1822 // Mark members as static
    public StringContent? CallConvertRequestToJsonContent(TestRequest request)
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        return typeof(BaseHttpClient)
                .GetMethod("ConvertRequestToJsonContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(typeof(TestRequest))
                .Invoke(null, [request]) as StringContent;
    }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
#pragma warning disable CA1822 // Mark members as static
    public void CallAddHeaders(HttpRequestMessage msg, IDictionary<string, string>? headers)
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
        typeof(BaseHttpClient)
                .GetMethod("AddHeaders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .Invoke(null, [msg, headers]);
    }

    public Task<Result<TestResponse>> CallPostAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PostAsync<TestRequest, TestResponse>(url, body, token)
            : PostAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    public Task<Result<TestResponse>> CallPutAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PutAsync<TestRequest, TestResponse>(url, body, token)
            : PutAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    public Task<Result<TestResponse>> CallPatchAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PatchAsync<TestRequest, TestResponse>(url, body, token)
            : PatchAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    public Task<Result<TestResponse>> CallDeleteAsync(string url, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? DeleteAsync<TestResponse>(url, token)
            : DeleteAsync<TestResponse>(url, headers, token);
    }

    public Task<Result<TestResponse>> CallGetAsync(string url, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? GetAsync<TestResponse>(url, token)
            : GetAsync<TestResponse>(url, headers, token);
    }
}