using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;
using Pronia.Utilities.Extentions;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
	[Area("AdminFoxic")]
	public class ColorController : Controller
	{
		public FoxicDb _context;
		private readonly IWebHostEnvironment _env;

		public ColorController(FoxicDb context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public IActionResult Index()
		{
			IEnumerable<Color> colors = _context.Colors.AsEnumerable();
			return View(colors);
		}

		public IActionResult Details(int Id)
		{
			Color color = _context.Colors.FirstOrDefault(c => c.Id == Id);
			return View(color);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		public async  Task<IActionResult> Creating(Color createdColor)
		{
			if(createdColor.Name is null)
			{
				return View();
			}

			if (createdColor.iff_Path is null)
			{
				ModelState.AddModelError("Iff_Path", "You must choose image here");
				return View();
			}

			if (!createdColor.iff_Path.IsValidFile("image/"))
			{
				ModelState.AddModelError("Iff_Path", "You must coose image formats as: jpg, png etc.");
				return View();
			}

			if (!createdColor.iff_Path.IsValidLength(2))
			{
				ModelState.AddModelError("Iff_Path", "Image size cant be over 2MB");
				return View();
			}

			Color color = new()
			{
				Name = createdColor.Name,
			};

			string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");
			color.Path = await createdColor.iff_Path.CreateImage(imagesFolderPath, "Colors");

			bool duplicate = _context.Colors.Any(c => c.Name == createdColor.Name);
			if (duplicate)
			{
				ModelState.AddModelError("", "You cannot duplicate color name");
				return View();
			}
			//return Json(color);
			_context.Colors.Add(color);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Edit(int Id)
		{
			Color color = _context.Colors.FirstOrDefault(c => c.Id == Id);
			return View(color);
		}

		[HttpPost]
		[ActionName("Edit")]
		public async Task<IActionResult> Editing(Color editedColor, int Id)
		{
			if (Id != editedColor.Id) return BadRequest();
			Color color = _context.Colors.FirstOrDefault(c => c.Id == Id);
			if (color is null) return NotFound();

			if (editedColor.Name is null)
			{
				return View();
			}

			//===Image Edit===//
			if (editedColor.iff_Path is null)
			{	
				bool duplicated = _context.Colors.Any(c => c.Name == editedColor.Name && editedColor.Name != color.Name);
				if (duplicated)
				{
					ModelState.AddModelError("Name", "You cannot duplicate color name");
					return View();
				}
				
				color.Name = editedColor.Name;
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			if (!editedColor.iff_Path.IsValidFile("image/"))
			{
				ModelState.AddModelError("Iff_Path", "You must coose image formats as: jpg, png etc.");
				return View();
			}

			if (!editedColor.iff_Path.IsValidLength(2))
			{
				ModelState.AddModelError("Iff_Path", "Image size cant be over 2MB");
				return View();
			}

			string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");
			color.Path = await editedColor.iff_Path.CreateImage(imagesFolderPath, "Colors");

			//===Name Edit===//

			bool duplicate = _context.Colors.Any(c => c.Name == editedColor.Name && editedColor.Name != color.Name);
			if (duplicate)
			{
				ModelState.AddModelError("Name", "You cannot duplicate category name");
				return View();
			}

			color.Name = editedColor.Name;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int Id)
		{
			Color color = _context.Colors.FirstOrDefault(c => c.Id == Id);
			return View(color);
		}

		[HttpPost]
		[ActionName("Delete")]
		public IActionResult Deleting(Color deletedColor, int Id)
		{
			if (Id != deletedColor.Id) return NotFound();
			Color? color = _context.Colors.FirstOrDefault(c => c.Id == Id);
			if (color is null) return NotFound();

			_context.Colors.Remove(color);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
