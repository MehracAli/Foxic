using FOXIC.DataAccesLayer;
using FOXIC.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FOXIC.Areas.AdminFoxic.Controllers
{
    [Area("AdminFoxic")]
    public class SettingController : Controller
    {
        public FoxicDb _context;

        public SettingController(FoxicDb context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Setting> settings = _context.Settings.ToList();
            return View(settings);
        }

        public IActionResult Detail(int Id)
        {
            if (Id == 0) return BadRequest();
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == Id);
            if (setting == null) NotFound();

            return View(setting);
        }

        public IActionResult Edit(int Id)
        {
            if (Id == 0) return BadRequest();
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == Id);
            if (setting == null) NotFound();

            return View(setting);
        }

        public IActionResult Editing(int Id, Setting editedSetting)
        {
            if (Id == 0) return BadRequest();
            Setting setting = _context.Settings.FirstOrDefault(s => s.Id == Id);
            if (setting == null) NotFound();

            setting.Key = editedSetting.Key;
            setting.Value = editedSetting.Value;

            return View(setting);
        }
    }
}
