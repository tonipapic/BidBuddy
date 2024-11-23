namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Auction")]
    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            AuctionBids = new HashSet<AuctionBid>();
            AuctionImages = new HashSet<AuctionImage>();
            Receipts = new HashSet<Receipt>();
            Reviews = new HashSet<Review>();
        }

        public int AuctionId { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        public decimal MinimalBidPrice { get; set; }

        public decimal? InstantBuyPrice { get; set; }

        public int ProductStateId { get; set; }

        public int CreatorId { get; set; }

        public int RegionId { get; set; }

        public int CategoryId { get; set; }

        public int AuctionStateId { get; set; }

        public virtual AuctionState AuctionState { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }

        public virtual ProductState ProductState { get; set; }

        public virtual Region Region { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuctionBid> AuctionBids { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuctionImage> AuctionImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
