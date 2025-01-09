using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Tolitech.HttpServices.Abstractions;
using Tolitech.Results;
using Tolitech.Results.Http;

namespace Tolitech.HttpServices;

/// <summary>
/// Base class for making HTTP service requests.
/// </summary>
/// <param name="httpClient">The HTTP client used to send requests.</param>
public abstract class BaseHttpService(HttpClient httpClient)
{
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
                JsonSerializer.Serialize(request, JsonSerializerOptions.Web),
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
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize successful response
                TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken).ConfigureAwait(false);

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