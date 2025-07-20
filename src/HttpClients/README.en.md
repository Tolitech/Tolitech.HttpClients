# Tolitech.HttpClients

**Tolitech.HttpClients** is a generic HTTP client implementation based on the abstractions from the previous project. It provides a powerful base class (`BaseHttpClient`) to perform HTTP operations (GET, POST, PUT, PATCH, DELETE) in a structured, safe, and efficient way.

## Key Features

- Generic implementation for HTTP clients.
- Native support for JSON (serialization/deserialization).
- Custom header management.
- HTTP response validation and error handling.
- Direct integration with Tolitech.HttpClients.Abstractions contracts.

## Main Class Structure

### `BaseHttpClient`

The base class provides methods such as:

- `GetAsync<TResponse>()`: Performs a GET request.
- `PostAsync<TRequest, TResponse>()`: Performs a POST request.
- `PutAsync<TRequest, TResponse>()`: Performs a PUT request.
- `PatchAsync<TRequest, TResponse>()`: Performs a PATCH request.
- `DeleteAsync<TResponse>()`: Performs a DELETE request.

### Implementation Example

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

## Advanced Examples

### Sending POST Requests with Body and Headers

```csharp
var body = new MyRequest { Name = "John" };
var headers = new Dictionary<string, string> { { "Authorization", "Bearer token" } };

var result = await service.PostAsync<MyRequest, MyResponse>(
    "https://api.example.com/message",
    body,
    headers
);
```

### Error Handling and Custom Responses

```csharp
if (!result.IsSuccess)
{
    Console.WriteLine($"Status: {result.StatusCode}");
    Console.WriteLine($"Detail: {result.Detail}");
}
```