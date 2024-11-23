using EntitiesLayer.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {

    /// <summary>
    /// Base repository class.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Repository<T> : IDisposable where T : class {

        protected DatabaseContext Context { get; set; }
        protected DbSet<T> Entities { get; set; }

        public Repository() {
            Context = new DatabaseContext();
            Entities = Context.Set<T>();
        }

        public virtual IQueryable<T> GetAll() {
            var query = from e in Entities
                        select e;
            return query;
        }

        public virtual int Add(T entity, bool save = true) {
            Entities.Add(entity);
            if (save) return SaveChanges();
            return 0;
        }

        public virtual int Update(T entity, bool save = true) {
            throw new NotImplementedException("Metoda za ažuriranje nije implementirana!");
        }

        public virtual int Remove(T entity, bool save = true) {
            Entities.Attach(entity);
            Entities.Remove(entity);
            if (save) return SaveChanges();
            return 0;
        }

        public virtual int SaveChanges() {
            return Context.SaveChanges();
        }

        public virtual Task<int> AddAsync(T entity) {
            Entities.Add(entity);
            return SaveChangesAsync();
        }

        public virtual Task<int> UpdateAsync(T entity) {
            throw new NotImplementedException("Metoda za ažuriranje nije implementirana!");
        }

        public virtual Task<int> RemoveAsync(T entity) {
            Entities.Attach(entity);
            Entities.Remove(entity);
            return SaveChangesAsync();
        }

        public virtual Task<int> SaveChangesAsync() {
            return Context.SaveChangesAsync();
        }

        public virtual void Dispose() {
            Context.Dispose();
        }
    }

}
