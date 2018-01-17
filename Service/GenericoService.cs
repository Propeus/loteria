using Repository;
using System;
using System.Data.Entity;

namespace Service
{
    /// <summary>
    /// Serviço genérico com um repositório genérico
    /// </summary>
    public class GenericoService : IDisposable
    {
        /// <summary>
        /// Repositório genérico
        /// </summary>
        public GenericoRepository Repository { get; protected set; }

        /// <summary>
        /// Instacia do serviço criando um novo contexto para o repositório
        /// </summary>
        public GenericoService()
        {
            Repository = new GenericoRepository();
        }

        /// <summary>
        /// Instacia do serviço utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public GenericoService(LotericaEntities repositoryFactory)
        {
            Repository = new GenericoRepository(repositoryFactory);
        }

        public void Dispose()
        {
            ((IDisposable)Repository).Dispose();
        }
    }

    /// <summary>
    /// Serviço genérico com um repositório <see cref="TRepository"/>
    /// </summary>
    public class GenericoService<TRepository> : GenericoService where TRepository : GenericoRepository
    {
        /// <summary>
        /// Repositório <see cref="TRepository"/>
        /// </summary>
        public new TRepository Repository => (TRepository)base.Repository;

        /// <summary>
        /// Instacia do serviço criando um novo contexto para o repositório <see cref="TRepository"/>
        /// </summary>
        public GenericoService()
        {
            base.Repository = Activator.CreateInstance<TRepository>();
        }

        /// <summary>
        /// Instacia do serviço utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public GenericoService(DbContext repositoryFactory)
        {
            base.Repository = Activator.CreateInstance(typeof(TRepository), repositoryFactory) as TRepository;
        }
    }
}
