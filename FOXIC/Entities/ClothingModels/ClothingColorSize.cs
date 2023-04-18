using FOXIC.Entities.UserModels;

namespace FOXIC.Entities.ClothingModels
{
	public class ClothingColorSize:BaseEntity
	{
		public int ClothingId { get; set; }
		public int ColorId { get; set; }
		public int SizeId { get; set; }

		public Clothing Clothing { get; set; }
		public Color Color { get; set; }
		public Size Size { get; set; }

		public int Quantity { get; set; }
		public List<BasketItem>? BasketItems { get; set; }

        public ClothingColorSize()
        {
            BasketItems = new();
        }
    }
}
