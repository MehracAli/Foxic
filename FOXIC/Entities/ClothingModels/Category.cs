namespace FOXIC.Entities.ClothingModels
{
	public class Category:BaseEntity
	{
		public string Name { get; set; }
		public List<Clothing> Clothings { get; set; }

        public Category()
        {
			Clothings = new();
		}
    }
}
