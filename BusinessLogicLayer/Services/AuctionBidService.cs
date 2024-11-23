using BusinessLogicLayer.Exceptions;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {

    /// <summary>
    /// <remarks>Toni Papić, Karlo Jačmenjak</remarks>
    /// </summary>
    public class AuctionBidService
    {
       

      
        private EmailService emailService;
        private AuctionBidRepository auctionBidRepository;

        public AuctionBidService() {
            emailService = new EmailService();
            auctionBidRepository = new AuctionBidRepository();
        }
        
    


        /// <summary>
        /// Adds or updates bid for a given auction and user
        /// email is sent to a user if he is outbid
        /// </summary>
        /// <param name="auctionId"></param>
        /// <param name="value"></param>
        /// <param name="recieveEmails"></param>
        /// <remarks>Toni Papić</remarks>
        /// <returns>1 if Bid is equal or higher than Instant buy price otherwise returns -1 </returns>
        /// <exception cref="InvalidInputException"></exception>
        public int AddNewBid(Auction auction,  string value, bool recieveEmails, AuctionBid currentBid)
        {
            var userId = AuthenticationService.LoggedUser.UserId;
            if (!decimal.TryParse(value,out var bidOffer))
            {
          
                throw new InvalidInputException("Vrijednost ponude smije biti samo u brojčanom obliku (uključujući zarez)");
            }

            if (bidOffer < auction.MinimalBidPrice)
            {
                throw new InvalidInputException("Ponuda mora biti veća od početne cijene ( "+auction.MinimalBidPrice+" € )");
            }
            if (currentBid != null)
            {
                if (bidOffer <= currentBid.Value)
                {
                    throw new InvalidInputException("Ponuda mora biti veća od trenutne najveće ponude ( " + currentBid.Value + " € )");
                }
                if (currentBid.Value < bidOffer && currentBid.RecieveEmails)
                {
                    emailService.SendEmail(currentBid.User.Email, "Aukcija", "Netko je ponudio veću vrijednost za aukciju <b>" + auction.Name + "</b>, " + "te sada najveća vrijednost iznosi " + bidOffer.ToString("N0", CultureInfo.CurrentCulture) + " €");
                }
            }

            var auctionBid = new AuctionBid()
            {
                AuctionId = auction.AuctionId,
                BidderId = userId,
                Date = DateTime.Now,
                Value = bidOffer,
                RecieveEmails = recieveEmails,
                Selected=true
            };

       
               
            auctionBidRepository.Add(auctionBid);
            if(currentBid!=null)
            auctionBidRepository.Update(auctionBid);
            if (auctionBid.RecieveEmails)
                emailService.SendEmail("Dodana ponuda", "Uspješno ste dodali ponudu za aukciju <b>" + auction.Name + "</b>, " + "te sada iznosi " + bidOffer.ToString("N0", CultureInfo.CurrentCulture) + " €");



            if (auctionBid.Value >= auction.InstantBuyPrice)
            {
                new AuctionRepository().InstantBuy(auction.AuctionId);
                if (auctionBid.RecieveEmails)
                    emailService.SendEmail("Aukcija je prodana", "Aukcija <b>" + auction.Name + "</b> je prodana s Instant buy opcijom s cijenom od <b>" + auctionBid.Value + "</b> €");
                return 1;
            }
            return -1;
        }
        /// <summary>
        /// Returns true if bid exists for a given auction and user
        /// </summary>
        /// <param name="auctionId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<bool> BidExists(int auctionId, int userId)
        {

            return await auctionBidRepository.DoesBidExists(auctionId, userId);
        }

        /// <summary>
        /// Returns all bids for a given auction
        /// Bids are sorted by value descending
        /// Bids are returned as 
        /// </summary>
        /// <param name="auctionId"></param>
        /// <returns>List<dynamic> containing bidder full name, bid value and date</returns>

        public List<dynamic> GetBids(int auctionId)
        {
            var bids = auctionBidRepository.GetBids(auctionId);
            bids = bids.OrderByDescending(p => p.Value).ToList();
            

           return bids.Select(p=> new
           {
               Korisnik=p.User.FullName,
               Vrijednost=p.Value.ToString("N0",CultureInfo.CurrentCulture),
               Datum =p.Date.ToString("dd.MM.yyyy. HH:mm")
           } as dynamic).ToList();
        }

        /// <summary>
        /// Returns highest bids for every currently active auction
        /// Bids are sorted by date descending
        /// <remarks>Toni Papić</remarks>
        /// </summary>
        /// <returns>List<AuctionBid> containing auction name, bid value and date  </returns>

       
        public List<AuctionBid> GetUserBids()
        {
            var bids= auctionBidRepository.GetBids().Where(p => p.BidderId == AuthenticationService.LoggedUser.UserId).OrderByDescending(p=>p.Value).ToList();
            var myBids= new List<AuctionBid>();
           foreach (var bid in bids.GroupBy(p=>p.AuctionId))
            {
                foreach(var b in bid)
                {
                    myBids.Add(b);
                    break;
                }
            }
           return myBids;

        }
        /// <summary>
        /// Returns all bids of specified user of active auctions
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>Karlo Jačmenjak, Toni Papić</remarks>
        /// <returns></returns>
        public List<AuctionBid> GetAllActiveUserBids(User user)
        {
            List<AuctionBid> bids = new AuctionBidRepository().GetBids();
            return bids.Where(p => p.BidderId == user.UserId && p.Auction.EndDate > DateTime.Now).ToList();
        }

        /// <summary>
        /// Returns all bids for a given auction
        /// </summary>
        /// <param name="auctionId"></param>
        /// <remarks>Toni Papić</remarks>
        /// <returns>Returns list of auction bids sorted desc by value</returns>
        public List<AuctionBid> GetBidsForAuction(int auctionId) {
            using(var repo = new AuctionBidRepository()) {
                return repo.GetBids(auctionId).OrderByDescending(p=>p.Value).ToList();
            }
        }


        /// <summary>
        /// Returns current (highest) bid for a given auction or null if there are no bids
        /// </summary>
        /// <param name="auctionBids"></param>
        /// <remarks>Created By: Josip Mojzeš</remarks>
        /// <remarks>Simplified By: Toni Papić</remarks>
        /// <returns>AuctionBid</returns>
        public AuctionBid GetCurrentBid(List<AuctionBid> auctionBids) {
            if (auctionBids.Count == 0) return null;
            return auctionBids.OrderByDescending(p => p.Value).FirstOrDefault(); ;

        }

    }
}
