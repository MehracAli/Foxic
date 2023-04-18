using FOXIC.Entities;

namespace FOXIC.ViewModel
{
    public class CookiesBasketVM
    {
        public decimal TotalPrice { get; set; }
        public  List<CookiesItemVM> cookiesItemVMs { get; set; }

        public CookiesBasketVM()
        {
            cookiesItemVMs = new();
        }
    }
}
