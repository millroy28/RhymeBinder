namespace RhymeBinder.Models
{
    public static class AcceptedImportFileTypes
    {
        public const string PlainText = "text/plain";

        public static bool Contains(string value)
        {
            bool match = false;

            if (value == PlainText)
            {
                match = true;
            }

            return match;
        }
    }
}
