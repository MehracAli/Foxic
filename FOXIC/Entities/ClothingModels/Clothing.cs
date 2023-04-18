using FOXIC.Entities.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOXIC.Entities.ClothingModels
{
	public class Clothing:BaseEntity
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		public string Description { get; set; }
		[NotMapped]
		public bool InStock { get; set; } = false;
		public int Stock { get; set; }
		public string SKU { get; set; }
		public string Barcode { get; set; }
		public int BrandId { get; set; }
		public Brand Brand { get; set; }
		public int CollectionId { get; set; }
		public Collection Collection { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public List<Comment> Comments { get; set; }
		public List<ProductImage> Images { get; set; }
		public List<ClothingTag> ClothingTags { get; set; }
		public List<ClothingQualityDetail> ClothingQualityDetails { get; set; }
		public List<ClothingColorSize> ClothingColorSizes { get; set; }
		[NotMapped]
		public IFormFile iff_IsMainImage { get; set; }
		[NotMapped]
		public List<IFormFile> iff_Images { get; set; }
		[NotMapped]
        public List<int> TagsIds { get; set; }
		[NotMapped]
		public List<int> ColorsIds { get; set; }
        [NotMapped]
        public List<int> SizesIds { get; set; }
		[NotMapped]
		public List<int> ImagesIds { get; set; }
		[NotMapped]
		public string? ColorSizeQuantity { get; set; }

        public Clothing()
		{
			Images = new();
			ClothingQualityDetails = new();
			ClothingColorSizes = new();
			ClothingTags = new();
			iff_Images = new();
			TagsIds = new();
			ColorsIds = new();
			SizesIds = new();
			ImagesIds = new();
		}
	}
}
