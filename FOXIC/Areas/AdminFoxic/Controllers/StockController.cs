using FOXIC.DataAccesLayer;
using FOXIC.Entities.ClothingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
    [Area("AdminFoxic")]
    public class StockController : Controller
    {
        public FoxicDb _context;

        public StockController(FoxicDb context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Clothes = _context.Clothes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            List<ClothingColorSize> clothingColorSizes = _context.ClothingColorSizes
                            .ToList();
            return View(clothingColorSizes);
        }

        public IActionResult Edit(int Id) 
        {
            ViewBag.Clothes = _context.Clothes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            if (Id == 0) return BadRequest();
            ClothingColorSize clothingColorSize = _context.ClothingColorSizes.FirstOrDefault(c => c.Id == Id);
            if (clothingColorSize == null) return NotFound();

            return View(clothingColorSize);
        }

        public IActionResult Delete(int Id)
        {
            ViewBag.Clothes = _context.Clothes.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();

            if (Id == 0) return BadRequest();
            ClothingColorSize? clothingColorSize = _context.ClothingColorSizes
                .Include(cs=>cs.Clothing)
                    .Include(cs=>cs.Color)
                        .Include(cs=>cs.Size)
                            .FirstOrDefault(c => c.Id == Id);
            if (clothingColorSize == null) return NotFound();

            return View(clothingColorSize);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult Editing(int Id, ClothingColorSize edited)
        {
            if (Id == 0) return BadRequest();
            ClothingColorSize? clothingColorSize = _context.ClothingColorSizes.FirstOrDefault(deleted => deleted.Id == Id);
            if (clothingColorSize == null) return NotFound();

            clothingColorSize.ColorId = edited.ColorId;
            clothingColorSize.SizeId = edited.SizeId;
            clothingColorSize.Quantity = edited.Quantity;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Deleting(int Id, ClothingColorSize deleted)
        {
            if (Id == 0) return BadRequest();
            ClothingColorSize? clothingColorSize = _context.ClothingColorSizes.FirstOrDefault(deleted => deleted.Id == Id);
            if(clothingColorSize == null) return NotFound();

            _context.ClothingColorSizes.Remove(clothingColorSize);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
