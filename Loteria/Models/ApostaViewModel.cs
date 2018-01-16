using Repository;
using Service.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loteria.Models
{


    public class ApostasViewModel
    {
        public Usuarios Usuario { get; set; }
        public Sorteios Sorteio { get; set; }
        public Apostas Aposta { get; set; }

        public List<ResultadoAposta> Apostas { get; set; }
    }
}