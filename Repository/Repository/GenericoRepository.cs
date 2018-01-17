using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// Permite conectar em qualquer <see cref="DbContext"/> e executar operações básicas como inserção, remoção e edição
    /// </summary>
    public class GenericoRepository : IDisposable
    {
        public DbContext RepositoryFactory { get; protected set; }

        public GenericoRepository(DbContext repositoryFactory)
        {
            this.RepositoryFactory = repositoryFactory;
        }
        public GenericoRepository()
        {
            RepositoryFactory = new LotericaEntities();
            RepositoryFactory.Database.Connection.Open();
        }

        public virtual void Inserir(object entity)
        {
            RepositoryFactory.Set(entity.GetType()).Add(entity);
            RepositoryFactory.SaveChanges();
        }

        public virtual void Editar(object entity)
        {
            RepositoryFactory.Entry(entity).State = EntityState.Modified;
            RepositoryFactory.SaveChanges();
        }
        public virtual void EditarLote(IEnumerable<object> entities)
        {
            foreach (var entity in entities)
            {
                RepositoryFactory.Entry(entity).State = EntityState.Modified;
            }
            RepositoryFactory.SaveChanges();
        }

        public virtual void Remover(object entity)
        {
            RepositoryFactory.Set(entity.GetType()).Remove(entity);
            RepositoryFactory.SaveChanges();
        }

        public void Dispose()
        {
            RepositoryFactory.Database.Connection.Close();
            RepositoryFactory.Dispose();
        }
    }

    /// <summary>
    /// Permite conectar em qualquer <see cref="DbContext"/> usando o <see cref="DbSet"/> com o tipo <see cref="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Qualquer classe mapeado pelo edmx</typeparam>
    public class GenericoRepository<TEntity> : GenericoRepository where TEntity : class
    {
        public GenericoRepository(DbContext repositoryFactory) : base(repositoryFactory)
        {
        }

        public GenericoRepository()
        {
        }

        public virtual void Inserir(TEntity entity)
        {
            RepositoryFactory.Set<TEntity>().Add(entity);
            RepositoryFactory.SaveChanges();
        }

        public virtual void Editar(TEntity entity)
        {
            RepositoryFactory.Entry<TEntity>(entity).State = EntityState.Modified;
            RepositoryFactory.SaveChanges();
        }

        public virtual void EditarLote(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                RepositoryFactory.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            RepositoryFactory.SaveChanges();
        }

        public virtual void Remover(TEntity entity)
        {
            RepositoryFactory.Set<TEntity>().Remove(entity);
            RepositoryFactory.SaveChanges();
        }

        public virtual TEntity Recuperar(Expression<Func<TEntity, bool>> expression)
        {
            return RepositoryFactory.Set<TEntity>().FirstOrDefault(expression);
        }
        public virtual IQueryable<TEntity> RecuperarVarios(Expression<Func<TEntity, bool>> expression)
        {
            return RepositoryFactory.Set<TEntity>().Where(expression);
        }
        public virtual IQueryable<TEntity> RecuperarTodos()
        {
            return RepositoryFactory.Set<TEntity>();
        }
    }
}
