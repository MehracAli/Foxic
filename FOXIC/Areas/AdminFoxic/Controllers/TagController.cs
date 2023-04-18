using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class TagController : Controller
	{
		public FoxicDb _context;

		public TagController(FoxicDb context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			IEnumerable<Tag> tags = _context.Tags.AsEnumerable();
			return View(tags);
		}

		public IActionResult Details(int Id)
		{
			Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
			return View(tag);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public IActionResult Creating(Tag createdTag)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Tags.Any(t => t.Name == createdTag.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate tag name");
				return View();
			}

			_context.Tags.Add(createdTag);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int Id)
		{
			Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
			return View(tag);
		}

		[HttpPost]
		[ActionName("Edit")]
		public IActionResult Editing(Tag editedTag, int Id)
		{
			if (Id != editedTag.Id) return BadRequest();
			Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);

			if (!ModelState.IsValid)
			{
				return View();
			}

			bool duplicate = _context.Tags.Any(t => t.Name == editedTag.Name && editedTag.Name != tag.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate tag name");
				return View();
			}
			tag.Name = editedTag.Name;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int Id)
		{
			Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
			return View(tag);
		}

		[HttpPost]
		[ActionName("Delete")]
		public IActionResult Deleting(Tag deletedTag, int Id)
		{
			if (Id != deletedTag.Id) return NotFound();
			Tag tag = _context.Tags.FirstOrDefault(t => t.Id == Id);
			if (tag is null) return NotFound();

			_context.Tags.Remove(tag);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
