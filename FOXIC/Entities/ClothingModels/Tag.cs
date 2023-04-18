namespace FOXIC.Entities.ClothingModels
{
	public class Tag:BaseEntity
	{
		public string Name { get; set; }
		public List<ClothingTag> ClothingTags { get; set; }
        
        public Tag()
        {
            ClothingTags = new();    
        }
    }
}
