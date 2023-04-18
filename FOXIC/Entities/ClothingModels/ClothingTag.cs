namespace FOXIC.Entities.ClothingModels
{
	public class ClothingTag:BaseEntity
	{
		public int ClothingId { get; set; }
		public int TagId { get; set; }
		public Clothing Clothing { get; set; }
		public Tag Tag { get; set; }
	}
}
