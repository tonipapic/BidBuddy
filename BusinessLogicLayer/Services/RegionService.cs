
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services {
    public class RegionService {
        /// <summary>
        /// <remarsk>Josip Mojzeš</remarsk>
        /// </summary>
        public RegionService() {}

        public List<Region> GetAllRegions() {
            using(var repo = new RegionRepository()) {
                return repo.GetAll().ToList();
            }
        }

        public void AddRegion(Region region) {
            using (var repo = new RegionRepository()) {
                repo.Add(region);
            }
        }

        public void UpdateRegion(Region region) {
            using (var repo = new RegionRepository()) {
                repo.Update(region);
            }
        }

        public void DeleteRegion(Region region) {
            using (var repo = new RegionRepository()) {
                repo.Remove(region);
            }
        }

    }
}
