namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public int WriterId { get; set; }

        public int AuctionId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual User User { get; set; }
    }
}
