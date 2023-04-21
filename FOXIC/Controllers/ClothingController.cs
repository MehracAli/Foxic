using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.UserModels;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FOXIC.Controllers
{
	public class ClothingController : Controller
	{
		public FoxicDb _context { get; set; }
		private readonly UserManager<User> _userManager;

		public ClothingController(FoxicDb context, UserManager<User> userManager)
        {
			_context = context;
			_userManager = userManager;
        }
        public IActionResult Index(int page=1)
		{
			ViewBag.TotalPage = Math.Ceiling((double)_context.Clothes.Count() / 9);
			ViewBag.CurrentPage = page;

			List<ClothingVM> clothings = _context.Clothes
			.Include(c => c.Images)
					.Include(c => c.ClothingTags)
						.Include(c => c.ClothingQualityDetails)
							.Include(c => c.ClothingColorSizes)
								.Include(c=>c.Brand)
			.Select(c => new ClothingVM
			{
				Id = c.Id,
				Name = c.Name,
				Price = c.Price,
				Discount = c.Discount,
				Description = c.Description,
				Stock = c.Stock,
				InStock = c.InStock,
				SKU = c.SKU,
				Barcode = c.Barcode,
				Brand = c.Brand,
				CollectionId = c.CollectionId,
				CategoryId = c.CategoryId,
				ImageIds = c.Images.Select(i => i.Id).ToList(),
				Images = c.Images.Select(p => new ProductImage
				{
					Id = p.Id,
					Path = p.Path,
					IsMain = p.IsMain
				}).ToList(),
				TagsIds = c.ClothingTags.Select(t => t.TagId).ToList(),
				QualityDetailsIds = c.ClothingQualityDetails.Select(qd => qd.QualityDetailId).ToList(),
				ColorsIds = c.ClothingColorSizes.Select(c => c.ColorId).ToList(),
				SizesIds = c.ClothingColorSizes.Select(c => c.SizeId).ToList(),
			}).Skip((page-1)/9).Take(9).ToList();

			ViewBag.Images = _context.ProductImages.ToList();
			ViewBag.Collections = _context.Collections.ToList();
			ViewBag.Clothes = _context.Clothes.ToList();

			return View(clothings);
		}

		public IActionResult Detail(int Id)
		{
			if(Id == 0) return NotFound();

			ClothingVM? clothing = _context.Clothes
			.Include(c => c.Images)
					.Include(c=>c.ClothingTags)
						.Include(c=>c.ClothingQualityDetails)
							.Include(c=>c.ClothingColorSizes)
								.Include(c=>c.Comments).ThenInclude(c=>c.User)
			.Select(c => new ClothingVM
			{
				Id = c.Id,
				Name = c.Name,
				Price = c.Price,
				Discount = c.Discount,
				Description = c.Description,
				Stock = c.Stock,
				InStock = c.InStock,
				SKU = c.SKU,
				Barcode = c.Barcode,
				CollectionId = c.CollectionId,
				CategoryId = c.CategoryId,
				ImageIds = c.Images.Select(i=>i.Id).ToList(),
				TagsIds = c.ClothingTags.Select(t=>t.TagId).ToList(),
				QualityDetailsIds = c.ClothingQualityDetails.Select(qd=>qd.QualityDetailId).ToList(),
				ColorsIds = c.ClothingColorSizes.Select(c=>c.ColorId).ToList(),
				SizesIds = c.ClothingColorSizes.Select(c=>c.SizeId).ToList(),
				Comments = c.Comments.ToList(),
				CommentsIds = c.Comments.Select(c=>c.Id).ToList(),
			}).FirstOrDefault(c => c.Id == Id);

			if(clothing is null) return NotFound();

			ViewBag.Clothes = _context.Clothes
				.Include(c=>c.ClothingTags)
					.Include(c=>c.Images)
						.Include(c=>c.ClothingColorSizes)
							.ToList();
			ViewBag.QualityDetails = _context.QualityDetails.ToList();
			ViewBag.Collections = _context.Collections.ToList();
			ViewBag.Images = _context.ProductImages.ToList();
			ViewBag.Colors = _context.Colors.ToList();
			ViewBag.Sizes = _context.Sizes.ToList();
			ViewBag.Tags = _context.Tags.ToList();
			ViewBag.Comments = _context.Comments.ToList();
			return View(clothing);
		}

		public IActionResult AddToBasket(int Id)
		{
			ViewBag.Clothes = _context.Clothes
				.Include (c=>c.Images)
					.DistinctBy(c=>c.Id)
						.ToList();

			if (Id == null) return NotFound();
			Clothing clothing = _context.Clothes.FirstOrDefault(c=>c.Id == Id);
			if (clothing is null) return NotFound();

			User? user = (User)_context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
			Basket basket = _context.Baskets.FirstOrDefault(b => b.User.Name == User.Identity.Name);

			if(user != null)
			{
				if (basket is null)
				{
					Basket newBasket = new()
					{
						User = user,
					};

					BasketItem basketItem = new()
					{
						ClothingId = Id,
						UnitPrice = (clothing.Price-clothing.Discount),
						Discount = clothing.Discount,
						ItemQuantity = 1,
						BasketId = newBasket.Id,
						Basket = newBasket
					};

					newBasket.BasketItems.Add(basketItem);
					newBasket.TotalPrice += basketItem.UnitPrice;

					_context.BasketItems.Add(basketItem);
					_context.Baskets.Add(newBasket);
				}
				else
				{

					BasketItem exicted = basket.BasketItems.Find(bi => bi.ClothingId == Id);

					if (exicted is null)
					{
						BasketItem newItem = new()
						{
							ClothingId = Id,
							UnitPrice = (clothing.Price - clothing.Discount),
							Discount = clothing.Discount,
							ItemQuantity = 1,
							BasketId = basket.Id,
							Basket = basket,
						};

						basket.BasketItems.Add(newItem);
						basket.TotalPrice += newItem.UnitPrice;

						_context.BasketItems.Add(newItem);
					}
					else
					{
						exicted.ItemQuantity++;
						basket.TotalPrice += (exicted.UnitPrice);
					}

				}
			}

			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult AddToBasketDetail(int Id, ClothingVM clothingVM)
		{
			if (Id == null) return NotFound();
			Clothing clothing = _context.Clothes.FirstOrDefault(c => c.Id == Id);
			if (clothing is null) return NotFound();

			User? user = (User)_context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
			Basket basket = _context.Baskets.FirstOrDefault(b => b.User.Name == User.Identity.Name);

			Color color = _context.Colors.FirstOrDefault(c => c.Id == clothingVM.AddCart.ColorId);
			Size size = _context.Sizes.FirstOrDefault(s=>s.Id == clothingVM.AddCart.SizeId);
			int quantity = clothingVM.AddCart.Quantity;

			//return Json(color);

			if (user != null)
			{
				if (basket is null)
				{
					Basket newBasket = new()
					{
						User = user,
					};

					BasketItem basketItem = new()
					{
						ClothingId = Id,
						UnitPrice = (clothing.Price - clothing.Discount),
						Discount = clothing.Discount,
						ItemQuantity = quantity,
						BasketId = newBasket.Id,
						Basket = newBasket,
					};

					basketItem.ClothingColorSize.ColorId = color.Id;
					basketItem.ClothingColorSize.SizeId = size.Id;
					basketItem.ClothingColorSize.ClothingId = clothing.Id;

					newBasket.BasketItems.Add(basketItem);
					newBasket.TotalPrice += basketItem.UnitPrice;

					_context.BasketItems.Add(basketItem);
					_context.Baskets.Add(newBasket);
				}
				else
				{

					BasketItem exicted = basket.BasketItems.Find(bi => bi.ClothingId == Id);

					if (exicted is null)
					{
						BasketItem newItem = new()
						{
							ClothingId = Id,
							UnitPrice = (clothing.Price - clothing.Discount),
							Discount = clothing.Discount,
							ItemQuantity = 1,
							BasketId = basket.Id,
							Basket = basket,
						};

						newItem.ClothingColorSize.ColorId = color.Id;
						newItem.ClothingColorSize.SizeId = size.Id;
						newItem.ClothingColorSize.ClothingId = clothing.Id;

						basket.BasketItems.Add(newItem);
						basket.TotalPrice += newItem.UnitPrice;

						_context.BasketItems.Add(newItem);
					}
					else
					{
						exicted.ItemQuantity++;
						basket.TotalPrice += (exicted.UnitPrice);
					}

				}
			}

			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		//public IActionResult AddToBasket(int Id)
		//{
		//	if(Id == 0) return NotFound();

		//	Clothing clothing = _context.Clothes.FirstOrDefault(c => c.Id == Id);

		//	if(clothing is null) return NotFound();

		//	var Cookies = HttpContext.Request.Cookies["Basket"];

		//	CookiesBasketVM basket = new();

		//	if (Cookies is null)
		//	{
		//		CookiesItemVM item = new()
		//		{
		//			Id = clothing.Id,
		//			Quantity = 1,
		//			Price = clothing.Price,
		//			Discount = clothing.Discount,
		//		};
		//		basket.cookiesItemVMs.Add(item);
		//		basket.TotalPrice += (item.Price-item.Discount);
		//	}
		//	else
		//	{
		//		basket = JsonConvert.DeserializeObject<CookiesBasketVM>(Cookies);
		//		CookiesItemVM exictedItem = basket.cookiesItemVMs.Find(i => i.Id == Id);
		//		if (exictedItem is null)
		//		{
		//			CookiesItemVM item = new()
		//			{
		//				Id = clothing.Id,
		//				Quantity = 1,
		//				Price = clothing.Price,
		//				Discount = clothing.Discount,
		//			};
		//			basket.cookiesItemVMs.Add(item);
		//			basket.TotalPrice += (item.Price-item.Discount);
		//		}
		//		else
		//		{
		//			exictedItem.Quantity++;
		//			basket.TotalPrice += exictedItem.Price-exictedItem.Discount;
		//		}
		//	}
		//	var basketStr = JsonConvert.SerializeObject(basket);
		//	HttpContext.Response.Cookies.Append("basket", basketStr);
		//	return RedirectToAction(nameof(Index));
		//}

		//public IActionResult DeleteBasketItem(int Id)
		//{
		//	var cookies = HttpContext.Request.Cookies["Basket"];
		//	var basket = JsonConvert.DeserializeObject<CookiesBasketVM>(cookies);

		//	foreach (CookiesItemVM item in basket.cookiesItemVMs)
		//	{
		//		if (item.Id == Id)
		//		{
		//			basket.cookiesItemVMs.Remove(item);
		//			basket.TotalPrice -= (item.Price-item.Discount) * item.Quantity;
		//			break;
		//		};
		//	}
		//	var basketStr = JsonConvert.SerializeObject(basket);
		//	HttpContext.Response.Cookies.Append("basket", basketStr);
		//	return RedirectToAction("Index", "Home");
		//}

		public async Task<IActionResult> AddComment(int Id, Comment newComment)
		{
			if (newComment.Text is null)
			{
				return Json("comment cant be empty");
			}
			Clothing clothing = await _context.Clothes.Include(pt => pt.Comments).FirstOrDefaultAsync(c => c.Id == Id);
				User user = await _userManager.FindByNameAsync(User.Identity.Name);
				Comment comment = new Comment()
				{
					Text = newComment.Text,
					User = user,
					CreationTime = DateTime.UtcNow,
					ClothingId = newComment.ClothingId,
					Clothing = clothing
				};
				user.Comments.Add(comment);
				clothing.Comments.Add(comment);
				await _context.Comments.AddAsync(comment);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Detail), new { Id });
		}

		public async Task<IActionResult> Edit(Comment editedComment, int Id)
		{
			if(editedComment.Text is null)
			{
				return Json("comment cant be empty");
			}

			Comment comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);
			User user = await _userManager.FindByNameAsync(User.Identity.Name);

			comment.Text = editedComment.Text;
			comment.ClothingId = editedComment.ClothingId;
			int id = comment.ClothingId;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Detail), new { id });
		}

		public async Task<IActionResult> Delete(int Id, Comment deletedComment)
		{
			Comment comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == deletedComment.Id);
			//return Json(comment);

			Clothing clothing = await _context.Clothes.FirstOrDefaultAsync(c=>c.Id == 2);
			//return Json(clothing);

			User user = await _userManager.FindByNameAsync(User.Identity.Name);

			clothing.Comments.Remove(comment);
			user.Comments.Remove(comment);

			comment.ClothingId = deletedComment.ClothingId;
			int id = comment.ClothingId;

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Detail), new { id });
		}
	}
}
