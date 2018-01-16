using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Metadata
{

    public partial class UsuariosMetadata
    {
        [StringLength(50, ErrorMessage = "O tamanho do nome deve ser menor que 50 caracteres")]
        [Required(ErrorMessage = "É obrigatório um nome de usuario")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "É obrigatório uma senha")]
        public string Senha { get; set; }
        [Display(Name = "Repita a senha")]
        [Compare("Senha", ErrorMessage = "As senhas não são iguais.")]
        public string ConfirmarSenha { get; set; }
    }
}
