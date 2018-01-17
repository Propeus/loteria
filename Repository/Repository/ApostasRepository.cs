using System.Data.Entity;
using System.Linq;

namespace Repository
{
    /// <summary>
    /// Repositório para a entidade <see cref="Apostas"/>
    /// </summary>
    public class ApostasRepository : GenericoRepository<Apostas>
    {
        /// <summary>
        /// Instacia do repositório utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public ApostasRepository(LotericaEntities repositoryFactory) : base(repositoryFactory)
        {
        }
        /// <summary>
        /// Instacia do repositório criando um novo contexto
        /// </summary>
        public ApostasRepository()
        {
        }

        /// <summary>
        /// Obtem um objeto do tipo <see cref="Apostas"/> utilizando o <see cref="Apostas.Id"/>.
        /// </summary>
        /// <param name="Id">Id da <see cref="Apostas"/></param>
        /// <returns><see cref="Apostas"/></returns>
        public Apostas RecuerarPorId(int Id)
        {
            return Recuperar(x => x.Id == Id);
        }

        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o <see cref="Usuarios.Id"/>.
        /// </summary>
        /// <param name="IdUsuario">Id do objeto <see cref="Usuarios"/></param>
        /// <returns><see cref="Apostas"/></returns>
        public IQueryable<Apostas> RecuperarPorUsuario(int IdUsuario)
        {
            return RecuperarVarios(x => x.IdUsuario == IdUsuario);
        }

        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o mês da aposta.
        /// </summary>
        /// <param name="Mes">Mês da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public IQueryable<Apostas> RecuperarPorMes(int Mes)
        {
            return RecuperarVarios(x => x.DataAposta.Month == Mes);
        }

        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o ano da aposta.
        /// </summary>
        /// <param name="Ano">Ano da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public IQueryable<Apostas> RecuperarPorAno(int Ano)
        {
            return RecuperarVarios(x => x.DataAposta.Year == Ano);
        }

        /// <summary>
        /// Obtem um conjunto de <see cref="Apostas"/> utilizando o mês e ano da aposta.
        /// </summary>
        /// <param name="Mes">Mês da aposta</param>
        /// <param name="Ano">Ano da aposta</param>
        /// <returns><see cref="Apostas"/></returns>
        public IQueryable<Apostas> RecuperarPorMesAno(int Mes, int Ano)
        {
            return RecuperarVarios(x => x.DataAposta.Month == Mes && x.DataAposta.Year == Ano);
        }
    }
}
