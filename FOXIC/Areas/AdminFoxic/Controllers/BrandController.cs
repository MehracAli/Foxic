using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class BrandController : Controller
	{
		public FoxicDb _context;

		public BrandController(FoxicDb context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			IEnumerable<Brand> brands = _context.Brands.AsEnumerable();
			return View(brands);
		}

		public IActionResult Details(int Id)
		{
			Brand brand = _context.Brands.FirstOrDefault(b => b.Id == Id);
			return View(brand);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public IActionResult Creating(Brand createdBrand)
		{
			if (createdBrand.Name is null)
			{
				return View();
			}

			bool duplicate = _context.Brands.Any(b => b.Name == createdBrand.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate brand name");
				return View();
			}

			_context.Brands.Add(createdBrand);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int Id)
		{
			Brand brand = _context.Brands.FirstOrDefault(b => b.Id == Id);
			return View(brand);
		}

		[HttpPost]
		[ActionName("Edit")]
		public IActionResult Editing(Brand editedBrand, int Id)
		{
			if (Id != editedBrand.Id) return BadRequest();
			Brand brand = _context.Brands.FirstOrDefault(b => b.Id == Id);
			if (brand is null) return NotFound();

			if(editedBrand.Name is null)
			{
				return View();
			}

			bool duplicate = _context.Brands.Any(b => b.Name == editedBrand.Name && editedBrand.Name != brand.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate brand name");
				return View();
			}

			brand.Name = editedBrand.Name;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int Id)
		{
			Brand brand = _context.Brands.FirstOrDefault(b => b.Id == Id);
			return View(brand);
		}

		[HttpPost]
		[ActionName("Delete")]
		public IActionResult Deleting(Brand deletedBrand, int Id)
		{
			if (Id != deletedBrand.Id) return NotFound();
			Brand? brand = _context.Brands.FirstOrDefault(b => b.Id == Id);
			if (brand is null) return NotFound();

			_context.Brands.Remove(brand);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
