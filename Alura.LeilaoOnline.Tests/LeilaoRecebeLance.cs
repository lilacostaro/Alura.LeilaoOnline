using Alura.LeilaoOnline.Core;
using System.Drawing;
using System.Linq;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Theory]
        [InlineData(2, new double[] { 800, 1000})]
        [InlineData(4, new double[] { 1000, 1200, 1400, 1300 })]
        public void NaopermiteNovosLancesDadoLeilaoFinalizado(int qtEsperada, double[] ofertas)
        {
            // Arranje - cénario
            var modalidade = new MaiorValor();
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
            
            leilao.TerminaPregao();

            // Act - execuçaõ do metodo sob teste
            leilao.RecebeLance(fulano, 1200);


            // Assert
            var qtObtida = leilao.Lances.Count();
            Assert.Equal(qtEsperada, qtObtida);
        }

        [Fact]
        public void NaoAceitaProximoLancedadoMesmoClienteRealizouUltimoLance()
        {
            // Arranje - cénario
            var modalidade = new MaiorValor();
            var leilao = new Leilao("van Gogh", modalidade);
            leilao.IniciaPregao();
            var fulano = new Interessada("Fulano", leilao);
            leilao.RecebeLance(fulano, 1000);

            // Act - execuçaõ do metodo sob teste
            leilao.RecebeLance(fulano, 1200);

            // Assert
            var qtEsperada = 1;
            var qtObtida = leilao.Lances.Count();
            Assert.Equal(qtEsperada, qtObtida);
        }
    }
}
