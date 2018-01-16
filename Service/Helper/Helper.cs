using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helper
{
    public static class Helper
    {
        public static int NumerosQuantidade { get; set; } = 6;
        public static int NumeroMinimo { get; set; } = 0;
        public static int NumeroMaximo { get; set; } = 60;


        public static int[] SortearValores(int quantidade, int valorMinimo, int valorMaximo, bool Repete = false)
        {
            LinkedList<int> numeros = new LinkedList<int>();
            int valorAux = 0;
            Random random = new Random();
            for (int i = 0; i < quantidade; i++)
            {
                do
                {
                    valorAux = random.Next(valorMinimo, valorMaximo);
                } while (numeros.Contains(valorAux) && !Repete);
                numeros.AddLast(valorAux);
            }
            return numeros.ToArray();
        }
       
    }
}
