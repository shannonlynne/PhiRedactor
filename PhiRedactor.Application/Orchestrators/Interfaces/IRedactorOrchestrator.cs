using Microsoft.AspNetCore.Http;

namespace PhiRedactor.Application.Orchestrators.Interfaces;

public interface IRedactorOrchestrator
{
    /// <summary>
    /// Redacts sensitive information from the uploaded files using regex patterns.
    /// </summary>
    /// <param name="formFiles">The list of files that need redaction</param>
    Task<string> RedactTextFileUsingRegex(List<IFormFile> formFiles);
}
