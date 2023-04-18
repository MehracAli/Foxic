namespace FOXIC.Entities.ClothingModels
{
	public class ClothingQualityDetail:BaseEntity
	{
		public int ClothingId { get; set; }
		public int QualityDetailId { get; set; }
		public Clothing Clothing { get; set; }
		public QualityDetail QualityDetail { get; set; }
	}
}
