using EntitiesLayer.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class RegionRepository : Repository<Region> {

        public override IQueryable<Region> GetAll() {
            var query = from e in Entities
                        where e.RegionId != Region.UnknownRegion
                        select e;
            return query;
        }

        public override int Update(Region entity, bool save = true) {

            var region = Entities.SingleOrDefault((e) => e.RegionId == entity.RegionId);
            region.Name = entity.Name;

            if (save) return SaveChanges();
            return 0;
        }

    }
}
