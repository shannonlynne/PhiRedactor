namespace PhiRedactor.Application.CustomExceptions
{
    /// <summary>
    /// Files need to be in the correct format whether it's .txt or later if .pdf, .docx or any other format is added
    /// </summary>
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message)
            : base(message)
        {
        }
    }
}
