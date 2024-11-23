using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EntitiesLayer.Entities {
    public partial class DatabaseContext : DbContext {
        public DatabaseContext()
            : base("name=DatabaseContext") {
        }

        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<AuctionBid> AuctionBids { get; set; }
        public virtual DbSet<AuctionImage> AuctionImages { get; set; }
        public virtual DbSet<AuctionState> AuctionStates { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductState> ProductStates { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Auction>()
                .Property(e => e.MinimalBidPrice)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Auction>()
                .Property(e => e.InstantBuyPrice)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.AuctionBids)
                .WithRequired(e => e.Auction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.AuctionImages)
                .WithRequired(e => e.Auction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.Receipts)
                .WithRequired(e => e.Auction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Auction>()
                .HasMany(e => e.Reviews)
                .WithRequired(e => e.Auction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AuctionBid>()
                .Property(e => e.Value)
                .HasPrecision(19, 2);

            modelBuilder.Entity<AuctionState>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.AuctionState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.SubCategories)
                .WithOptional(e => e.ParentCategory)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<ProductState>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.ProductState)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Receipt>()
                .Property(e => e.Amount)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.Region)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Auctions)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.AuctionBids)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.BidderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Receipts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reviews)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.WriterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserRole)
                .HasForeignKey(e => e.UserRoleId)
                .WillCascadeOnDelete(false);
        }
    }
}
