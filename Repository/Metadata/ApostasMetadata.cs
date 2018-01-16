using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Metadata
{
   public partial class ApostasMetadata
    {
        [Display(Name ="Numeros da aposta")]
        [Required(ErrorMessage ="É obrigatório preencher a aposta")]
        public int NumeroAposta { get; set; }
    }
}
