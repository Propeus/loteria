using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SorteioRepository : GenericoRepository<Sorteios>
    {
        public SorteioRepository(DbContext repositoryFactory) : base(repositoryFactory)
        {
        }

        public SorteioRepository()
        {
        }

        public Sorteios ObterSorteioPorId(int Id)
        {
            return Recuperar(x => x.Id == Id);
        }

        public IQueryable<Sorteios> RecuperarPorMes(int Mes)
        {
            return RecuperarVarios(x => x.DataSorteio.Month == Mes);
        }

        public IQueryable<Sorteios> RecuperarPorAno(int Ano)
        {
            return RecuperarVarios(x => x.DataSorteio.Year == Ano);
        }

        public IQueryable<Sorteios> RecuperarPorMesAno(int Mes, int Ano)
        {
            return RecuperarVarios(x => x.DataSorteio.Month == Mes && x.DataSorteio.Year == Ano);
        }
    }
}
