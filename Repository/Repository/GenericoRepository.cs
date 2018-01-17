using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    /// <summary>
    /// Permite conectar em qualquer <see cref="DbContext"/> e executar operações básicas como inserção, remoção e edição
    /// </summary>
    public class GenericoRepository : IDisposable
    {
        public LotericaEntities RepositoryFactory { get; protected set; }
        private bool Disposed { get; set; } = false;

        /// <summary>
        /// Instacia do repositório criando um novo contexto
        /// </summary>
        public GenericoRepository()
        {
            RepositoryFactory = new LotericaEntities();
            RepositoryFactory.Database.Connection.Open();
        }

        /// <summary>
        /// Instacia do repositório utilizando um contexto já existente
        /// </summary>
        /// <param name="repositoryFactory">Contexto de algum repositório ja instanciado</param>
        public GenericoRepository(LotericaEntities repositoryFactory)
        {
            this.RepositoryFactory = repositoryFactory;
        }

        /// <summary>
        /// Insere um objeto entidade do entity framework
        /// </summary>
        public virtual void Inserir(object entity)
        {
            RepositoryFactory.Set(entity.GetType()).Add(entity);
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Edita um objeto entidade do entity framework
        /// </summary>
        public virtual void Editar(object entity)
        {
            RepositoryFactory.Entry(entity).State = EntityState.Modified;
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Edita um conjunto de objetos de entidade do entity framework
        /// </summary>
        public virtual void EditarLote(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                RepositoryFactory.Entry(entity).State = EntityState.Modified;
            }
            RepositoryFactory.SaveChanges();
        }
        /// <summary>
        /// Remove um objeto entidade do entity framework
        /// </summary>
        public virtual void Remover(object entity)
        {
            RepositoryFactory.Set(entity.GetType()).Remove(entity);
            RepositoryFactory.SaveChanges();
        }

        public void Dispose()
        {
            if (!Disposed)
            {

                if (RepositoryFactory != null)
                {
                    RepositoryFactory.Dispose();
                    RepositoryFactory = null;

                }

                Disposed = true;
            }
        }
    }

    /// <summary>
    /// Permite conectar em qualquer <see cref="LotericaEntities"/> usando o <see cref="DbSet"/> com o tipo <see cref="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Qualquer classe mapeado pelo edmx</typeparam>
    public class GenericoRepository<TEntity> : GenericoRepository where TEntity : class
    {
        public GenericoRepository(LotericaEntities repositoryFactory) : base(repositoryFactory)
        {
        }

        public GenericoRepository()
        {
        }
        /// <summary>
        /// Insere um objeto entidade do entity framework
        /// </summary>
        public virtual void Inserir(TEntity entity)
        {
            RepositoryFactory.Set<TEntity>().Add(entity);
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Edita um objeto entidade do entity framework
        /// </summary>
        public virtual void Editar(TEntity entity)
        {
            RepositoryFactory.Entry<TEntity>(entity).State = EntityState.Modified;
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Edita um conjunto de objetos de entidade do entity framework
        /// </summary>
        public virtual void EditarLote(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                RepositoryFactory.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Remove um objeto entidade do entity framework
        /// </summary>
        public virtual void Remover(TEntity entity)
        {
            RepositoryFactory.Set<TEntity>().Remove(entity);
            RepositoryFactory.SaveChanges();
        }

        /// <summary>
        /// Obtem um objeto do tipo <see cref="TEntity"/> baseado em uma expressão
        /// </summary>
        public virtual TEntity Recuperar(Expression<Func<TEntity, bool>> expression)
        {
            return RepositoryFactory.Set<TEntity>().FirstOrDefault(expression);
        }

        /// <summary>
        /// Obtem um conjunto de objetos do tipo <see cref="TEntity"/> baseado em uma expressão
        /// </summary>
        public virtual IQueryable<TEntity> RecuperarVarios(Expression<Func<TEntity, bool>> expression)
        {
            return RepositoryFactory.Set<TEntity>().Where(expression);
        }

        /// <summary>
        /// Obtem um todos os objetos do tipo <see cref="TEntity"/>
        /// </summary>
        public virtual IQueryable<TEntity> RecuperarTodos()
        {
            return RepositoryFactory.Set<TEntity>();
        }
    }
}
