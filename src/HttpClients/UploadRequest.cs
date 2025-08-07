using Tolitech.HttpClients.Abstractions;

namespace Tolitech.HttpClients;

/// <summary>
/// Represents a file upload request.
/// </summary>
/// <param name="Key">The unique key for the upload.</param>
/// <param name="FileName">The name of the file being uploaded.</param>
/// <param name="ContentType">The MIME type of the file.</param>
/// <param name="FileStream">The stream containing the file's content.</param>
public record UploadRequest(
    string Key,
    string FileName,
    string ContentType,
    Stream FileStream)
        : IRequest;