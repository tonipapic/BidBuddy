using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Service class for managing reviews.
    /// </summary>
    /// <remarks>Dorian Rušak</remarks>
    public class ReviewService
    {
        public ReviewService() { }

        /// <summary>
        /// Adds a new review to the repository.
        /// </summary>
        /// <param name="review"></param>
        /// <remarks>Dorian Rušak</remarks>
        public void AddReview(Review review)
        {
            using (var repo = new ReviewRepository())
            {
                repo.Add(review);
            }
        }

        /// <summary>
        /// Retrieves a list of all reviews.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Dorian Rušak</remarks>
        public List<Review> GetReviews()
        {
            using (var repo = new ReviewRepository())
            {
                return repo.GetReviews().ToList();
            }
        }


        /// <summary>
        /// Retrieves a list of reviews for a specific seller.
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns></returns>
        /// <remarks>Dorian Rušak</remarks>
        public List<Review> GetReviewsForSeller(int sellerId)
        {
            using (var repo = new ReviewRepository())
            {
                return repo.GetReviews()
                           .Where(r => r.Auction.CreatorId == sellerId && r.WriterId != sellerId)
                           .ToList();
            }
        }
    }
}
