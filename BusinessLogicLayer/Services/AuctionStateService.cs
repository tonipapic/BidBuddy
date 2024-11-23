using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services {
    /// <summary>
    /// <remarks>Josip Mojzeš</remarks>
    /// </summary>
    public class AuctionStateService {

        public List<AuctionState> GetAllStates() {
            using (var repo = new AuctionStateRepository()) {
                return repo.GetAll().ToList();
            }
        }

        public AuctionState GetStateById(int auctionStateId) {
            using (var repo = new AuctionStateRepository()) {
                return repo.GetStateById(auctionStateId);
            }
        }

    }
}
