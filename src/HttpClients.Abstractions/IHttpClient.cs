namespace Tolitech.HttpClients.Abstractions;

/// <summary>
/// Defines the contract for HTTP client implementations.
/// This interface serves as a marker for classes that handle HTTP communication.
/// Typically, implementations of this interface provide methods to perform HTTP operations
/// such as GET, POST, PUT, DELETE, and PATCH.
/// </summary>
public interface IHttpClient
{
    /// <summary>
    /// Gets the base address of the HTTP client used for requests.
    /// </summary>
    Uri? BaseAddress { get; }
}