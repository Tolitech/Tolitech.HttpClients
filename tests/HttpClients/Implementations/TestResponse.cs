using Tolitech.HttpClients.Abstractions;

namespace Tolitech.HttpClients.UnitTests.Implementations;

/// <summary>
/// Represents a test implementation of the <see cref="IResponse"/> interface
/// for unit testing HTTP client responses.
/// </summary>
internal sealed class TestResponse : IResponse
{
    /// <summary>
    /// Gets or sets the result value returned by the test response.
    /// </summary>
    public string Result { get; set; } = "ok";
}