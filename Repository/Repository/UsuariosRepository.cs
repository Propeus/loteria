using System.Data.Entity;

namespace Repository
{
    /// <summary>
    /// Repositório para a entidade <see cref="Usuarios"/>
    /// </summary>
    public class UsuariosRepository : GenericoRepository<Usuarios>
    {
        /// <summary>
        /// Instacia do repositório criando um novo contexto
        /// </summary>
        public UsuariosRepository()
        {
        }

        /// <summary>
        /// Instacia do repositório utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public UsuariosRepository(LotericaEntities repositoryFactory) : base(repositoryFactory)
        {
        }

        /// <summary>
        /// Obtem um objeto do tipo <see cref="Usuarios"/> utilizando o <see cref="Usuarios.Id"/>.
        /// </summary>
        /// <param name="Id">Id do <see cref="Usuarios"/></param>
        /// <returns><see cref="Usuarios"/></returns>
        public Usuarios RecuperarPorId(int Id)
        {
           return Recuperar(x => x.Id == Id);
        }
    }
}
