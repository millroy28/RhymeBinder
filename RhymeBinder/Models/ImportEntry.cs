using Microsoft.AspNetCore.Http;

namespace RhymeBinder.Models
{
    public class ImportEntry
    {
      //  public BufferedSingleFileUploadDb FileObject { get; set; }
        public IFormFile File { get; set; }
        public bool UseFirstLineForName { get; set; }
        public int BinderId { get; set; }
        public int UserId { get; set; }
        public bool CreateNewBinderForImport { get; set; }
        public string NewBinderName { get; set; }
    }
}
