using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Models
{
    public class PainelModelViewModel
    {
        public List<Sorteios> Sorteios { get; set; }
        public Usuarios Usuario { get; set; }
    }
}