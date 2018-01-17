using System.ComponentModel.DataAnnotations;

namespace Repository.Metadata
{
    /// <summary>
    /// Metadados da entidade <see cref="Apostas"/>
    /// </summary>
    public partial class ApostasMetadata
    {
        [Display(Name ="Numeros da aposta")]
        [Required(ErrorMessage ="É obrigatório preencher a aposta")]
        public int NumeroAposta { get; set; }
    }
}
