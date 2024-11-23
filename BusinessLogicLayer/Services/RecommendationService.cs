using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// <remarks>Karlo Jačmenjak</remarks>
    /// </summary>
    public class RecommendationService
    {
        private AuctionBidService auctionBidService;
        private AuctionService auctionService;

        public RecommendationService()
        {
            auctionBidService = new AuctionBidService();
            auctionService = new AuctionService();
        }

        /// <summary>
        /// Generates a <paramref name="count"/> list of <see cref="Auction"/> for <paramref name="user"/> 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns>async List of <see cref="Auction"/></returns>
        public async Task<List<Auction>> GetUsersRecommendedAuctions(User user, int count)
        {
            List<Auction> auctions;

            Task<List<Auction>> auctionsTask = GenerateRecommendations(user, count);
            auctions = await auctionsTask;
            return auctions;
        }

        /// <summary>
        /// <para>Returns a List of <see cref="Auction"/>s to remove duplicates of auctions</para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="auctions"></param>
        /// <returns></returns>
        private static List<Auction> FilterDuplicateAuctions(List<Auction> auctions)
        {
            return auctions
                .GroupBy(auction => auction.AuctionId)
                .Select(group => group.First())
                .ToList();
        }
        /// <summary>
        /// <para>Returns a List of <see cref="Auction"/> in which <paramref name="user"/> bidded</para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<AuctionBid> GetUsersBids(User user)
        {
            List<AuctionBid> bids = auctionBidService.GetAllActiveUserBids(user);
            return bids;
        }
        /// <summary>
        /// 
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private async Task<List<Auction>> GenerateRecommendations(User user, [Range(0, int.MaxValue)] int count)
        {
            List<Auction> auctions = new List<Auction>();

            List<AuctionBid> usersBids = GetUsersBids(user);
            foreach (var userBid in usersBids)
            {
                if (auctions.Count >= count) break;
                var filters = CreateRecommendationsFilters(user, userBid);
                var newAuctions = await GetAuctionsAsync(filters);
                auctions.AddRange(newAuctions);
            }
            auctions = FilterDuplicateAuctions(auctions);
            if (auctions.Count < count)
            {
                var filters = CreateFillerAuctionFilters();
                var fillers = PickRandomAuctions(await GetAuctionsAsync(filters), count - auctions.Count);
                auctions.AddRange(fillers);
            }
            else
            {
                auctions = PickRandomAuctions(auctions, count);
            }

            return FilterDuplicateAuctions(auctions);
        }

        /// <summary>
        /// Applies gets auctions based on <paramref name="filters"/>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>List of <see cref="Auction"/></returns>
        private async Task<List<Auction>> GetAuctionsAsync(Filter<Auction> filters)
        {
            return await auctionService.GetAuctionsFilteredAsync(filters);
        }

        /// <summary>
        /// <para>Returns a random list of <paramref name="count"/> <see cref="Auction"/> from <paramref name="auctions"/></para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="auctions"></param>
        /// <param name="count"></param>
        /// <returns>List of <see cref="Auction"/></returns>
        private static List<Auction> PickRandomAuctions(List<Auction> auctions, int count)
        {
            Random random = new Random();
            return auctions.OrderBy(x => random.Next()).Take(count).ToList();
        }

        /// <summary>
        /// <para>Creates <see cref="Filter{T}"/> for finding potential recommendations for <paramref name="user"/> based on <paramref name="bid"/></para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bid"></param>
        /// <returns><see cref="Filter{T}"/></returns>
        private static Filter<Auction> CreateRecommendationsFilters(User user, AuctionBid bid)
        {
            Filter<Auction> filter = new Filter<Auction>();
            filter.AddFilter(x => x.EndDate > DateTime.Now);
            filter.AddFilter(x => !x.AuctionBids.Any(b => b.BidderId == user.UserId));
            return filter;
        }

        /// <summary>
        /// <para>Creates <see cref="Filter{T}"/> for finding additional auctions to fill recommendations</para>
        /// <remarks>Karlo Jačmenjak</remarks>
        /// </summary>
        /// <returns><see cref="Filter{T}"/></returns>
        private static Filter<Auction> CreateFillerAuctionFilters()
        {
            Filter<Auction> filter = new Filter<Auction>();
            filter.AddFilter(x => x.EndDate > DateTime.Now);
            filter.AddFilter(x => x.CreatorId != AuthenticationService.LoggedUser.UserId);
            return filter;
        }
    }
}
