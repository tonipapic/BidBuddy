using EntitiesLayer.Entities;
using EntitiesLayer.Models;

namespace BusinessLogicLayer.Payment {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public interface IBank {

        void MakeCardPayment(Auction auction, AuctionBid selectedBid, CardPaymentInfo info);

    }
}
