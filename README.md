# Tolitech.HttpClients.Abstractions

**Tolitech.HttpClients.Abstractions** provides contracts and abstractions to facilitate the creation of HTTP clients. It defines the necessary interfaces to build and consume HTTP APIs in a consistent and extensible way.

## Features

- Defines fundamental interfaces for HTTP operations:
  - `IHttpClient`: Marks classes that represent HTTP clients.
  - `IRequest`: Base contract for request objects sent to an HTTP endpoint.
  - `IResponse`: Base contract for response objects received from an HTTP endpoint.

## Purpose

This project serves as a foundation for standardizing HTTP communication in applications, allowing simplified integration with concrete implementations of HTTP clients.

## Usage

Implement the provided interfaces to create custom request and response structures. A simple example:

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

# Tolitech.HttpClients

**Tolitech.HttpClients** is a practical implementation of an HTTP client based on the abstractions provided by **Tolitech.HttpClients.Abstractions**. This project contains the base class `BaseHttpClient`, which offers methods for performing HTTP operations such as GET, POST, PUT, PATCH, and DELETE.

## Features

- Generic implementation for HTTP clients.
- Built-in support for JSON requests and responses.
- Header management and response validation.
- Direct integration with the contracts from `Tolitech.HttpClients.Abstractions`.

## Main Class

### `BaseHttpClient`

`BaseHttpClient` provides methods such as:

- `GetAsync<TResponse>()`: Performs an HTTP GET request.
- `PostAsync<TRequest, TResponse>()`: Performs an HTTP POST request.
- `PutAsync<TRequest, TResponse>()`: Performs an HTTP PUT request.
- `PatchAsync<TRequest, TResponse>()`: Performs an HTTP PATCH request.
- `DeleteAsync<TResponse>()`: Performs an HTTP DELETE request.

### Example Usage

```csharp
public class MyHttpClient : BaseHttpClient
{
    public MyHttpClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<MyResponse>> GetMessageAsync(string url)
    {
        return await GetAsync<MyResponse>(url);
    }
}
```

### How to Use

```csharp
HttpClient client = new HttpClient();
var service = new MyHttpClient(client);

var result = await service.GetMessageAsync("https://api.example.com/message");

if (result.IsSuccess)
{
    Console.WriteLine(result.Value.Message);
}
else
{
    Console.WriteLine($"Error: {result.Detail}");
}
```