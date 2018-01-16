using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class ResultadoAposta
    {
        public Sorteios Sorteio { get; set; }
        public Apostas Aposta { get; set; }

        public int Acertos { get; private set; } = 0;
        public int[] NumerosAcertados { get; private set; } = new int[0];
        public double ValorPremio { get; set; }
       
        public void AvaliarAcertos()
        {
            string[] numerosSorteio = Sorteio?.NumeroSorteioExibicao.Split('-');
            if (numerosSorteio == null)
                return;

            string[] numerosAposta = Aposta.NumeroApostaExibicao.Split('-');

            LinkedList<int> numerosAcertos = new LinkedList<int>();

            for (int i = 0; i < numerosSorteio.Length; i++)
            {
                if (numerosSorteio[i] == numerosAposta[i])
                {
                    Acertos++;
                    numerosAcertos.AddLast(Convert.ToInt32(numerosAposta[i]));
                }
            }

            NumerosAcertados = numerosAcertos.ToArray();
        }
    }
}
