# Tolitech.HttpClients.Abstractions

**Tolitech.HttpClients.Abstractions** provides contracts and abstractions to facilitate the creation of HTTP clients in .NET applications. It defines essential interfaces to standardize HTTP communication, making API integration simpler, safer, and more extensible.

## Main Interfaces

- `IHttpClient`: Marks classes that represent HTTP clients.
- `IRequest`: Base contract for request objects sent to HTTP endpoints.
- `IResponse`: Base contract for response objects received from HTTP endpoints.

## Purpose

Standardize HTTP operations, promoting reuse, testability, and consistent integration between different HTTP client implementations.

## Usage Example

```csharp
public class MyRequest : IRequest
{
    public string Name { get; set; }
}

public class MyResponse : IResponse
{
    public string Message { get; set; }
}
```