# Tolitech.HttpClients.Abstractions

**Tolitech.HttpClients.Abstractions** oferece contratos e abstrações para facilitar a criação de clientes HTTP em aplicações .NET. O projeto define interfaces essenciais para padronizar a comunicação HTTP, tornando a integração com APIs mais simples, segura e extensível.

## Principais Interfaces

- `IHttpClient`: Marca classes que representam clientes HTTP.
- `IRequest`: Contrato base para objetos de requisição enviados a endpoints HTTP.
- `IResponse`: Contrato base para objetos de resposta recebidos de endpoints HTTP.

## Objetivo

Padronizar operações HTTP, promovendo reuso, testabilidade e integração consistente entre diferentes implementações de clientes HTTP.

## Exemplo de Uso

```csharp
public class MinhaRequisicao : IRequest
{
    public string Nome { get; set; }
}

public class MinhaResposta : IResponse
{
    public string Mensagem { get; set; }
}
```