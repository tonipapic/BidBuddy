using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer.Entities
{
    public partial class Auction
    {
        public decimal GetCurrentBid()
        {
            if (AuctionBids.Count == 0)
            {
                return MinimalBidPrice;
            }
            return AuctionBids.OrderBy(x => x.Value).FirstOrDefault().Value;
        }

        public static bool IsAuctionOver(Auction auction)
        {
            return DateTime.Now.Subtract(auction.EndDate).TotalMilliseconds < 0;
        }

        public static string TimeLeft(Auction auction)
        {
            string text = "Aukcija gotova";
            var timeLeft = DateTime.Now.Subtract(auction.EndDate);
            if(timeLeft.TotalMilliseconds > 0)
            {
                if(timeLeft.Days > 0)
                {
                    text = $"{timeLeft.Days} dan(a)";
                }
                else
                {
                   text = timeLeft.ToString("hh:mm:ss");
                }
            }
            return text;
        }
    }
}
