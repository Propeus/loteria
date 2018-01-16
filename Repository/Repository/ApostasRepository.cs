using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ApostasRepository : GenericoRepository<Apostas>
    {
        public ApostasRepository(DbContext repositoryFactory) : base(repositoryFactory)
        {
        }

        public ApostasRepository()
        {
        }

        public Apostas RecuerarPorId(int Id)
        {
            return Recuperar(x => x.Id == Id);
        }

        public IQueryable<Apostas> RecuperarPorUsuario(int IdUsuario)
        {
            return RecuperarVarios(x => x.IdUsuario == IdUsuario);
        }

        public IQueryable<Apostas> RecuperarPorData(DateTime dataAposta)
        {
            return RecuperarVarios(x => x.DataAposta == dataAposta);
        }

        public IQueryable<Apostas> RecuperarPorMes(int Mes)
        {
            return RecuperarVarios(x => x.DataAposta.Month == Mes);
        }

        public IQueryable<Apostas> RecuperarPorAno(int Ano)
        {
            return RecuperarVarios(x => x.DataAposta.Year == Ano);
        }

        public IQueryable<Apostas> RecuperarPorMesAno(int Mes, int Ano)
        {
            return RecuperarVarios(x => x.DataAposta.Month == Mes && x.DataAposta.Year == Ano);
        }
    }
}
