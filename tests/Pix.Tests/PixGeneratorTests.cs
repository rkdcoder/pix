namespace Pix.Tests
{
    public class PixGeneratorTests
    {
        [Fact]
        public void Deve_Gerar_Payload_Com_Sucesso_Campos_Obrigatorios()
        {
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "TX123456"
            );
            Assert.True(result.Success);
            Assert.NotNull(result.Payload);
        }

        [Fact]
        public void Deve_Falhar_Quando_Chave_Pix_Vazia()
        {
            var result = PixGenerator.GeneratePayload(
                "",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "TX123456"
            );
            Assert.False(result.Success);
            Assert.Null(result.Payload);
            Assert.Contains("chave", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_Nome_Maior_Que_25()
        {
            var nomeGrande = new string('A', 26);
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                nomeGrande,
                10.50m,
                "CURITIBA",
                "TX123456"
            );
            Assert.False(result.Success);
            Assert.Contains("nome", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_Cidade_Maior_Que_15()
        {
            var cidadeGrande = new string('C', 16);
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                cidadeGrande,
                "TX123456"
            );
            Assert.False(result.Success);
            Assert.Contains("cidade", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_TxId_Com_Apenas_Caracteres_Invalidos()
        {
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "!@#"
            );
            Assert.False(result.Success);
            Assert.Contains("identificador", result.Message!, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_Mensagem_Maior_Que_35()
        {
            var mensagemGrande = new string('M', 36);
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "TX123456",
                mensagemGrande // parâmetro mensagem opcional
            );
            Assert.False(result.Success);
            Assert.Contains("mensagem", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_Moeda_Invalida()
        {
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "TX123456",
                null,     // mensagem
                "99"      // moeda inválida
            );
            Assert.False(result.Success);
            Assert.Contains("moeda", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Falhar_Quando_Pais_Invalido()
        {
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                10.50m,
                "CURITIBA",
                "TX123456",
                null,     // mensagem
                "986",    // moeda
                "X"       // país inválido
            );
            Assert.False(result.Success);
            Assert.Contains("sigla do país", result.Message!, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Deve_Gerar_Com_Mensagem_Opcional()
        {
            var result = PixGenerator.GeneratePayload(
                "abc@pix.com.br",
                "JOSE DA SILVA",
                99.99m,
                "CURITIBA",
                "TX123456",
                "Pagamento de serviço"
            );
            Assert.True(result.Success);
            Assert.NotNull(result.Payload);
            Assert.Contains("PAGAMENTO DE SERVICO", result.Payload!);
        }

        [Fact]
        public void Teste_Thread_Safety_GeneratePayload()
        {
            int successCount = 0;
            Parallel.For(0, 20, i =>
            {
                var result = PixGenerator.GeneratePayload(
                    "chave" + i,
                    "JOSE DA SILVA",
                    100 + i,
                    "CURITIBA",
                    "ID" + i
                );
                if (result.Success) Interlocked.Increment(ref successCount);
            });
            Assert.Equal(20, successCount);
        }
    }
}