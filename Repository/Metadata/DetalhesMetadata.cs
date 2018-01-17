using System.ComponentModel.DataAnnotations;

namespace Repository.Metadata
{
    /// <summary>
    /// Metadados da entidade <see cref="Detalhes"/>
    /// </summary>
    public partial class DetalhesMetadata
    {
        [Required(ErrorMessage ="O campo e-mail é obrigatório")]
        [Display(Name ="E-mail")]
        [EmailAddress(ErrorMessage ="Endereço de e-mail inválido")]
        public string E_Mail { get; set; }
    }
}
