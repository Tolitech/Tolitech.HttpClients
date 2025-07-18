# Tolitech.HttpClients

**Tolitech.HttpClients** é uma implementação genérica de cliente HTTP baseada nas abstrações do projeto anterior. Oferece uma classe base poderosa (`BaseHttpClient`) para realizar operações HTTP (GET, POST, PUT, PATCH, DELETE) de forma estruturada, segura e eficiente.

## Principais Recursos

- Implementação genérica para clientes HTTP.
- Suporte nativo a JSON (serialização/deserialização).
- Gerenciamento de headers customizados.
- Validação e tratamento de respostas HTTP.
- Integração direta com as abstrações de Tolitech.HttpClients.Abstractions.

## Estrutura da Classe Principal

### `BaseHttpClient`

A classe base oferece métodos como:

- `GetAsync<TResponse>()`: Realiza uma requisição GET.
- `PostAsync<TRequest, TResponse>()`: Realiza uma requisição POST.
- `PutAsync<TRequest, TResponse>()`: Realiza uma requisição PUT.
- `PatchAsync<TRequest, TResponse>()`: Realiza uma requisição PATCH.
- `DeleteAsync<TResponse>()`: Realiza uma requisição DELETE.

### Exemplo de Implementação

```csharp
public class MeuHttpClient : BaseHttpClient
{
    public MeuHttpClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<Result<MinhaResposta>> BuscarMensagemAsync(string url)
    {
        return await GetAsync<MinhaResposta>(url);
    }
}
```

### Como Utilizar

```csharp
HttpClient client = new HttpClient();
var service = new MeuHttpClient(client);

var resultado = await service.BuscarMensagemAsync("https://api.exemplo.com/mensagem");

if (resultado.IsSuccess)
{
    Console.WriteLine(resultado.Value.Mensagem);
}
else
{
    Console.WriteLine($"Erro: {resultado.Detail}");
}
```

## Exemplos Avançados

### Enviando Requisições POST com Body e Headers

```csharp
var body = new MinhaRequisicao { Nome = "João" };
var headers = new Dictionary<string, string> { { "Authorization", "Bearer token" } };

var resultado = await service.PostAsync<MinhaRequisicao, MinhaResposta>(
    "https://api.exemplo.com/mensagem",
    body,
    headers
);
```

### Tratamento de Erros e Respostas Customizadas

```csharp
if (!resultado.IsSuccess)
{
    Console.WriteLine($"Status: {resultado.StatusCode}");
    Console.WriteLine($"Detalhe: {resultado.Detail}");
}
```