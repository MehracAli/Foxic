namespace FOXIC.Entities.ClothingModels
{
	public class Size:BaseEntity
	{
		public string Name { get; set; }
		public List<ClothingColorSize> ClothingColorSizes { get; set; }

        public Size()
        {
			ClothingColorSizes = new();
		}
    }
}
