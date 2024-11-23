using EntitiesLayer.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class CategoryRepository : Repository<Category> {

        public override IQueryable<Category> GetAll() {
            var query = from e in Entities
                        where e.CategoryId != Category.UnknownCategory
                        select e;
            return query;
        }

        public IQueryable<Category> GetRootCategories() {
            var query = from e in Entities
                        where e.CategoryId != Category.UnknownCategory && e.ParentId == null
                        select e;
            return query;
        }

        public override int Update(Category entity, bool save = true) {

            var category = Entities.SingleOrDefault((e) => e.CategoryId == entity.CategoryId);
            category.Name = entity.Name;
            category.ParentId = entity.ParentId;

            if (save) return SaveChanges();
            return 0;

        }

    }
}
