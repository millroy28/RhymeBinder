using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace RhymeBinder.Models
{
    public class BufferedSingleFileUploadDbModel : PageModel
    {
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }
}

    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
