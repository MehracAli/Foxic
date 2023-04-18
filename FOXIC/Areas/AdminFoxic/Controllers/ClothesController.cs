using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using FOXIC.Entities.SliderModel;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Pronia.Utilities.Extentions;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class ClothesController : Controller
	{
		public FoxicDb _context;
        private readonly IWebHostEnvironment _env;

       
		public ClothesController(FoxicDb context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}
		
		public IActionResult Index()
		{
			List<Clothing> clothes = _context.Clothes.Include(c=>c.Images).ToList();

			ViewBag.Brands = _context.Brands.ToList();
			ViewBag.Collections = _context.Collections.ToList();
			ViewBag.Categories = _context.Categories.ToList();
            return View(clothes);
		}

		public IActionResult Details(int Id)
		{
			Clothing clothing = _context.Clothes.Include(c=>c.Images).FirstOrDefault(c=>c.Id.Equals(Id));

            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Collections = _context.Collections.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            return View(clothing);
		}

		public IActionResult Create()
		{
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Collections = _context.Collections.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public async Task<IActionResult> Creating(Clothing createdClothing)
		{
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Collections = _context.Collections.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
			ViewBag.Tags = _context.Tags.ToList();

			if (createdClothing.iff_IsMainImage.Equals(null))
			{
				ModelState.AddModelError("iff_IsMainImage", "Please choose main image");
				return View();
			}
            if(createdClothing.iff_Images.Equals(null)) 
            {
				ModelState.AddModelError("iff_Images", "Please choose images");
				return View();
			}
            if (!createdClothing.iff_IsMainImage.IsValidFile("image/"))
            {
                ModelState.AddModelError("iff_IsMainImage", "Please choose image as: jpg, png...");
                return View();
            }
            if (!createdClothing.iff_IsMainImage.IsValidLength(2))
            {
                ModelState.AddModelError("iff_IsMainImage", "Please choose image which size is maximum 2MB");
                return View();
            }

			Clothing clothing = new()
            {
                Name = createdClothing.Name,
                Price = createdClothing.Price,
                Discount = createdClothing.Discount,
                Description = createdClothing.Description,
                Stock = createdClothing.Stock,
                SKU = createdClothing.SKU,
                Barcode = createdClothing.Barcode,
                BrandId = createdClothing.BrandId,
                CollectionId = createdClothing.CollectionId,
                CategoryId = createdClothing.CategoryId,
            };

            string imageFolderPath = Path.Combine(_env.WebRootPath, "images");
            foreach (IFormFile image in createdClothing.iff_Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(2))
                {
                    TempData["InvalidImages"] += image.FileName;
                    continue;
                }

                ProductImage newImage = new()
                {
                    IsMain = false,
                    Path = await image.CreateImage(imageFolderPath, "Clothing")
                };

                clothing.Images.Add(newImage);
            }

            ProductImage mainImage = new()
            {
                IsMain = true,
                Path = await createdClothing.iff_IsMainImage.CreateImage(imageFolderPath, "Clothing")
            };

            clothing.Images.Add(mainImage);

            foreach (int id in createdClothing.TagsIds)
            {
                ClothingTag tag = new()
                {
                    TagId = id
                };
                clothing.ClothingTags.Add(tag);
            }

            if (createdClothing.ColorSizeQuantity is null)
            {
                ModelState.AddModelError("ColorSizeQuantity", "Please Select Color,Size and Quantity");
                return View();
            }
            else
            {
                string[] colorSizeQuantities = createdClothing.ColorSizeQuantity.Split(',');
                foreach (string colorSizeQuantity in colorSizeQuantities)
                {
                    string[] datas = colorSizeQuantity.Split('-');
                    ClothingColorSize clothingColorSize = new()
                    {
                        SizeId = int.Parse(datas[0]),
                        ColorId = int.Parse(datas[1]),
                        Quantity = int.Parse(datas[2])
                    };
                    if (clothingColorSize.Quantity > 0)
                    {
                        clothing.InStock = true;
                        clothing.Stock += clothingColorSize.Quantity;
                    }
                    clothing.ClothingColorSizes.Add(clothingColorSize);
                }
            }

            _context.Clothes.Add(clothing);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            ViewBag.Brands = _context.Brands.ToList();
            ViewBag.Collections = _context.Collections.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            ViewBag.ColorSizeQuantity = _context.ClothingColorSizes.ToList();

            Clothing clothing = _context.Clothes
				.Include(c => c.Images)
					.Include(c=>c.ClothingTags)
						.Include(c=>c.Collection)
							.Include(c=>c.Brand)
							.Include(c=>c.ClothingColorSizes)
							.FirstOrDefault(c => c.Id == Id);

			return View(clothing);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Editing(int Id,Clothing editedClothing)
        {
            if(Id.Equals(0)) return BadRequest();
			Clothing clothing = _context.Clothes.Include(c=>c.Images).FirstOrDefault(c => c.Id == Id);
			if (clothing.Equals(null)) return BadRequest();

			List<string> removables = clothing.Images.Where(c => !editedClothing.ImagesIds.Contains(c.Id)).Select(i => i.Path).ToList();
			var imagefolderPath = Path.Combine(_env.WebRootPath, "images");

			foreach (string removable in removables)
			{
				string path = Path.Combine(imagefolderPath, "Clothing", removable);
				FileUpload.DeleteImage(path);
			}

			if (editedClothing.iff_IsMainImage is not null)
			{
				if (!editedClothing.iff_IsMainImage.IsValidFile("image/"))
				{
					ModelState.AddModelError(string.Empty, "Please choose image file");
					return View();
				}
				if (!editedClothing.iff_IsMainImage.IsValidLength(2))
				{
					ModelState.AddModelError(string.Empty, "Please choose image which size is maximum 2MB");
					return View();
				}
				await AdjustPlantPhoto(true, editedClothing.iff_IsMainImage, clothing);
			}

			string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");

            if (editedClothing.Images is not null)
            {
				foreach (IFormFile image in editedClothing.iff_Images)
				{
					if (!image.IsValidFile("image/") || !image.IsValidLength(2))
					{
						TempData["NonSelect"] += image.FileName;
						continue;
					}
					ProductImage simpleImage = new()
					{
						Path = await image.CreateImage(imagesFolderPath, "Clothing"),
						IsMain = false,
						ClothingId = clothing.Id
					};
					clothing.Images.Add(simpleImage);
				}
			}

			if (editedClothing.TagsIds != null)
			{
				clothing.ClothingTags.RemoveAll(pt => !editedClothing.TagsIds.Contains(pt.TagId));
				foreach (int tagId in editedClothing.TagsIds)
				{
					Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
					if (tag is not null)
					{
						ClothingTag clothingTag = new() { Tag = tag };
						clothing.ClothingTags.Add(clothingTag);
					}
				}
			}

			if (editedClothing.ColorSizeQuantity != null)
			{
				string[] colorSizeQuantities = editedClothing.ColorSizeQuantity.Split(',');
				foreach (string colorSizeQuantity in colorSizeQuantities)
				{
					string[] datas = colorSizeQuantity.Split('-');
					ClothingColorSize clothingColorSize = new()
					{
						SizeId = int.Parse(datas[0]),
						ColorId = int.Parse(datas[1]),
						Quantity = int.Parse(datas[2])
					};
					if (clothingColorSize.Quantity > 0)
					{
						clothing.InStock = true;
						clothing.Stock += clothingColorSize.Quantity;
					}

					var existingItem = clothing.ClothingColorSizes.FirstOrDefault(c => c.SizeId == clothingColorSize.SizeId && c.ColorId == clothingColorSize.ColorId);
					if (existingItem != null)
					{
						existingItem.Quantity = clothingColorSize.Quantity;
					}
					else
					{
						clothing.ClothingColorSizes.Add(clothingColorSize);
					}
				}
			}

			clothing.Name = editedClothing.Name;
			clothing.Price = editedClothing.Price;
			clothing.Discount = editedClothing.Discount;
			clothing.Description = editedClothing.Description;
			clothing.SKU = editedClothing.SKU;
			clothing.Barcode = editedClothing.Barcode;
			clothing.BrandId = editedClothing.BrandId;
			clothing.CollectionId = editedClothing.CollectionId;
			clothing.CategoryId = editedClothing.CategoryId;
			clothing.TagsIds = editedClothing.TagsIds;

			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		private async Task AdjustPlantPhoto(bool? isMain, IFormFile image, Clothing clothing)
		{
			var imagefolderPath = Path.Combine(_env.WebRootPath, "images");
			string filepath = Path.Combine(imagefolderPath, "Clothing", clothing.Images.FirstOrDefault(p => p.IsMain == isMain).Path);
			FileUpload.DeleteImage(filepath);
			clothing.Images.FirstOrDefault(p => p.IsMain == isMain).Path = await image.CreateImage(imagefolderPath, "Clothing");
		}
	}
}
