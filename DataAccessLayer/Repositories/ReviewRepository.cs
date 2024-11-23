using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Repository for accessing review data.
    /// </summary>
    /// <remarks>Dorian Rušak</remarks>
    public class ReviewRepository : Repository<Review>
    {
        /// <summary>
        /// Retrieves reviews written by a specific seller.
        /// </summary>
        /// <param name="sellerId"></param>
        /// <returns>Queryable collection of reviews by the seller</returns>
        /// <remarks>Dorian Rušak</remarks>
        public IQueryable<Review> GetReviewsBySeller(int sellerId)
        {
            var query = from r in Entities
                        where r.WriterId == sellerId
                        select r;
            return query;
        }

        /// <summary>
        /// Retrieves all reviews in the repository.
        /// </summary>
        /// <returns>Queryable collection of all reviews.</returns>
        /// <remarks>Dorian Rušak</remarks>
        public IQueryable<Review> GetReviews()
        {
            var query = from r in Entities
                        select r;
            return query;
        }

        /// <summary>
        /// Updates a review entity in the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        /// <remarks>Dorian Rušak</remarks>
        public override int Update(Review entity, bool save = true)
        {
            var review = Entities.SingleOrDefault((r) => r.ReviewId == entity.ReviewId);
            review.Rating = entity.Rating;
            review.Comment = entity.Comment;
            review.WriterId = entity.WriterId;
            review.AuctionId = entity.AuctionId;
            review.Date = entity.Date;

            if (save) return SaveChanges();
            return 0;
        }
    }
}
