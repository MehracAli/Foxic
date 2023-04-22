using FOXIC.DataAccesLayer;
using FOXIC.Entities;
using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.UserModels;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FOXIC.Services
{
	public class LayoutService
    {
        public FoxicDb _context { get; set; }
        public IHttpContextAccessor _contextAccessor { get; set; }

        private readonly UserManager<User> _userManager;


        public LayoutService(FoxicDb context, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
			_userManager = userManager;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

		//public async Task<Basket> GetBasket()
		//{
  //          var currentUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);

		//	if(currentUser is not null)
		//	{
		//		Basket? basket = _context.Baskets
		//			.Include(b=>b.BasketItems)
		//				.ThenInclude(b=>b.ClothingColorSize)
		//					.ThenInclude(c=>c.Color)
		//						.Where(b=>b.User.UserName == currentUser.UserName)
		//							.FirstOrDefault(b=>b.IsOrdered == false);
  //              foreach (var item in basket.BasketItems)
  //              {
		//			Clothing clothing = _context.Clothes.FirstOrDefault(c => c.Id == item.ClothingId);
		//			if(clothing == null)
		//			{
		//				basket.BasketItems.Remove(item);
		//				basket.TotalPrice -= (item.UnitPrice*item.ItemQuantity);
		//			}
  //              }
  //              return basket;
		//	}
			
		//	Basket newBasket = new Basket();
		//	return newBasket;
		//}

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
						basket.TotalPrice -= (item.Price - item.Discount) * item.Quantity;
					}
					GetBasketItem(item.Id);
				}
			}
			return basket;
		}

		public Clothing GetBasketItem(int id)
		{
			var cookies = _contextAccessor.HttpContext.Request.Cookies["Basket"];
			List<Clothing> clothes = _context.Clothes.Include(c => c.Images).ToList();
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
