using System.ComponentModel.DataAnnotations.Schema;

namespace FOXIC.Entities.ClothingModels
{
	public class Color:BaseEntity
	{
		public string Name { get; set; }
		public string Path { get; set; }
		[NotMapped]
		public IFormFile? iff_Path { get; set; }
		public List<ClothingColorSize> ClothingColorSizes { get; set; }

		public Color()
		{
			ClothingColorSizes = new();
		}
	}
}
