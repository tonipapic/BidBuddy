namespace EntitiesLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Receipt")]
    public partial class Receipt
    {
        public int ReceiptId { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual User User { get; set; }
    }
}
