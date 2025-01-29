namespace Tolitech.HttpClients.Abstractions;

/// <summary>
/// Represents the base contract for HTTP request objects.
/// Classes implementing this interface are used to encapsulate
/// the data sent to an HTTP endpoint in the body of a request.
/// </summary>
/// <remarks>
/// This interface can be extended to include additional metadata
/// or properties specific to a request structure, such as validation rules.
/// </remarks>
public interface IRequest;