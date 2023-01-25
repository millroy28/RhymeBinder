using Microsoft.AspNetCore.Http;

namespace RhymeBinder.Models
{
    public class ImportEntry
    {
      //  public BufferedSingleFileUploadDb FileObject { get; set; }
        public IFormFile File { get; set; }
        /*  Hacky - Form does not pass files into this object, so I am using HttpContext.Request.Form.Files to get the file contents
         *  However, if I remove this object or the reference to it form the form it no longer works.
         */
        public int BinderId { get; set; }
        public int UserId { get; set; }
        public string TitleDerivationMethod { get; set; }
        public string CreateNewBinderForImport { get; set; }
        public string NewBinderName { get; set; }
    }
}
