using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class SizeController : Controller
	{
		public FoxicDb _context;

		public SizeController(FoxicDb context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			IEnumerable<Size> sizes = _context.Sizes.AsEnumerable();
			return View(sizes);
		}

		public IActionResult Details(int Id)
		{
			Size size = _context.Sizes.FirstOrDefault(b => b.Id == Id);
			return View(size);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public IActionResult Creating(Size createdSize)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Sizes.Any(s => s.Name == createdSize.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate size name");
				return View();
			}
			_context.Sizes.Add(createdSize);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int Id)
		{
			Size size = _context.Sizes.FirstOrDefault(s => s.Id == Id);
			return View(size);
		}

		[HttpPost]
		[ActionName("Edit")]
		public IActionResult Editing(Size editedSize, int Id)
		{
			if (Id != editedSize.Id) return BadRequest();
			Size size = _context.Sizes.FirstOrDefault(s => s.Id == Id);
			if (size is null) return NotFound();

			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Sizes.Any(b => b.Name == editedSize.Name && editedSize.Name != size.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate size name");
				return View();
			}
			size.Name = editedSize.Name;
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
