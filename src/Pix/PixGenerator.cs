﻿using System.Globalization;
using System.Reflection;
using System.Text;

namespace Pix
{
    /// <summary>
    /// API principal para geração do payload Pix Copia e Cola.
    /// </summary>
    public static class PixGenerator
    {
        /// <summary>
        /// Gera o payload Pix Copia e Cola.
        /// </summary>
        /// <param name="chavePix">Chave Pix do favorecido</param>
        /// <param name="nomeFavorecido">Nome do favorecido</param>
        /// <param name="valor">Valor do Pix (maior que zero)</param>
        /// <param name="cidade">Cidade do favorecido</param>
        /// <param name="identificador">Identificador da transação (TxId)</param>
        /// <param name="mensagem">Mensagem ao destinatário (opcional)</param>
        /// <param name="moeda">Código da moeda (opcional, padrão: 986)</param>
        /// <param name="pais">Sigla do país (opcional, padrão: BR)</param>
        /// <returns>PixPayloadResult</returns>
        public static PixPayloadResult GeneratePayload(
            string chavePix,
            string nomeFavorecido,
            decimal valor,
            string cidade,
            string identificador,
            string? mensagem = null,
            string moeda = "986",
            string pais = "BR")
        {
            try
            {
                var chavePixNorm = chavePix.Trim(); // email, aleatória, telefone, CPF/CNPJ
                var nomeNorm = PixStringNormalizer.Normalize(nomeFavorecido);
                var cidadeNorm = PixStringNormalizer.Normalize(cidade);
                var identificadorNorm = PixStringNormalizer.Normalize(identificador);
                var mensagemNorm = mensagem != null ? PixStringNormalizer.Normalize(mensagem) : null;
                var moedaNorm = moeda.Trim();
                var paisNorm = PixStringNormalizer.Normalize(pais);

                PixPayloadValidator.Validate(
                    chavePixNorm,
                    nomeNorm,
                    valor,
                    cidadeNorm,
                    identificadorNorm,
                    mensagemNorm,
                    moedaNorm,
                    paisNorm);

                var payload = BuildPayloadString(
                    chavePixNorm,
                    nomeNorm,
                    valor,
                    cidadeNorm,
                    identificadorNorm,
                    mensagemNorm,
                    moedaNorm,
                    paisNorm);

                return new PixPayloadResult
                {
                    Success = true,
                    Message = "Payload do Pix gerado com sucesso",
                    Payload = payload
                };
            }
            catch (Exception ex)
            {
                return new PixPayloadResult
                {
                    Success = false,
                    Message = ex.Message,
                    Payload = null
                };
            }
        }

        private static string BuildPayloadString(
            string chavePix,
            string nomeFavorecido,
            decimal valor,
            string cidade,
            string identificador,
            string? mensagem,
            string moeda,
            string pais)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            string valorFormatado = valor.ToString("0.00", cultureInfo);

            var sb = new StringBuilder();

            sb.Append("000201");

            var merchantAccountInfo = new StringBuilder();
            merchantAccountInfo.Append("0014br.gov.bcb.pix");
            merchantAccountInfo.Append("01");
            merchantAccountInfo.Append(chavePix.Length.ToString("D2"));
            merchantAccountInfo.Append(chavePix);
            if (!string.IsNullOrEmpty(mensagem))
            {
                merchantAccountInfo.Append("02");
                merchantAccountInfo.Append(mensagem.Length.ToString("D2"));
                merchantAccountInfo.Append(mensagem);
            }
            string merchantAccountInfoStr = merchantAccountInfo.ToString();
            sb.Append("26");
            sb.Append(merchantAccountInfoStr.Length.ToString("D2"));
            sb.Append(merchantAccountInfoStr);

            sb.Append("52040000");
            sb.Append("5303");
            sb.Append(moeda);
            sb.Append("54");
            sb.Append(valorFormatado.Length.ToString("D2"));
            sb.Append(valorFormatado);

            sb.Append("58");
            sb.Append(pais.Length.ToString("D2"));
            sb.Append(pais);

            sb.Append("59");
            sb.Append(nomeFavorecido.Length.ToString("D2"));
            sb.Append(nomeFavorecido);

            sb.Append("60");
            sb.Append(cidade.Length.ToString("D2"));
            sb.Append(cidade);

            string txidField = "05" + identificador.Length.ToString("D2") + identificador;
            sb.Append("62");
            sb.Append(txidField.Length.ToString("D2"));
            sb.Append(txidField);

            sb.Append("6304");

            string semCrc = sb.ToString();
            string crc16 = CRC16.Compute(semCrc);

            return $"{semCrc}{crc16}";
        }
    }
}