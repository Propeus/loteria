using Repository.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Repository
{
    [MetadataType(typeof(UsuariosMetadata))]
    public partial class Usuarios
    {
        public string ConfirmarSenha { get; set; }
    }
}
