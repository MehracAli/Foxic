using FOXIC.Entities.ClothingModels;

namespace FOXIC.Entities.UserModels
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public User User { get; set; }
        public int ClothingId { get; set; }
        public Clothing Clothing { get; set; }
    }
}
