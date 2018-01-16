using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsuariosRepository : GenericoRepository<Usuarios>
    {
        public UsuariosRepository()
        {
        }

        public UsuariosRepository(DbContext repositoryFactory) : base(repositoryFactory)
        {
        }

        public Usuarios RecuperarPorId(int Id)
        {
           return Recuperar(x => x.Id == Id);
        }
    }
}
