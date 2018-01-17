using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Models
{
    public class PesquisaViewModel
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int Acertos { get; set; }
    }
}