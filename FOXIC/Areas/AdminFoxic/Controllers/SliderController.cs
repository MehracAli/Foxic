using FOXIC.DataAccesLayer;
using FOXIC.Entities.SliderModel;
using Microsoft.AspNetCore.Mvc;
using Pronia.Utilities.Extentions;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
    [Area("AdminFoxic")]
    public class SliderController : Controller
    {
        public FoxicDb _context { get; set; }
        private readonly IWebHostEnvironment _env;


        public SliderController(FoxicDb context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Details(int Id)
        {
            Slider slider = _context.Sliders.SingleOrDefault(s => s.Id == Id);
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Creating(Slider createdSlider)
        {
            if (createdSlider.Title is null)
            {
                return View();
            }

            if (createdSlider.Order.Equals(0))
            {
                return View();
            }

            if (createdSlider.iff_ImagePath is null)
            {
                ModelState.AddModelError("Iff_Path", "You must choose image here");
                return View();
            }

            if (!createdSlider.iff_ImagePath.IsValidFile("image/"))
            {
                ModelState.AddModelError("Iff_Path", "You must coose image formats as: jpg, png etc.");
                return View();
            }

            if (!createdSlider.iff_ImagePath.IsValidLength(2))
            {
                ModelState.AddModelError("Iff_Path", "Image size cant be over 2MB");
                return View();
            }

            Slider slider = new()
            {
                Title = createdSlider.Title,
                Order = createdSlider.Order,
            };

            string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");
            slider.ImagePath = await createdSlider.iff_ImagePath.CreateImage(imagesFolderPath, "slider");

            bool duplicate = _context.Sliders.Any(s => s.Order == createdSlider.Order);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate order");
                return View();
            }

            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            Slider slider = _context.Sliders.SingleOrDefault(s => s.Id == Id);
            return View(slider);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Editing(int Id, Slider editedSlider)
        {
            if (Id != editedSlider.Id) return BadRequest();
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == Id);
            if (slider is null) return NotFound();

            if (editedSlider.Title is null)
            {
                return View();
            }

            if (editedSlider.Order.Equals(0))
            {
                return View();
            }

            if (editedSlider.iff_ImagePath is null)
            {
                bool duplicated = _context.Sliders.Any(s => s.Order == editedSlider.Order && editedSlider.Order != slider.Order);
                if (duplicated)
                {
                    ModelState.AddModelError("Name", "You cannot duplicate order");
                    return View();
                }

                slider.Order = editedSlider.Order;
                slider.Title = editedSlider.Title;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //return Json(editedSlider);

            if (!editedSlider.iff_ImagePath.IsValidFile("image/"))
            {
                ModelState.AddModelError("Iff_Path", "You must coose image formats as: jpg, png etc.");
                return View();
            }

            if (!editedSlider.iff_ImagePath.IsValidLength(2))
            {
                ModelState.AddModelError("Iff_Path", "Image size cant be over 2MB");
                return View();
            }

            string imagesFolderPath = Path.Combine(_env.WebRootPath, "images");
            slider.ImagePath = await editedSlider.iff_ImagePath.CreateImage(imagesFolderPath, "slider");

            bool duplicate = _context.Sliders.Any(s => s.Order == editedSlider.Order && editedSlider.Order != slider.Order);
            if (duplicate)
            {
                ModelState.AddModelError("Name", "You cannot duplicate order");
                return View();
            }

            slider.Title = editedSlider.Title;
            slider.Order = editedSlider.Order;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(c => c.Id == Id);
            return View(slider);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Deleting(Slider deletedSlider, int Id)
        {
            if (Id != deletedSlider.Id) return NotFound();
            Slider? slider = _context.Sliders.FirstOrDefault(s => s.Id == Id);
            if (slider is null) return NotFound();

            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
