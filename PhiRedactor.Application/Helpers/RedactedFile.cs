namespace PhiRedactor.Application.Helpers
{
    public class RedactedFile
    {
        public string Content { get; set; }
        public string FileName { get; set; }

        public RedactedFile(string content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }
    }
}
