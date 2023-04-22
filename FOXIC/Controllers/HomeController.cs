using FOXIC.DataAccesLayer;
using FOXIC.Entities.SliderModel;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FOXIC.Controllers
{
    public class HomeController : Controller
    {
        public FoxicDb _context { get; set; }

        public HomeController(FoxicDb context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<SliderVM> sliders = _context.Sliders.Select(s=> new SliderVM
            {
                Title = s.Title,
                Order = s.Order,
                ImagePath = s.ImagePath,

            }).OrderBy(s=>s.Order).ToList();

            ViewBag.Clothes = _context.Clothes.Include(c=>c.Images).Include(c=>c.ClothingColorSizes).OrderByDescending(c=>c.Id).ToList();
            ViewBag.Collections = _context.Collections.ToList();
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Sizes = _context.Sizes.ToList();
            ViewBag.Brands = _context.Brands.ToList();
			return View(sliders);
        }
    }
}