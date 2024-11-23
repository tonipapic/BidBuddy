namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuctionImage")]
    public partial class AuctionImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public byte[] ImageData { get; set; }

        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
