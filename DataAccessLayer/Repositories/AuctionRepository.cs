using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {
    public class AuctionRepository : Repository<Auction> {

        public AuctionRepository() { }

        public override int Update(Auction entity, bool save = true) {

            var auction = Entities.SingleOrDefault((e) => e.AuctionId == entity.AuctionId);
            auction.Name = entity.Name;
            auction.Description = entity.Description;
            auction.CreationDate = entity.CreationDate;
            auction.EndDate = entity.EndDate;
            auction.MinimalBidPrice = entity.MinimalBidPrice;
            auction.InstantBuyPrice = entity.InstantBuyPrice;
            auction.ProductStateId = entity.ProductStateId;
            auction.CreatorId = entity.CreatorId;
            auction.RegionId = entity.RegionId;
            auction.CategoryId = entity.CategoryId;
            auction.AuctionStateId = entity.AuctionStateId;

            if (save) return SaveChanges();
            return 0;
        }

        public List<Auction> GetAuctionsByUserId(int userId)
        {
            return Entities.Where(a => a.CreatorId == userId)
                .Include("Region")
                .Include("Category")
                .Include("AuctionState")
                .Include("ProductState")
                .Include("User")
                .Include("AuctionBids")
                .Include(p=>p.AuctionImages)
                .Include(path=>path.AuctionBids.Select(p=>p.User))
                .ToList();
        }

        public override int Remove(Auction entity, bool save = true) {
            var auction = Entities.SingleOrDefault((e) => e.AuctionId == entity.AuctionId);
            Entities.Remove(auction);

            if (save) return SaveChanges();
            return 0;
        }

        public void InstantBuy(int auctionId)
        {
            Entities.First(p => p.AuctionId == auctionId).AuctionStateId = AuctionState.Sold;
            Context.SaveChanges();
        }

    }
}
