using System.Text.RegularExpressions;

namespace Pix
{
    /// <summary>
    /// Validações dos parâmetros do Pix.
    /// </summary>
    internal static class PixPayloadValidator
    {
        private static readonly HashSet<string> _paises = new()
        {
            "BR", "US", "AR", "DE", "FR", "JP", "CN", "IT", "GB", "ES", "PT", "MX", "CA", "AU", "CH",
            // Adicione outros se quiser mais países válidos
        };

        internal static void Validate(
            string chavePix,
            string nomeFavorecido,
            decimal valor,
            string cidade,
            string identificador,
            string? mensagem,
            string moeda,
            string pais)
        {
            if (string.IsNullOrWhiteSpace(chavePix) || chavePix.Length > 77)
                throw new ArgumentException("A chave Pix é obrigatória e deve ter até 77 caracteres.", nameof(chavePix));

            if (string.IsNullOrWhiteSpace(nomeFavorecido) || nomeFavorecido.Length > 25)
                throw new ArgumentException("O nome do favorecido é obrigatório e deve ter até 25 caracteres.", nameof(nomeFavorecido));

            if (valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.", nameof(valor));

            if (string.IsNullOrWhiteSpace(cidade) || cidade.Length > 15)
                throw new ArgumentException("A cidade é obrigatória e deve ter até 15 caracteres.", nameof(cidade));

            if (string.IsNullOrWhiteSpace(identificador) || identificador.Length > 25)
                throw new ArgumentException("O identificador é obrigatório e deve ter até 25 caracteres.", nameof(identificador));

            if (!Regex.IsMatch(identificador, "^[a-zA-Z0-9]+$"))
                throw new ArgumentException("O identificador só pode conter letras e números (sem acentos).", nameof(identificador));

            if (mensagem != null && mensagem.Length > 35)
                throw new ArgumentException("A mensagem ao destinatário pode ter até 35 caracteres.", nameof(mensagem));

            if (string.IsNullOrWhiteSpace(moeda) || !int.TryParse(moeda, out _) || moeda.Length != 3)
                throw new ArgumentException("A moeda deve ser um código ISO 4217 válido (ex: '986').", nameof(moeda));

            if (string.IsNullOrWhiteSpace(pais) || pais.Length != 2 || !_paises.Contains(pais.ToUpperInvariant()))
                throw new ArgumentException("A sigla do país deve ter 2 letras, conforme ISO 3166-1 alpha-2 (ex: 'BR').", nameof(pais));
        }
    }
}