using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RhymeBinder.Models;


namespace RhymeBinder.Controllers
{
    public class FileImportController : Controller
    {
        private readonly RhymeBinderContext _context;

        public FileImportController(RhymeBinderContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Import(int userId)
        {
            DisplayInputForm displayInputForm = new DisplayInputForm()
            {
                UserId = userId,
                Binders = _context.Binders.Where(x => x.UserId == userId).ToList()
            };
            
            return View(displayInputForm);
        }
        [HttpPost]
        public async Task<IActionResult> Import(ImportEntry import)
        {
            var files = HttpContext.Request.Form.Files;

            //using (var memoryStream = new MemoryStream())
            //{
            //   // await import.File.CopyToAsync(memoryStream);

            //    // Upload the file if less than 2 MB
            //    if (memoryStream.Length < 2097152)
            //    {
            //        var file = new AppFile()
            //        {
            //            Content = memoryStream.ToArray()
            //        };

            //        //_dbContext.File.Add(file);

            //        //await _dbContext.SaveChangesAsync();
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("File", "The file is too large.");
            //    }
            //}
            return View();
        }
    }
}
