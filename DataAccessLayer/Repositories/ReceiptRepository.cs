using EntitiesLayer.Entities;
using System;
using System.Linq;

namespace DataAccessLayer.Repositories {
    public class ReceiptRepository : Repository<Receipt> {

        public Receipt GetReceiptForAuction(int auctionId) {
            return Entities.FirstOrDefault((e) => e.AuctionId == auctionId);
        }

        public override int Update(Receipt entity, bool save = true) {
            throw new NotImplementedException();
        }

    }
}
