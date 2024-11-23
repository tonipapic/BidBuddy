using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AuctionBidRepository : Repository<AuctionBid>
    {
        /// <summary>
        /// Sets selected property to true for a given bid
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public override int Update(AuctionBid entity, bool save = true)
        {
                var bid= Entities.Where(p => p.AuctionId == entity.AuctionId).OrderByDescending(p=>p.Value).Skip(1).First();
                bid.Selected = false;
                Context.SaveChanges();
                return 0;
        }

       
        /// <summary>
        /// Checks if bid exists for a given auction and user
        /// </summary>
        /// <param name="auctionId"></param>
        /// <param name="userId"></param>
        /// <returns> True if bid exists, otherwise false </returns>
        public async Task<bool> DoesBidExists(int auctionId, int userId)
        {
           return await Entities.AnyAsync(p=>p.AuctionId==auctionId && p.BidderId==userId);
        }
        /// <summary>
        /// Returns all bids for a given auction if auctionId is provided, otherwise returns all bids
        /// </summary>
        /// <param name="auctionId"></param>
        /// <returns> List of bids containg Bidder full name and bid value </returns>
        public List<AuctionBid> GetBids(int auctionId=0)
        {
           return auctionId!=0 ?  Entities.Where(p => p.AuctionId == auctionId).Include(p => p.User).ToList() : Entities.Include(p=>p.Auction).Include(p => p.User).ToList();
            
        }

     

    }
}
