namespace FOXIC.Entities.ClothingModels
{
	public class QualityDetail:BaseEntity
	{
		public string Detail { get; set; }
		public List<ClothingQualityDetail> ClothingQualityDetails { get; set; }

        public QualityDetail()
        {
			ClothingQualityDetails = new();
		}
    }
}
