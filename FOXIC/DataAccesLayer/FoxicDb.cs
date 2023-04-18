using FOXIC.Entities;
using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.SliderModel;
using FOXIC.Entities.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FOXIC.DataAccesLayer
{
    public class FoxicDb : IdentityDbContext
    {
        public FoxicDb(DbContextOptions<FoxicDb> options):base(options)
        {
            
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        //ClothingModels
        public DbSet<Clothing> Clothes { get; set; }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<ClothingTag> ClothingTags { get; set; }
		public DbSet<QualityDetail> QualityDetails { get; set; }
		public DbSet<ClothingQualityDetail> ClothingQualityDetails { get; set; }
		public DbSet<Collection> Collections { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ClothingColorSize> ClothingColorSizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        //UserModels
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasIndex(s => s.Key).IsUnique();
            modelBuilder.Entity<Slider>().HasIndex(s => s.Order).IsUnique();
            modelBuilder.Entity<Clothing>().HasIndex(c => c.Barcode).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Collection>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Color>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Size>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Tag>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Brand>().HasIndex(c => c.Name).IsUnique();
			base.OnModelCreating(modelBuilder);
        }
    }
}
