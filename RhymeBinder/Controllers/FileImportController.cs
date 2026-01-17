using Microsoft.AspNetCore.Mvc;
// using Microsoft.Web.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RhymeBinder.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.ViewModels;
using RhymeBinder.Models.DTOModels;
using RhymeBinder.Models.ImportModels;
using RhymeBinder.Models.HelperModels;

namespace RhymeBinder.Controllers
{
    public class FileImportController : Controller
    {
        private readonly RhymeBinderContext _context;
        public ModelHelper _modelHelper;

        public FileImportController(RhymeBinderContext context, ModelHelper modelHelper)
        {
            _context = context;
            _modelHelper = modelHelper; 
        }
        [HttpGet]
        public IActionResult Import(int userId)
        {
            DisplayInputForm displayInputForm = new DisplayInputForm()
            {
                UserId = userId,
                Binders = _context.Binders.Where(x => x.UserId == userId && x.Hidden == false).ToList(),
                Results = new List<DisplayFileImportResult>()

            };
            
            return View(displayInputForm);
        }
        [HttpPost]
        public IActionResult Import(DisplayInputForm import)
        {
            var files = HttpContext.Request.Form.Files;
            List<DisplayFileImportResult> results = new List<DisplayFileImportResult>();
            int binderId = import.ImportEntry.BinderId;

            // Build new binder or use specified Binder Id
            if (import.ImportEntry.CreateNewBinderForImport == "on")
            {
                Binder newBinder = new Binder()
                {
                    Name = import.ImportEntry.NewBinderName
                };
                Status status = _modelHelper.BinderHelper.CreateNewBinder(import.ImportEntry.UserId, newBinder);
                binderId = status.recordId;               
            };

            // Begin attempting import
            if (files.Count() > 0)
            {
                foreach(var file in files)
                {
                    // prep results
                    DisplayFileImportResult result = new DisplayFileImportResult()
                    {
                        FileName = file.FileName,
                        FileType = file.ContentType
                    };

                    // Check file type before attempting
                    if (AcceptedImportFileTypes.Contains(file.ContentType))
                    {
                        bool success = ImportFile(file, import.ImportEntry.UserId, binderId, import.ImportEntry.TitleDerivationMethod);

                        if (success)
                        {
                            result.ImportStatus = "Success!";
                        }
                        else
                        {
                            result.ImportStatus = "Failed!";
                            result.FailureMessage = "Attempt to import failed for unknow reason";
                        }
                    } else
                    {
                        result.ImportStatus = "Failed!";
                        result.FailureMessage = "File type unsupported";
                    }
                    results.Add(result);
                }
            };

            import.Results = results;
            
            return View(import);
        }

        public bool ImportFile(IFormFile file, int userId, int binderId, string titleDerivationMethod)
        {
            bool success = false;

            try
            {
                TextHeaderBodyPair textHeaderBodyPair = new TextHeaderBodyPair();

                // Get file contents into header/body objects
                if (file.ContentType == AcceptedImportFileTypes.PlainText)
                {
                    textHeaderBodyPair = ConvertPlainTextToHeaderBodyPair(file, titleDerivationMethod);
                }

                // Fill out body values
                Text newText = new Text()
                {
                    TextBody = textHeaderBodyPair.Body.TextBody,
                    Created = DateTime.Now
                };

                if (ModelState.IsValid)
                {
                    _context.Texts.Add(newText);
                    _context.SaveChanges();
                }

                // Fill out header values
                TextHeader newHeader = new TextHeader()
                {
                    Title = textHeaderBodyPair.Header.Title,
                    Created = DateTime.Now,
                    CreatedBy = userId,
                    LastModified = DateTime.Now,
                    LastModifiedBy = userId,
                    LastRead = DateTime.Now,
                    LastReadBy = userId,
                    //TextRevisionStatusId = 1,
                    VisionNumber = 1,
                    VersionOf = null,
                    Deleted = false,
                    Locked = false,
                    Top = true,
                    BinderId = binderId,
                    TextId = newText.TextId
                };

                if (ModelState.IsValid)
                {
                    _context.TextHeaders.Add(newHeader);
                    _context.SaveChanges();
                    // yay, we did it!
                    success = true;
                }

            }
            catch
            {
                success = false;
            }
            return success;
        }
        
        public TextHeaderBodyPair ConvertPlainTextToHeaderBodyPair(IFormFile file, string titleDerivationMethod)
        {
            string title = "";

            var contents = new StringBuilder();
             using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    contents.AppendLine(reader.ReadLine());
                }

            if (titleDerivationMethod == "filename")
            {
                int extPeriod = file.FileName.IndexOf('.');
                if (extPeriod > 0)
                {
                    title = file.FileName.Substring(0, extPeriod);
                }
                else
                {
                    title = file.FileName;
                };
            }
            if(titleDerivationMethod == "firstline")
            {
                bool textFound = false;
                string contentString = contents.ToString();

                int lineStart = 0;
                int maxLength = contentString.Length;
                int lineBreak = contentString.IndexOf("\r\n");
                string line = contentString.Substring(lineStart, lineBreak);

                while(lineStart < maxLength && !textFound)
                {
                    if(line.Trim().Length > 0)
                    {
                        title = line;
                        textFound = true;
                    }
                    lineStart = lineBreak + 1;
                    lineBreak = contentString.IndexOf("\r\n", lineStart);
                    line = contentString.Substring(lineStart, lineBreak);
                }

                if (!textFound)
                {
                    title = "title";
                }
            }

            TextHeaderBodyPair textHeaderBodyPair = new TextHeaderBodyPair()
            {
                Header = new TextHeader()
                {
                    Title = title
                },
                Body = new Text()
                {
                    TextBody = contents.ToString()
                }
            };
            return textHeaderBodyPair;
        }
    }
}
