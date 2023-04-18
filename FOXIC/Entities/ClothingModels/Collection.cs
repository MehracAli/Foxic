namespace FOXIC.Entities.ClothingModels
{
	public class Collection : BaseEntity
	{
		public string Name { get; set; }
		public List<Clothing> Clothes { get; set; }

        public Collection()
        {
            Clothes = new();
        }
    }
}
