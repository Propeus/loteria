using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GenericoService
    {
        public GenericoRepository Repository { get; protected set; }

        public GenericoService()
        {
            Repository = new GenericoRepository();
        }

        public GenericoService(DbContext repositoryFactory)
        {
            Repository = new GenericoRepository(repositoryFactory);
        }
    }

    public class GenericoService<TRepository> : GenericoService where TRepository : GenericoRepository
    {
        public new TRepository Repository => (TRepository)base.Repository;

        public GenericoService()
        {
            base.Repository = Activator.CreateInstance<TRepository>();
        }

        public GenericoService(DbContext repositoryFactory)
        {
            base.Repository = Activator.CreateInstance(typeof(TRepository), repositoryFactory) as TRepository;
        }
    }
}
