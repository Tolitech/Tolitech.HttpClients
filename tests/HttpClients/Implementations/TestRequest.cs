using Tolitech.HttpClients.Abstractions;

namespace Tolitech.HttpClients.UnitTests.Implementations;

/// <summary>
/// Represents a test implementation of the <see cref="IRequest"/> interface
/// for unit testing HTTP client requests.
/// </summary>
internal class TestRequest : IRequest
{
    /// <summary>
    /// Gets or sets the data value sent by the test request.
    /// </summary>
    public string Data { get; set; } = "test";
}