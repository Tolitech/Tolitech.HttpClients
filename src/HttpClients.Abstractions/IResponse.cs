namespace Tolitech.HttpClients.Abstractions;

/// <summary>
/// Represents the base contract for HTTP response objects.
/// Classes implementing this interface are used to encapsulate
/// the data received from an HTTP endpoint.
/// </summary>
/// <remarks>
/// This interface can be extended to include additional metadata
/// or properties specific to a response structure, such as status codes
/// or response-specific validations.
/// </remarks>
public interface IResponse;