using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PhiRedactor.Application.CustomExceptions;
using PhiRedactor.Application.Helpers;
using PhiRedactor.Application.Orchestrators.Interfaces;
using PhiRedactor.Application.Services.Interfaces;

namespace PhiRedactor.Application.Orchestrators;

public class PhiRedactorOrchestrator : IRedactorOrchestrator
{
    private readonly IRegexService _regexService;
    private readonly IFileService _fileService;
    private readonly ILogger<PhiRedactorOrchestrator> _logger;

    public PhiRedactorOrchestrator(IRegexService regexService, IFileService fileService, ILogger<PhiRedactorOrchestrator> logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(regexService));
        ArgumentNullException.ThrowIfNull(nameof(fileService));
        ArgumentNullException.ThrowIfNull(nameof(logger));

        _regexService = regexService;
        _fileService = fileService;
        _logger = logger;
    }

    /// <summary>
    /// Calls the RegexService to redact the files and calls the file service to save the redacted files.
    /// </summary>
    /// <param name="formFiles"></param>
    /// <exception cref="Exception">If an error is thrown during redaction and saving.</exception>
    public async Task<string> RedactTextFileUsingRegex(List<IFormFile> formFiles)
    {
        ArgumentNullException.ThrowIfNull(nameof(formFiles));

        List<IFormFile> validatedFiles = new List<IFormFile>();
        try
        {
            validatedFiles = _fileService.ValidateFiles(formFiles);
        }
        catch (Exception)
        {
            _logger.LogError("An error occurred while validating the files.");
            throw;
        }

        List<RedactedFile> redactedFiles = new List<RedactedFile>();
        foreach (IFormFile file in validatedFiles)
        {
            try
            {
                using StreamReader reader = new StreamReader(file.OpenReadStream());
                string fileContent = await reader.ReadToEndAsync();

                if (string.IsNullOrEmpty(fileContent))
                {
                    continue;
                }

                string redactedContent = _regexService.RedactPatterns(fileContent);
                redactedFiles.Add(new RedactedFile(redactedContent, file.FileName));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the file {file.FileName}: {ex.Message}");
                throw new Exception($"An error occurred while processing the file {file.FileName}: {ex.Message}", ex);
            }
        }

        if (redactedFiles.Count > 0)
        {
            foreach (RedactedFile file in redactedFiles)
            {
                try
                {
                    await _fileService.SaveRedactedFileAsync(file.Content, file.FileName);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        else
        {
            _logger.LogError("No files were redacted. User uploaded only an empty .txt files.");
            throw new EmptyFileException("No files were redacted. Please upload only non-empty .txt files.");
        }

        int filesNotRedactedCount = formFiles.Count - redactedFiles.Count;

        //If there are redated files and some are missing, I still want to make sure I get them the correct message. 
        //Also wanted simple messages when there were no errors
        return filesNotRedactedCount > 0
                ? filesNotRedactedCount > 1
                    ? $"{redactedFiles.Count} file(s) redacted and saved successfully. " +
                    $"{filesNotRedactedCount}  files were not the correct .txt file type or didn't contain any data"
                    : $"{redactedFiles.Count} file(s) redacted and saved successfully. " +
                    $"{filesNotRedactedCount}  file was not the correct .txt file type or didn't contain any data"
                : redactedFiles.Count > 1
                    ? $"All files were redacted and saved successfully"
                    : $"File was redacted and saved successfully";
    }
}
