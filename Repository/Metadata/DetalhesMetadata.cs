using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Metadata
{
    public partial class DetalhesMetadata
    {
        [Required(ErrorMessage ="O campo e-mail é obrigatório")]
        [Display(Name ="E-mail")]
        [EmailAddress(ErrorMessage ="Endereço de e-mail inválido")]
        public string E_Mail { get; set; }
    }
}
