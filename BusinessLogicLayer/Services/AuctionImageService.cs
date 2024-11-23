using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// <remarks>Josip Mojzeš</remarks>
    /// </summary>
    public class AuctionImageService {

        public List<AuctionImage> GetImagesForAuction(int auctionId) {
            using (var repo = new AuctionImageRepository()) {
                return repo.GetForAuction(auctionId).ToList();
            }
        }

        public void AddImage(AuctionImage image) {
            using(var repo = new AuctionImageRepository()) {
                repo.Add(image);
            }
        }

        public void AddImages(IEnumerable<AuctionImage> auctionImages) {
            using (var repo = new AuctionImageRepository()) {
                foreach (var image in auctionImages) {
                    repo.Add(image);
                }
            }
        }

        public void DeleteImage(AuctionImage image) {
            using (var repo = new AuctionImageRepository()) {
                repo.Remove(image);
            }
        }

        public void DeleteAllImagesForAuction(int auctionId) {
            using (var repo = new AuctionImageRepository()) {
                repo.DeleteAllImagesForAuction(auctionId);
            }
        }

    }
}
