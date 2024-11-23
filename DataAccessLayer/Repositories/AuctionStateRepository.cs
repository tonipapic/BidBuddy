using EntitiesLayer.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class AuctionStateRepository : Repository<AuctionState> {

        public AuctionState GetStateById(int auctionStateId) {
            return Entities.FirstOrDefault((e) => e.AuctionStateId == auctionStateId);
        }

    }
}
