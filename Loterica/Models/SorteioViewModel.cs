using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loteria.Models
{


    public class SorteioViewModel
    {
        public Usuarios Usuario { get; set; }
        public Sorteios Sorteio { get; set; }

        public List<Sorteios> Sorteios { get; set; }
    }
}