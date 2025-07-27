namespace PhiRedactor.Application.CustomExceptions
{
    /// <summary>
    /// A user may pass in a file with no data
    /// </summary>
    public class EmptyFileException : Exception
    {
        public EmptyFileException(string message)
            : base(message)
        {
        }
    }
}
