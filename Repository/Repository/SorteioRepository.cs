using System.Data.Entity;
using System.Linq;

namespace Repository
{
    /// <summary>
    /// Repositório para a entidade <see cref="Sorteios"/>
    /// </summary>
    public class SorteioRepository : GenericoRepository<Sorteios>
    {
        /// <summary>
        /// Instacia do repositório criando um novo contexto
        /// </summary>
        public SorteioRepository()
        {
        }
        
        /// <summary>
        /// Instacia do repositório utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public SorteioRepository(LotericaEntities repositoryFactory) : base(repositoryFactory)
        {
        }


        /// <summary>
        /// Obtem um objeto do tipo <see cref="Sorteios"/> utilizando o <see cref="Sorteios.Id"/>.
        /// </summary>
        /// <param name="Id">Id da <see cref="Sorteios"/></param>
        /// <returns><see cref="Sorteios"/></returns>
        public Sorteios ObterSorteioPorId(int Id)
        {
            return Recuperar(x => x.Id == Id);
        }

        /// <summary>
        /// Obtem um conjunto de objetos do tipo <see cref="Sorteios"/> utilizando o mês do sorteio.
        /// </summary>
        /// <param name="Mes">Mês do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public IQueryable<Sorteios> RecuperarPorMes(int Mes)
        {
            return RecuperarVarios(x => x.DataSorteio.Month == Mes);
        }

        /// <summary>
        /// Obtem um conjunto de objetos do tipo <see cref="Sorteios"/> utilizando o ano do sorteio.
        /// </summary>
        /// <param name="Ano">Ano do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public IQueryable<Sorteios> RecuperarPorAno(int Ano)
        {
            return RecuperarVarios(x => x.DataSorteio.Year == Ano);
        }

        /// <summary>
        /// Obtem um conjunto de objetos do tipo <see cref="Sorteios"/> utilizando o mês e ano do sorteio.
        /// </summary>
        /// <param name="Mes">Mês do sorteio</param>
        /// <param name="Ano">Ano do sorteio</param>
        /// <returns><see cref="Sorteios"/></returns>
        public IQueryable<Sorteios> RecuperarPorMesAno(int Mes, int Ano)
        {
            return RecuperarVarios(x => x.DataSorteio.Month == Mes && x.DataSorteio.Year == Ano);
        }
    }
}
