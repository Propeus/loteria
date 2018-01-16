using System;
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
