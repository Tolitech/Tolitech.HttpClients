using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Moq;
using Moq.Protected;

using Tolitech.HttpClients.UnitTests.Implementations;

namespace Tolitech.HttpClients.UnitTests;

/// <summary>
/// Unit tests for the <see cref="BaseHttpClient"/> class, covering all public and protected methods.
/// </summary>
public class BaseHttpClientTests
{
    /// <summary>
    /// Tests that PostAsync returns a successful result when the response is OK.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task PostAsync_Success_ReturnsOkResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "ok" });
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("ok", result.Value.Result);
    }

    /// <summary>
    /// Tests that PutAsync returns a successful result when the response is OK.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task PutAsync_Success_ReturnsOkResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "updated" });
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPutAsync("http://test", new TestRequest());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("updated", result.Value.Result);
    }

    /// <summary>
    /// Tests that PatchAsync returns a successful result when the response is OK.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task PatchAsync_Success_ReturnsOkResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "patched" });
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPatchAsync("http://test", new TestRequest());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("patched", result.Value.Result);
    }

    /// <summary>
    /// Tests that DeleteAsync returns a successful result when the response is OK.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task DeleteAsync_Success_ReturnsOkResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "deleted" });
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallDeleteAsync("http://test");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("deleted", result.Value.Result);
    }

    /// <summary>
    /// Tests that GetAsync returns a successful result when the response is OK.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetAsync_Success_ReturnsOkResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, new TestResponse { Result = "fetched" });
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallGetAsync("http://test");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("fetched", result.Value.Result);
    }

    /// <summary>
    /// Tests that PostAsync returns a result with the correct status code when the response is NoContent.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task PostAsync_NoContent_ReturnsResultWithStatusCode()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.NoContent);
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());

        // Assert
        Assert.Equal((int)HttpStatusCode.NoContent, (int)result.StatusCode);
    }

    /// <summary>
    /// Tests that SendRequestAsync returns an error result when the response is a problem+json.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task SendRequestAsync_ProblemJson_ReturnsErrorResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, new { title = "error" }, "application/problem+json");
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());

        // Assert
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Tests that SendRequestAsync returns an error result when the response is an error content.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task SendRequestAsync_ErrorContent_ReturnsErrorResult()
    {
        // Arrange
        using HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, "error", "text/plain");
        TestHttpClient client = new(httpClient);

        // Act
        Results.Result<TestResponse> result = await client.CallPostAsync("http://test", new TestRequest());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)HttpStatusCode.BadRequest, (int)result.StatusCode);
    }

    /// <summary>
    /// Tests that ConvertRequestToJsonContent returns null when the request is null.
    /// </summary>
    [Fact]
    public void ConvertRequestToJsonContent_NullRequest_ReturnsNull()
    {
        // Arrange
        using HttpClient httpClient = new();
        TestHttpClient client = new(httpClient);

        // Act
        StringContent? content = client.CallConvertRequestToJsonContent(null!);

        // Assert
        Assert.Null(content);
    }

    /// <summary>
    /// Tests that ConvertRequestToJsonContent returns a StringContent when the request is valid.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task ConvertRequestToJsonContent_ValidRequest_ReturnsStringContent()
    {
        // Arrange
        using HttpClient httpClient = new();
        TestHttpClient client = new(httpClient);

        // Act
        StringContent? content = client.CallConvertRequestToJsonContent(new TestRequest { Data = "abc" });

        // Assert
        Assert.NotNull(content);
        string json = await content.ReadAsStringAsync();
        Assert.Contains("abc", json, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that AddHeaders adds Accept-Language and custom headers to the request.
    /// </summary>
    [Fact]
    public void AddHeaders_AddsAcceptLanguageAndCustomHeaders()
    {
        // Arrange
        using HttpClient httpClient = new();
        TestHttpClient client = new(httpClient);
        using HttpRequestMessage msg = new();
        Dictionary<string, string> headers = new() { { "X-Test", "123" } };

        // Act
        client.CallAddHeaders(msg, headers);

        // Assert
        Assert.True(msg.Headers.Contains("Accept-Language"));
        Assert.True(msg.Headers.Contains("X-Test"));
    }

    /// <summary>
    /// Tests that AddHeaders does not duplicate headers if already present.
    /// </summary>
    [Fact]
    public void AddHeaders_DoesNotDuplicateHeaders()
    {
        // Arrange
        using HttpClient httpClient = new();
        TestHttpClient client = new(httpClient);
        using HttpRequestMessage msg = new();
        msg.Headers.Add("X-Test", "existing");
        Dictionary<string, string> headers = new() { { "X-Test", "123" } };

        // Act
        client.CallAddHeaders(msg, headers);

        // Assert
        Assert.Single(msg.Headers.GetValues("X-Test"));
    }

    /// <summary>
    /// Creates a mock HttpClient that returns a specified status code and response body.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <param name="responseBody">The response body to return.</param>
    /// <param name="mediaType">The media type of the response.</param>
    /// <returns>A mock HttpClient instance.</returns>
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