using System.ComponentModel.DataAnnotations.Schema;

namespace FOXIC.Entities.SliderModel
{
    public class Slider:BaseEntity
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile iff_ImagePath { get; set; }
    }
}
