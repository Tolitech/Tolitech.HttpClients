using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Moq;
using Moq.Protected;

using Tolitech.HttpClients.UnitTests.Implementations;

namespace Tolitech.HttpClients.UnitTests;

public class BaseHttpClientTests
{
    [Fact]
    public async Task PostAsync_Success_ReturnsOkResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "ok" });

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());
        Assert.True(result.IsSuccess);
        Assert.Equal("ok", result.Value.Result);
    }

    [Fact]
    public async Task PutAsync_Success_ReturnsOkResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "updated" });

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPutAsync("http://test", new TestRequest());
        Assert.True(result.IsSuccess);
        Assert.Equal("updated", result.Value.Result);
    }

    [Fact]
    public async Task PatchAsync_Success_ReturnsOkResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "patched" });

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPatchAsync("http://test", new TestRequest());
        Assert.True(result.IsSuccess);
        Assert.Equal("patched", result.Value.Result);
    }

    [Fact]
    public async Task DeleteAsync_Success_ReturnsOkResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "deleted" });

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallDeleteAsync("http://test");
        Assert.True(result.IsSuccess);
        Assert.Equal("deleted", result.Value.Result);
    }

    [Fact]
    public async Task GetAsync_Success_ReturnsOkResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "fetched" });

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallGetAsync("http://test");
        Assert.True(result.IsSuccess);
        Assert.Equal("fetched", result.Value.Result);
    }

    [Fact]
    public async Task PostAsync_NoContent_ReturnsResultWithStatusCode()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.NoContent);

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());
        Assert.Equal((int)HttpStatusCode.NoContent, (int)result.StatusCode);
    }

    [Fact]
    public async Task SendRequestAsync_ProblemJson_ReturnsErrorResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, new { title = "error" }, "application/problem+json");

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task SendRequestAsync_ErrorContent_ReturnsErrorResult()
    {
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, "error", "text/plain");

        TestHttpClient client = new(httpClient);
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());
        Assert.False(result.IsSuccess);
        Assert.Equal((int)HttpStatusCode.BadRequest, (int)result.StatusCode);
    }

    [Fact]
    public void ConvertRequestToJsonContent_NullRequest_ReturnsNull()
    {
        using HttpClient httpClient = new();

        TestHttpClient client = new(httpClient);
        StringContent? content = client.CallConvertRequestToJsonContent(null!);
        Assert.Null(content);
    }

    [Fact]
    public async Task ConvertRequestToJsonContent_ValidRequest_ReturnsStringContent()
    {
        using HttpClient httpClient = new();

        TestHttpClient client = new(httpClient);
        StringContent? content = client.CallConvertRequestToJsonContent(new TestRequest { Data = "abc" });
        Assert.NotNull(content);
        string json = await content.ReadAsStringAsync();
        Assert.Contains("abc", json, StringComparison.Ordinal);
    }

    [Fact]
    public void AddHeaders_AddsAcceptLanguageAndCustomHeaders()
    {
        using HttpClient httpClient = new();

        TestHttpClient client = new(httpClient);
        using HttpRequestMessage msg = new();
        Dictionary<string, string> headers = new() { { "X-Test", "123" } };
        client.CallAddHeaders(msg, headers);

        Assert.True(msg.Headers.Contains("Accept-Language"));
        Assert.True(msg.Headers.Contains("X-Test"));
    }

    [Fact]
    public void AddHeaders_DoesNotDuplicateHeaders()
    {
        using HttpClient httpClient = new();

        TestHttpClient client = new(httpClient);

        using HttpRequestMessage msg = new();
        msg.Headers.Add("X-Test", "existing");

        Dictionary<string, string> headers = new() { { "X-Test", "123" } };
        client.CallAddHeaders(msg, headers);

        // Should not add duplicate
        Assert.Single(msg.Headers.GetValues("X-Test"));
    }

    private static HttpClient CreateMockHttpClient(HttpStatusCode statusCode, object? responseBody = null, string? mediaType = "application/json")
    {
        Mock<HttpMessageHandler> handlerMock = new(MockBehavior.Loose);

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage _, CancellationToken __) =>
            {
                HttpResponseMessage response = new()
                {
                    StatusCode = statusCode,
                    Content = responseBody != null
                        ? new StringContent(JsonSerializer.Serialize(responseBody), Encoding.UTF8, mediaType)
                        : new StringContent(string.Empty),
                };

                if (mediaType != null)
                {
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                }

                return response;
            });

        return new HttpClient(handlerMock.Object);
    }
}