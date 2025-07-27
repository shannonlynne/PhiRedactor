using Microsoft.AspNetCore.Http;

namespace PhiRedactor.Application.Services.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Validates that the uploaded files are in the correct format (.txt).
    /// </summary>
    /// <param name="formFiles">The forms uploaded by the provider.</param>
    /// <returns>The same list minus any files that are not .txt.</returns>
    List<IFormFile> ValidateFiles(List<IFormFile> formFiles);

    /// <summary>
    /// Saves the redacted file to the output directory.
    /// </summary>
    /// <param name="redactedContent">The valid .txt files.</param>
    /// <param name="originalFileName">The original file name used to create the output file name.</param>
    Task SaveRedactedFileAsync(string redactedFiles, string originalFileName);


}
