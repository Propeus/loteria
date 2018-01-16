﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Metadata
{
   public class SorteiosMetadata
    {
        [Required(ErrorMessage ="É obrigatório preencher os numeros do sorteio")]
        public long NumeroSorteio { get; set; }
        public string NumeroSorteioExibicao { get; set; }
        [Range(1,999999999,ErrorMessage ="É obrigatório o valor do premio")]
        [Required(ErrorMessage = "É obrigatório o valor do premio")]
        public double ValorPremio { get; set; }
    }
}