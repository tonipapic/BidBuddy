using BusinessLogicLayer.Exceptions;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services {
    /// <summary>
    /// <remarks>Karlo Jačmenjak</remarks>
    /// </summary>
    public class AuctionService
    {
        /// <summary>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <returns>A list of all existing <see cref="Auction">Auction</see></returns>
        public List<Auction> GetAllAuctions()
        {
            using (var repo = new AuctionRepository())
            {
                var q = repo.GetAll()
                    .Include(a => a.User)
                    .Include(a => a.Region)
                    .Include(a => a.Category)
                    .Include(a => a.ProductState)
                    .Include(a => a.AuctionState);

                List<Auction> auctions = q.ToList();
                AuctionState finishedState = new AuctionStateService().GetStateById(AuctionState.Finished);

                foreach (Auction auction in auctions)
                {
                    UpdateActualAuctionState(auction, finishedState);
                }

                return auctions;
            }
        }
        /// <summary>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auctionId"></param>
        /// <returns><see cref="Auction"/> that has the AuctionId equal <paramref name="auctionId"/></returns>
        public Auction GetAuctionById(int auctionId) {
            using(var repo = new AuctionRepository()) {
                var q = repo.GetAll()
                    .Include(a => a.User)
                    .Include(a => a.Region)
                    .Include(a => a.Category)
                    .Include(a => a.ProductState)
                    .Include(a => a.AuctionState);
                q = q.Where((e) => e.AuctionId == auctionId);

                Auction auction = q.FirstOrDefault();
                AuctionState finishedState = new AuctionStateService().GetStateById(AuctionState.Finished);
                UpdateActualAuctionState(auction, finishedState);

                return auction;
            }
        }
        /// <summary>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <returns>A List of <see cref="Auction"/> objects that the <see cref="User"/> with <paramref name="userId"/> created</returns>
        public List<Auction> GetAuctionsForUser(int userId) {
            using (var repo = new AuctionRepository()) {
                List<Auction> auctions = repo.GetAuctionsByUserId(userId);
                AuctionState finishedState = new AuctionStateService().GetStateById(AuctionState.Finished);

                foreach (Auction auction in auctions) {
                    UpdateActualAuctionState(auction, finishedState);
                }

                return auctions;
            }
        }
        /// <summary>
        /// <para>Checks if <paramref name="auction"/> is valid and saves it in the repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        public void CreateAuction(Auction auction)
        {
            DateTime now = DateTime.Now;
            auction.CreationDate = now;
            auction.AuctionStateId = AuctionState.Active;
            CheckAuction(auction);
            AddAuction(auction);
        }

        /// <summary>
        /// <para>Saves <paramref name="auction"/> to the database</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        private void AddAuction(Auction auction) {
            using (var repo = new AuctionRepository()) {
                repo.Add(auction);
            }
        }
        /// <summary>
        /// <para>Updates <paramref name="auction"/> entry in repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        public void UpdateAuction(Auction auction) {
            using (var repo = new AuctionRepository()) {
                repo.Update(auction);
            }
        }

        public void DeleteAuction(Auction auction) {
            using(var repo = new AuctionRepository()) {
                repo.Remove(auction);
            }
        }

        /// <summary>
        /// <para>Returns a List of <see cref="Auction"/> asynchronously filtered by a List of <see cref="Filter{T}"/></para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="filterCollection"></param>
        /// <returns></returns>
        public async Task<List<Auction>> GetAuctionsFilteredAsync(Filter<Auction> filterCollection)
        {
            IQueryable<Auction> queryable = null;
            using (var repo = new AuctionRepository())
            {
                var filters = filterCollection.Filters;
                queryable = repo.GetAll()
                    .Include(a => a.User)
                    .Include(a => a.Region)
                    .Include(a => a.Category)
                    .Include(a => a.ProductState)
                    .Include(a => a.AuctionState)
                    .Include(a => a.AuctionBids);
                foreach (var filter in filters)
                {
                    queryable = queryable.Where(filter);
                }

                List<Auction> auctions = await queryable.ToListAsync();
                AuctionState finishedState = new AuctionStateService().GetStateById(AuctionState.Finished);
                
                foreach (Auction auction in auctions) {
                    UpdateActualAuctionState(auction, finishedState);
                }

                return auctions;
            }
        }
        /// <summary>
        /// <para>Checks if auctions Properties are valid for saving in repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        /// <exception cref="InvalidInputException"></exception>
        public void CheckAuction(Auction auction) {

            DateTime endDate = auction.EndDate;

            if (endDate < DateTime.Now) {
                throw new InvalidInputException("Datum završetka aukcije ne može biti u prošlosti!");
            }

            TimeSpan auctionDuration = endDate - DateTime.Now;

            if (auctionDuration.TotalMinutes < 10) {
                throw new InvalidInputException("Aukcija treba trajati barem 10 minuta!");
            }

            if (auction.MinimalBidPrice <= 0) {
                throw new InvalidInputException("Cijena početne ponude ne može biti negativna ili nula!");
            }

            if (auction.InstantBuyPrice <= auction.MinimalBidPrice) {
                throw new InvalidInputException("Cijena instant kupnje mora biti veća od cijene početne ponude!");
            }

        }

        /// <summary>
        /// <para>Updates and marks <paramref name="auction"/> as completed</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        /// <exception cref="ManageAuctionException"></exception>
        public void ConfirmAuctionEnd(Auction auction) {

            if(auction.AuctionStateId != AuctionState.Finished) {
                throw new ManageAuctionException("Za potvrdu završetka aukcije, aukcija mora biti u stanju završena!");
            }

            var bidService = new AuctionBidService();
            List<AuctionBid> bids = bidService.GetBidsForAuction(auction.AuctionId);

            if(bids.Count != 0) {
                auction.AuctionStateId = AuctionState.PaymentProcessing;
            } else {
                auction.AuctionStateId = AuctionState.NotSold;
            }

            UpdateAuction(auction);
           
            new EmailService().SendEmail(bids.FirstOrDefault().User.Email,"Završetak aukcije "+auction.Name,"Aukcija "+auction.Name+" je završila, te ste pobijedili jer ste ponudili najveći iznos.");
        }

        /// <summary>
        /// <para>Updates and marks <paramref name="auction"/> as sold</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="auction"></param>
        /// <exception cref="ManageAuctionException"></exception>
        public void ConfirmDelivery(Auction auction) {

            if(auction.AuctionStateId != AuctionState.InDelivery) {
                throw new ManageAuctionException("Za potvrdu dostave, aukcija mora biti u stanju dostave!");
            }

            auction.AuctionStateId = AuctionState.Sold;
            UpdateAuction(auction);
        }

        /// <summary>
        /// Updates <paramref name="auction"/> with new <paramref name="finishedState"/>
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="finishedState"></param>
        /// <exception cref="Exception"></exception>
        private void UpdateActualAuctionState(Auction auction, AuctionState finishedState) {

            if(finishedState.AuctionStateId != AuctionState.Finished) {
                throw new Exception("Metodi se mora proslijediti instanca stanja auckije Finished!");
            }

            // if the auction is still active, it may have ended
            // we cannot automatically switch the auction state to Finished
            if (auction.AuctionStateId == AuctionState.Active) {

                if (DateTime.Now >= auction.EndDate) {
                    // auction has ended
                    // set auction state to Finished
                    auction.AuctionStateId = finishedState.AuctionStateId;
                    auction.AuctionState = finishedState;
                }

            }

        }

    }
}
