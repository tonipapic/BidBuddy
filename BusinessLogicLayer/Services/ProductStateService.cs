using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services {
    /// <summary>
    /// <remarks>Josip Mojzeš</remarks>
    /// </summary>
    public class ProductStateService {

        public ProductStateService() {}

        public List<ProductState> GetAllStates() {
            using (var repo = new ProductStateRepository()) {
                return repo.GetAll().ToList();
            }
        }

    }
}
