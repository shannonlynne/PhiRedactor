namespace PhiRedactor.Application.CustomExceptions
{
    /// <summary>
    /// While I would hope a call isn't made with no files, I am checking against that anyways so it is handled properly
    /// </summary>
    public class NoFilesUploadedException : Exception
    {
        public NoFilesUploadedException(string message)
            : base(message)
        {
        }
    }
}
