using Repository.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    [MetadataType(typeof(UsuariosMetadata))]
    public partial class Usuarios
    {
        public string ConfirmarSenha { get; set; }
    }
}
