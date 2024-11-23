using EntitiesLayer.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class AuctionImageRepository : Repository<AuctionImage> {
        

        public IQueryable<AuctionImage> GetForAuction(int auctionId) {
            var query = from e in Entities
                        where e.AuctionId == auctionId
                        select e;
            return query;
        }

        public void DeleteAllImagesForAuction(int auctionId) {
            var images = GetForAuction(auctionId).ToList();
            foreach(var img in images) {
                Remove(img, false);
            }
            SaveChanges();
        }

    }
}
