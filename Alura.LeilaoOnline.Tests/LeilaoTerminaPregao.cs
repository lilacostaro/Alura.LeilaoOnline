using Alura.LeilaoOnline.Core;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, 1250, new double[] {800, 1150, 1400, 1250})]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(
            double valorDestino,
            double valorEsperado,
            double[] ofertas)
        {
            // Arranje - cénario

            IModalidadeAvaliadacao modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("van Gogh", modalidade);
            leilao.IniciaPregao();
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, ofertas[i]);
                }
                else
                {
                    leilao.RecebeLance(maria, ofertas[i]);
                }
            }


            // Act - execuçaõ do metodo sob teste
            leilao.TerminaPregao();


            // Assert
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }

        [Theory]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornamaiorValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            // Arranje - cénario

            IModalidadeAvaliadacao modalidade = new MaiorValor();
            var leilao = new Leilao("van Gogh", modalidade);
            leilao.IniciaPregao();
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2) == 0)
                {
                    leilao.RecebeLance(fulano, ofertas[i]);
                }
                else
                {
                    leilao.RecebeLance(maria, ofertas[i]);
                }
            }


            // Act - execuçaõ do metodo sob teste
            leilao.TerminaPregao();


            // Assert
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }
        [Fact]
        public void LancaInvalidOperationExceptionDadoPregaoNaoIniciado()
        {
            // Arranje - cénario
            var modalidade = new MaiorValor();
            var leilao = new Leilao("van Gogh", modalidade);

            // Assert
            var excecaoObtida = Assert.Throws<System.InvalidOperationException>(
                //Act - método sob teste
                () => leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possível terminar o pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().";
            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            // Arranje - cénario
            var modalidade = new MaiorValor();
            var leilao = new Leilao("van Gogh", modalidade);
            leilao.IniciaPregao();

            // Act - execuçaõ do metodo sob teste
            leilao.TerminaPregao();

            // Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
