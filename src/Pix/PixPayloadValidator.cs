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
        "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AZ",
        "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ", "BR",
        "BS", "BT", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM",
        "CN", "CO", "CR", "CU", "CV", "CW", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ",
        "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "GA", "GB",
        "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GU", "GT",
        "GW", "GY", "HK", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IR",
        "IS", "IT", "JE", "JM", "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW",
        "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC",
        "MD", "ME", "MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT",
        "MU", "MV", "MW", "MY", "MX", "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP",
        "NR", "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS",
        "PT", "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SB", "SH", "SA", "SC", "SD", "SE",
        "SG", "SI", "SK", "SL", "SM", "SN", "SO", "SR", "SS", "ST", "SV", "SX", "SY", "SZ", "TC",
        "TD", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TR", "TT", "TW", "TV", "TZ",
        "UA", "UG", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VN", "VU", "VI", "WF", "WS", "XK",
        "YE", "YT", "ZA", "ZM", "ZW"
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