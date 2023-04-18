using FOXIC.DataAccesLayer;
using FOXIC.Entities;
using FOXIC.Entities.ClothingModels;
using FOXIC.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FOXIC.Services
{
	public class LayoutService
    {
        public FoxicDb _context { get; set; }
        public IHttpContextAccessor _contextAccessor { get; set; }

        public LayoutService(FoxicDb context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

		public CookiesBasketVM GetBasket()
		{
			var cookies = _contextAccessor.HttpContext.Request.Cookies["Basket"];
			CookiesBasketVM basket = new();
			if (cookies is not null)
			{
				basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);
				foreach (CookiesItemVM item in basket.cookiesItemVMs)
				{
					Clothing clothing = _context.Clothes.FirstOrDefault(p => p.Id == item.Id);
					if (clothing is null)
					{
						basket.cookiesItemVMs.Remove(item);
						basket.TotalPrice -= (item.Price-item.Discount) * item.Quantity;
					}
					GetBasketItem(item.Id);
				}
			}
			return basket;
		}

		public Clothing GetBasketItem(int id)
		{
			var cookies = _contextAccessor.HttpContext.Request.Cookies["Basket"];
			List<Clothing> clothes = _context.Clothes.Include(c=>c.Images).ToList();
			CookiesBasketVM basket = new();
			if (cookies is not null)
			{
				basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);
				foreach (var item in basket.cookiesItemVMs)
				{
					Clothing clothing = clothes.FirstOrDefault(p => p.Id == id);
					if (clothing is not null)
					{
						return clothing;
					}
				}
			}
			return null;
		}
	}
}
