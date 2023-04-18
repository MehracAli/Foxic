namespace FOXIC.Entities.ClothingModels
{
	public class Brand:BaseEntity
	{
		public string Name { get; set; }
		public List<Clothing> clothings { get; set; }
	}
}
