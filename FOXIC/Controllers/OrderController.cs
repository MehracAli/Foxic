using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.UserModels;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FOXIC.Controllers
{
	public class OrderController : Controller
	{
		public FoxicDb _context { get; set; }
		private readonly UserManager<User> _userManager;

        public OrderController(FoxicDb context, UserManager<User> userManager)
        {
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index(string userName)
		{
			Basket basket = _context.Baskets.Include(b => b.BasketItems).FirstOrDefault();

			if (basket == null)
			{
				Basket newBasket = new();
				return View(newBasket);
			}

			User user = (User)_context.Users.FirstOrDefault(u=>u.UserName == userName);

			Basket userBasket =  user.Baskets.FirstOrDefault(b => b.Id == basket.Id);

			ViewBag.BasketItems = _context.BasketItems.ToList();
			ViewBag.Clothes = _context.Clothes.Include(c=>c.Images).ToList();

			return View(userBasket);
		}

		[HttpPost]
		public async Task<IActionResult> AddToDbBasket(int clothingId, ClothingVM basketClothing)
		{
			if (User.Identity.IsAuthenticated)
			{
				ClothingColorSize? clothingColorSize = _context.ClothingColorSizes
					.Include(cs => cs.Clothing)
						.FirstOrDefault(cs => cs.Id == clothingId && cs.ColorId == basketClothing.AddCart.ColorId && cs.SizeId == basketClothing.AddCart.SizeId);

				if(clothingColorSize is null) return Json(clothingColorSize);

				User user = await _userManager.FindByNameAsync(User.Identity.Name);
				Basket? basket = _context.Baskets
					.Include(b => b.User)
						.FirstOrDefault(b => b.User.Id == user.Id && !b.IsOrdered) ?? new Basket();

				BasketItem basketItem = new()
				{
					ClothingColorSizeId = clothingColorSize.Id,
					ItemQuantity = basketClothing.AddCart.Quantity,
					UnitPrice = clothingColorSize.Clothing.Price,
				};
				basket.BasketItems.Add(basketItem);
			}
			return RedirectToAction(nameof(Index));
		}

	}
}
