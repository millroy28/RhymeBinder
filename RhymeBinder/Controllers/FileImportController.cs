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
                Binders = _context.Binders.Where(x => x.UserId == userId).ToList(),
                Results = new List<DisplayFileImportResult>()

            };
            
            return View(displayInputForm);
        }
        [HttpPost]
        public async Task<IActionResult> Import(DisplayInputForm import)
        {
            var files = HttpContext.Request.Form.Files;

            List<DisplayFileImportResult> results = new List<DisplayFileImportResult>();

            foreach (var file in files)
            {
                DisplayFileImportResult thisResult = new DisplayFileImportResult()
                {
                    FileName = file.FileName,
                    FileType = file.ContentType,
                    ImportStatus = "Failed",
                    FailureMessage = "No one coded this part yet so nothing happened!"
                };
                results.Add(thisResult);
            };



            import.Results = results;


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
            return View(import);
        }
    }
}
