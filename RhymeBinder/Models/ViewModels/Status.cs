using RhymeBinder.Models.Enums;

namespace RhymeBinder.Models.ViewModels
{
    public class Status
    {
        public bool success { get; set; }
        public string message { get; set; }
        public AlertLevelEnum alertLevel { get; set; } = AlertLevelEnum.INFO;
        public int recordId { get; set; }
        public int userId { get; set; }

    }
}
