using Repository;
using System.Collections.Generic;

namespace Loteria.Models
{


    public class ApostasViewModel
    {
        public Usuarios Usuario { get; set; }
        public Sorteios Sorteio { get; set; }
        public Apostas Aposta { get; set; }

        public List<Apostas> Apostas { get; set; }
    }
}