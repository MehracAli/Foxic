using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.UserModels;

namespace FOXIC.ViewModel
{
    public class ClothingVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		public string Description { get; set; }
		public int Stock { get; set; }
		public bool InStock { get; set; }
		public string SKU { get; set; }
		public string Barcode { get; set; }
		public int CollectionId { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }
		public Brand Brand { get; set; }
		public AddBasketVM AddCart { get; set; }
		public List<ProductImage> Images { get; set; }
		public List<int> ImageIds { get; set; } = null;
		public List<Comment> Comments { get; set; }
		public List<int> CommentsIds { get; set; } = null;
		public List<int> TagsIds { get; set; } = null;
		public List<int> QualityDetailsIds { get; set; } = null;
		public List<int> ColorsIds { get; set; } = null;
		public List<int> SizesIds { get; set; } = null;
	}
}
