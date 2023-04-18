using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class CategoryController : Controller
	{
		public FoxicDb _context;

        public CategoryController(FoxicDb context)
        {
			_context = context;
        }

        public IActionResult Index()
		{
			IEnumerable<Category> categories = _context.Categories.AsEnumerable();
			return View(categories);
		}

		public IActionResult Details(int Id)
		{
			Category category = _context.Categories.FirstOrDefault(c => c.Id == Id);
			return View(category);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public IActionResult Creating(Category createdCategory)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Categories.Any(c => c.Name == createdCategory.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate category name");
				return View();
			}
			_context.Categories.Add(createdCategory);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int Id)
		{
			Category category = _context.Categories.FirstOrDefault(c => c.Id == Id);
			return View(category);
		}

		[HttpPost]
		[ActionName("Edit")]
		public IActionResult Editing(Category editedCategory,int Id)
		{
			if (Id != editedCategory.Id) return BadRequest();
			Category category = _context.Categories.FirstOrDefault(c => c.Id == Id);
			if (category is null) return NotFound();

			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Categories.Any(c => c.Name == editedCategory.Name && editedCategory.Name != category.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate category name");
				return View();
			}
			category.Name = editedCategory.Name;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int Id) 
		{
			Category category = _context.Categories.FirstOrDefault(c=>c.Id == Id);
			return View(category);
		}

		[HttpPost]
		[ActionName("Delete")]
		public IActionResult Deleting(Category deletedCategory,int Id)
		{
			if (Id != deletedCategory.Id) return NotFound();
			Category? category = _context.Categories.FirstOrDefault(c => c.Id == Id);
			if (category is null) return NotFound();
			_context.Categories.Remove(category);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
