namespace FOXIC.Entities.ClothingModels
{
	public class ProductImage : BaseEntity
	{
		public string Path { get; set; }
		public bool IsMain { get; set; }
		public int ClothingId { get; set; }
		public Clothing Clothing { get; set; }
	}
}
