# Pix

## Pix Nuget: Gerador de Payload para QrCode Pix (Copia e Cola) – .NET 6+

[![Nuget](https://img.shields.io/nuget/v/Pix)](https://www.nuget.org/packages/Pix)

> **Gere payloads Pix “Copia e Cola” de forma segura, validada e simples em aplicações .NET. Não gera imagens, apenas o texto pronto para pagamento!**

---

## Índice

- [Visão Geral](#visão-geral)
- [Instalação](#instalação)
- [Exemplo de Uso](#exemplo-de-uso)
- [API](#api)
- [Resposta Padrão](#resposta-padrão)
- [Validações](#validações)
- [Internacionalização](#internacionalização)
- [Testes e Cobertura](#testes-e-cobertura)
- [Limitações](#limitações)
- [Roadmap](#roadmap)
- [Licença](#licença)

---

## Visão Geral

O **Pix** é um pacote Nuget para geração do payload textual do Pix (“Copia e Cola”), seguindo o padrão do Banco Central do Brasil. Desenvolvido para ser simples, validado, direto, e seguro.

- Compatível com .NET 6 ou superior.
- Gera somente o texto do payload Pix, pronto para colar no app do banco ou gerar QR Code.
- **Não gera imagens**.
- Validação automática conforme Bacen.
- API enxuta e fácil de integrar.

---

## Instalação

Via Nuget Package Manager:

```
Install-Package Pix
```

Ou via .NET CLI:

```
dotnet add package Pix
```

---

## Exemplo de Uso

```csharp
using Pix;

var result = PixGenerator.GeneratePayload(
    "abc@pix.com.br",
    "JOAO DA SILVA",
    123.45m,
    "SAO PAULO",
    "PEDIDO123",
    "Obrigado!"
);

if (result.Success)
{
    Console.WriteLine(result.Payload); // "00020126..."
}
else
{
    Console.WriteLine("Erro: " + result.Message);
}
```

---

## API

Método principal:

```csharp
PixPayloadResult PixGenerator.GeneratePayload(
    string chavePix,
    string nomeFavorecido,
    decimal valor,
    string cidade,
    string identificador,
    string? mensagem = null,
    string moeda = "986",
    string pais = "BR"
)
```

- Parâmetros obrigatórios: `chavePix`, `nomeFavorecido`, `valor`, `cidade`, `identificador`
- Parâmetros opcionais: `mensagem`, `moeda` (padrão 986), `pais` (padrão BR)

---

## Resposta Padrão

```json
{
  "success": true,
  "message": "Payload do Pix gerado com sucesso",
  "payload": "00020126..."
}
```

Em caso de erro de validação:

```json
{
  "success": false,
  "message": "Descrição detalhada do erro",
  "payload": null
}
```

---

## Validações

Validações automáticas conforme especificação Bacen:

- Chave Pix: obrigatória, até 77 caracteres
- Nome: obrigatório, até 25 caracteres
- Valor: maior que zero
- Cidade: obrigatória, até 15 caracteres
- Identificador (TxId): obrigatório, até 25 caracteres, sem acentos/especiais
- Mensagem: opcional, até 35 caracteres
- Moeda: padrão ISO 4217 (3 dígitos), default 986
- País: padrão ISO 3166-1 alpha-2 (2 letras), default BR

Qualquer violação resulta em erro amigável na propriedade `message` da resposta.

---

## Internacionalização

As mensagens de erro e resposta padrão estão em português. O design permite futura internacionalização (ex: inglês).

---

## Testes e Cobertura

O projeto conta com testes unitários completos para todos os fluxos principais, cobrindo:

- Casos de sucesso e borda
- Todos os campos obrigatórios e limites de tamanho
- Cenários inválidos
- Concorrência

**Cobertura mínima recomendada: 90%.**

Para rodar os testes:

```
dotnet test
```

---

## Limitações

- **Não** gera imagem QR Code, apenas o texto Copia e Cola.
- Extensões futuras (Pix com vencimento, cobrança) podem ser adicionadas mantendo o padrão de validação.
- Caso queira personalizações além do padrão Bacen, sugira via Issues.

---

## Roadmap

- Suporte a Pix Cobrança (com vencimento)
- Internacionalização completa (pt-BR, en-US)
- Plugins de validação customizada
- Exemplo de integração com APIs de bancos

Contribuições são bem-vindas!

---

## Licença

MIT. Veja o arquivo LICENSE para detalhes.
