using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Tolitech.HttpClients.Abstractions;
using Tolitech.Results;
using Tolitech.Results.Http;

namespace Tolitech.HttpClients;

/// <summary>
/// Base class for making HTTP client requests.
/// </summary>
/// <param name="httpClient">The HTTP client used to send requests.</param>
public abstract class BaseHttpClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions WebOptions = JsonSerializerOptions.Web;

    /// <summary>
    /// Sends a POST request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PostAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        return await PostAsync<TRequest, TResponse>(url, body, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a POST request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PostAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        using HttpContent? requestBody = ConvertRequestToJsonContent(body);

        return await SendRequestAsync<TResponse>(HttpMethod.Post, url, requestBody, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PUT request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PutAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        return await PutAsync<TRequest, TResponse>(url, body, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PUT request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PutAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        using HttpContent? requestBody = ConvertRequestToJsonContent(body);

        return await SendRequestAsync<TResponse>(HttpMethod.Put, url, requestBody, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PATCH request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PatchAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        return await PatchAsync<TRequest, TResponse>(url, body, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a PATCH request to the specified URL with the provided body and headers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="body">The request body.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> PatchAsync<TRequest, TResponse>(
        string url,
        TRequest body,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TRequest : IRequest
            where TResponse : IResponse
    {
        using HttpContent? requestBody = ConvertRequestToJsonContent(body);

        return await SendRequestAsync<TResponse>(HttpMethod.Patch, url, requestBody, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a DELETE request to the specified URL with the provided headers.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> DeleteAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        return await DeleteAsync<TResponse>(url, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a DELETE request to the specified URL with the provided headers.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        return await SendRequestAsync<TResponse>(HttpMethod.Delete, url, null, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends an upload request to the specified URL asynchronously and returns the server response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected from the server. Must implement <see cref="IResponse"/>.</typeparam>
    /// <param name="url">The URL to which the upload request is sent. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="request">The upload request containing the data and parameters to be sent. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TResponse}"/>
    /// representing the server's response to the upload request.</returns>
    protected async Task<Result<TResponse>> UploadAsync<TResponse>(
        string url,
        UploadRequest request,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        return await UploadAsync<TResponse>(url, request, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a multipart/form-data HTTP POST request to upload a file and returns the deserialized response.
    /// </summary>
    /// <remarks>The file is uploaded using the multipart/form-data content type. The method serializes the
    /// file and associated metadata as specified in the <paramref name="request"/> parameter. <para> The caller is
    /// responsible for ensuring that the provided <paramref name="url"/> is valid and that the <paramref
    /// name="request"/> contains a readable file stream. </para> <para> The operation is performed asynchronously and
    /// can be cancelled via the <paramref name="cancellationToken"/>. </para></remarks>
    /// <typeparam name="TResponse">The type of the response object expected from the server. Must implement <see cref="IResponse"/>.</typeparam>
    /// <param name="url">The destination URL to which the file will be uploaded. Must be a valid absolute URI.</param>
    /// <param name="request">The <see cref="UploadRequest"/> containing the file stream, content type, and related metadata. Cannot be <see
    /// langword="null"/>.</param>
    /// <param name="additionalHeaders">An optional collection of additional HTTP headers to include in the request. May be <see langword="null"/> if no
    /// extra headers are needed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="Result{TResponse}"/> containing the deserialized server response of type <typeparamref
    /// name="TResponse"/>.</returns>
    protected async Task<Result<TResponse>> UploadAsync<TResponse>(
        string url,
        UploadRequest request,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        ArgumentNullException.ThrowIfNull(request);

        using MultipartFormDataContent content = [];

        if (request.FileStream is null)
        {
            using StreamContent fileContent = new(Stream.Null);
            content.Add(fileContent, request.Key);
        }
        else
        {
            using StreamContent fileContent = new(request.FileStream);

            if (!string.IsNullOrEmpty(request.ContentType))
            {
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
            }

            if (string.IsNullOrEmpty(request.FileName))
            {
                content.Add(fileContent, request.Key);
            }
            else
            {
                content.Add(fileContent, request.Key, request.FileName);
            }
        }

        return await SendRequestAsync<TResponse>(HttpMethod.Post, url, content, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a GET request to the specified URL with the provided headers.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> GetAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        return await GetAsync<TResponse>(url, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a GET request to the specified URL with the provided headers.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<TResponse>> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        return await SendRequestAsync<TResponse>(HttpMethod.Get, url, null, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a GET request to the specified URL with the provided headers.
    /// </summary>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<DownloadResponse>> DownloadAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        return await DownloadAsync(url, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a GET request to the specified URL with the provided headers.
    /// </summary>
    /// <param name="url">The endpoint URL.</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    protected async Task<Result<DownloadResponse>> DownloadAsync(
        string url,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync<DownloadResponse>(HttpMethod.Get, url, null, additionalHeaders, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds additional headers to the HTTP request.
    /// </summary>
    /// <param name="requestMessage">The HTTP request message.</param>
    /// <param name="headers">A dictionary of headers to add.</param>
    private static void AddHeaders(HttpRequestMessage requestMessage, IDictionary<string, string>? headers)
    {
        headers ??= new Dictionary<string, string>();

        if (!headers.ContainsKey("Accept-Language"))
        {
            headers.Add("Accept-Language", CultureInfo.CurrentCulture.Name);
        }

        foreach (KeyValuePair<string, string> header in headers)
        {
            if (!requestMessage.Headers.Contains(header.Key))
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }
    }

    /// <summary>
    /// Converts a request object of type <typeparamref name="TRequest"/> to a JSON-formatted <see cref="StringContent"/> object.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request object, which must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="request">The request object to be converted to content.</param>
    /// <returns>
    /// A <see cref="StringContent"/> object containing the serialized JSON representation of the request,
    /// or <c>null</c> if the request is <c>null</c>.
    /// </returns>
    private static StringContent? ConvertRequestToJsonContent<TRequest>(TRequest request)
        where TRequest : IRequest
    {
        return request is null
            ? null
            : new StringContent(
                JsonSerializer.Serialize(request, WebOptions),
                Encoding.UTF8,
                "application/json");
    }

    /// <summary>
    /// Sends a request with the specified HTTP method, body, and headers.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected.</typeparam>
    /// <param name="method">The HTTP method (POST, PUT, PATCH, GET, DELETE).</param>
    /// <param name="url">The endpoint URI.</param>
    /// <param name="requestBody">The request body (optional).</param>
    /// <param name="additionalHeaders">Additional headers to include in the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the response or error details.</returns>
    private async Task<Result<TResponse>> SendRequestAsync<TResponse>(
        HttpMethod method,
        string url,
        HttpContent? requestBody,
        IDictionary<string, string>? additionalHeaders = null,
        CancellationToken cancellationToken = default)
            where TResponse : IResponse
    {
        Result<TResponse> result = new();

        using HttpRequestMessage requestMessage = new(method, url);

        // Add headers
        AddHeaders(requestMessage, additionalHeaders);

        // Add body if applicable (POST, PUT, PATCH)
        if (requestBody is { })
        {
            requestMessage.Content = requestBody;
        }

        try
        {
            // Send the request
            HttpResponseMessage response = await httpClient.SendAsync(
                requestMessage,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken)
                    .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return result.WithStatusCode((StatusCode)response.StatusCode);
                }

                if (typeof(TResponse) == typeof(DownloadResponse))
                {
                    Stream fileStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

                    DownloadResponse responseStream = new(
                        response.Content.Headers.ContentDisposition?.FileName?.Trim('"'),
                        response.Content.Headers.ContentType?.MediaType,
                        response.Content.Headers.ContentLength,
                        fileStream);

                    return Result.OK((TResponse)(object)responseStream);
                }

                // Deserialize successful response
                TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>(WebOptions, cancellationToken).ConfigureAwait(false);

                return responseData is null
                    ? result.WithStatusCode((StatusCode)response.StatusCode)
                    : result.OK(responseData).WithStatusCode((StatusCode)response.StatusCode);
            }

            if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
            {
                // Handle error response
                await result.ReadProblemDetailsAsync(response).ConfigureAwait(false);
                return result;
            }

            string errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            result.WithDetail(errorContent)
                .WithStatusCode((StatusCode)response.StatusCode);
        }
        catch (Exception ex)
        {
            return result.InternalServerError().WithDetail(ex.Message);
        }

        return result;
    }
}