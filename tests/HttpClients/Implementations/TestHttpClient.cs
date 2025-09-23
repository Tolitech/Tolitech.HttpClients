using Tolitech.Results;

namespace Tolitech.HttpClients.UnitTests.Implementations;

/// <summary>
/// Helper class to expose protected and private methods of <see cref="BaseHttpClient"/> for unit testing purposes.
/// </summary>
/// <param name="client">The HTTP client instance used for requests in tests.</param>
internal sealed class TestHttpClient(HttpClient client) : BaseHttpClient(client)
{
    /// <summary>
    /// Exposes the ConvertRequestToJsonContent method for unit testing.
    /// </summary>
    /// <param name="request">Instance of <see cref="TestRequest"/> to be converted.</param>
    /// <returns>Serialized JSON content or null if the request is null.</returns>
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

    /// <summary>
    /// Exposes the AddHeaders method for unit testing.
    /// </summary>
    /// <param name="msg">HTTP message to be modified.</param>
    /// <param name="headers">Additional headers to be added.</param>
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

    /// <summary>
    /// Calls the PostAsync method of BaseHttpClient for unit testing.
    /// </summary>
    /// <param name="url">Target URL.</param>
    /// <param name="body">Request body.</param>
    /// <param name="headers">Additional headers.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Request result.</returns>
    public Task<IResult<TestResponse>> CallPostAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PostAsync<TestRequest, TestResponse>(url, body, token)
            : PostAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    /// <summary>
    /// Calls the PutAsync method of BaseHttpClient for unit testing.
    /// </summary>
    /// <param name="url">Target URL.</param>
    /// <param name="body">Request body.</param>
    /// <param name="headers">Additional headers.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Request result.</returns>
    public Task<IResult<TestResponse>> CallPutAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PutAsync<TestRequest, TestResponse>(url, body, token)
            : PutAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    /// <summary>
    /// Calls the PatchAsync method of BaseHttpClient for unit testing.
    /// </summary>
    /// <param name="url">Target URL.</param>
    /// <param name="body">Request body.</param>
    /// <param name="headers">Additional headers.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Request result.</returns>
    public Task<IResult<TestResponse>> CallPatchAsync(string url, TestRequest body, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? PatchAsync<TestRequest, TestResponse>(url, body, token)
            : PatchAsync<TestRequest, TestResponse>(url, body, headers, token);
    }

    /// <summary>
    /// Calls the DeleteAsync method of BaseHttpClient for unit testing.
    /// </summary>
    /// <param name="url">Target URL.</param>
    /// <param name="headers">Additional headers.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Request result.</returns>
    public Task<IResult<TestResponse>> CallDeleteAsync(string url, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? DeleteAsync<TestResponse>(url, token)
            : DeleteAsync<TestResponse>(url, headers, token);
    }

    /// <summary>
    /// Calls the GetAsync method of BaseHttpClient for unit testing.
    /// </summary>
    /// <param name="url">Target URL.</param>
    /// <param name="headers">Additional headers.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Request result.</returns>
    public Task<IResult<TestResponse>> CallGetAsync(string url, IDictionary<string, string>? headers = null, CancellationToken token = default)
    {
        return headers is null
            ? GetAsync<TestResponse>(url, token)
            : GetAsync<TestResponse>(url, headers, token);
    }
}