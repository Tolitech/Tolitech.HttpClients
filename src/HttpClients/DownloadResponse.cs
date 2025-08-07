using Tolitech.HttpClients.Abstractions;

namespace Tolitech.HttpClients;

/// <summary>
/// Represents the response data for a file download.
/// </summary>
/// <param name="FileName">The name of the file, if available.</param>
/// <param name="ContentType">The MIME type of the file.</param>
/// <param name="SizeInBytes">The size of the file in bytes, if known.</param>
/// <param name="FileStream">The stream containing the file's content.</param>
public record DownloadResponse(
    string? FileName,
    string? ContentType,
    long? SizeInBytes,
    Stream FileStream)
        : IResponse;