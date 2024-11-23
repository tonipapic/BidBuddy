namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuctionBid")]
    public partial class AuctionBid
    {
        [Key]
        public int BidId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public decimal Value { get; set; }

        public int BidderId { get; set; }

        public int AuctionId { get; set; }

        public bool RecieveEmails { get; set; }

        public bool Selected { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual User User { get; set; }
    }
}
