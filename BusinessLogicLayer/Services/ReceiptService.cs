using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;

namespace BusinessLogicLayer.Services {
    /// <summary>
    /// <remarks>Josip Mojzeš</remarks>
    /// </summary>
    public class ReceiptService {
        
        public Receipt GetReceiptForAuction(int auctionId) {
            using (var repo = new ReceiptRepository()) {
                return repo.GetReceiptForAuction(auctionId);
            }
        }

        public void GenerateReceiptForAuction(Auction auction, AuctionBid selectedBid) {

            Receipt receipt = new Receipt();

            receipt.Amount = selectedBid.Value;
            receipt.Date = DateTime.Now;
            receipt.UserId = selectedBid.BidderId;
            receipt.AuctionId = auction.AuctionId;

            AddReceipt(receipt);
        }

        private void AddReceipt(Receipt receipt) {
            using(var repo = new ReceiptRepository()) {
                repo.Add(receipt);
            }
        }

    }
}
