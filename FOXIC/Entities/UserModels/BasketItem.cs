using FOXIC.Entities.ClothingModels;

namespace FOXIC.Entities.UserModels
{
	public class BasketItem:BaseEntity
	{
		public decimal UnitPrice { get; set; }
		public int ItemQuantity { get; set; }
		public int ClothingColorSizeId { get; set; }
		public ClothingColorSize ClothingColorSize { get; set; }
		public int BasketId { get; set; }
		public Basket Basket { get; set; }
	}
}
