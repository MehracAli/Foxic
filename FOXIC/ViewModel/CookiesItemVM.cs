using FOXIC.Entities;

namespace FOXIC.ViewModel
{
    public class CookiesItemVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int? ClorId { get; set; } = 0;
        public int? SizeId { get; set; } = 0;
	}
}
