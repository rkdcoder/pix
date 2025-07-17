# Pix – Geração de Payload Copia e Cola para Pix (.NET 6+)

![NuGet](https://img.shields.io/nuget/v/Pix.Rkd)

Gere facilmente o payload Pix (“Copia e Cola”) pronto para uso em pagamentos instantâneos, com validação automática dos dados e API simples. Não gera imagens QR Code – apenas o texto do Pix, conforme padrão Bacen.

---

## Como Usar

```csharp
using Pix;

var result = PixGenerator.GeneratePayload(
    "chave@pix.com.br",
    "JOAO DA SILVA",
    150.75m,
    "CURITIBA",
    "PEDIDO123",
    "Obrigado pela preferência!"
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

## Parâmetros Disponíveis

- `chavePix` — Chave Pix (obrigatório)
- `nomeFavorecido` — Nome do recebedor (obrigatório)
- `valor` — Valor (obrigatório)
- `cidade` — Cidade do recebedor (obrigatório)
- `identificador` — Identificador da transação (obrigatório)
- `mensagem` — Mensagem ao destinatário (opcional)
- `moeda` — Código da moeda (opcional, padrão: 986)
- `pais` — País (opcional, padrão: BR)

---

## Retorno Padronizado

O método retorna sempre um objeto:

```json
{
  "success": true,
  "message": "Payload do Pix gerado com sucesso",
  "payload": "00020126..."
}
```

Se houver erro de validação:

```json
{
  "success": false,
  "message": "Descrição detalhada do erro",
  "payload": null
}
```

---

## Validações Aplicadas

- Chave Pix: obrigatória, até 77 caracteres
- Nome: obrigatório, até 25 caracteres
- Valor: maior que zero
- Cidade: obrigatória, até 15 caracteres
- Identificador (TxId): obrigatório, até 25 caracteres, sem acentos/especiais
- Mensagem: opcional, até 35 caracteres
- Moeda: padrão ISO 4217 (3 dígitos), default 986
- País: padrão ISO 3166-1 alpha-2 (2 letras), default BR

---

## Observações

- **Não gera imagens QR Code, apenas o texto Copia e Cola do Pix.**
- Compatível com .NET 6 ou superior.
- Todas as respostas e mensagens em português.
- Totalmente validado conforme padrão Bacen.

---

## Licença

MIT
