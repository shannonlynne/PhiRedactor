using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PhiRedactor.Application.CustomExceptions;
using PhiRedactor.Application.Services.Interfaces;

namespace PhiRedactor.Application.Services;

public class TextFileService : IFileService
{
    private string _outputDirectory = null!;
    private readonly ILogger<TextFileService> _logger;

    public TextFileService(IConfiguration configuration, ILogger<TextFileService> logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(configuration));
        ArgumentNullException.ThrowIfNull(nameof(logger));

        _logger = logger;

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        string configuredPath = configuration["RedactionSettings:OutputDirectory"]
            ?? throw new InvalidOperationException("No output directory has been specified.");

        string basePath = AppContext.BaseDirectory;
        _outputDirectory = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", configuredPath));

        if (!Directory.Exists(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }

    /// <summary>
    /// Validates that the uploaded files are in the correct format (.txt).
    /// </summary>
    /// <param name="formFiles">The list of files uploaded.</param>
    /// <returns>The list of .txt files.</returns>
    /// <exception cref="NoFilesUploadedException">Thrown if there are no files uploaded.</exception>
    /// <exception cref="InvalidFormatException">Thrown if there are no .txt files.</exception>
    public List<IFormFile> ValidateFiles(List<IFormFile> formFiles)
    {
        ArgumentNullException.ThrowIfNull(nameof(formFiles));

        if (formFiles.Count == 0)
        {
            throw new NoFilesUploadedException("No files were uploaded.");
        }

        List<IFormFile> files = formFiles.Where(file => file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) == true).ToList();

        return !files.Any()
            ? throw new InvalidFormatException("There were no .txt files uploaded.")
            : files;
    }

    /// <summary>
    /// Saves the redacted file to the output directory.
    /// </summary>
    /// <param name="redactedContent">The redacted content of the file.</param>
    /// <param name="originalFileName">The original file name used to create the output file name.</param>
    public async Task SaveRedactedFileAsync(string redactedFile, string originalFileName)
    {
        ArgumentNullException.ThrowIfNull(nameof(redactedFile));
        ArgumentNullException.ThrowIfNull(nameof(originalFileName));

        if (redactedFile.Length == 0)
        {
            throw new ArgumentNullException(nameof(redactedFile), "Redacted file content cannot be null or empty.");
        }
        string redactedFileName = Path.GetFileNameWithoutExtension(originalFileName) + "_sanitized.txt";

        string outputPath = Path.Combine(_outputDirectory, redactedFileName);

        try
        {
            await File.WriteAllTextAsync(outputPath, redactedFile);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Access denied to the output directory: {ex.Message}");
            throw;
        }
        catch (IOException ex)
        {
            _logger.LogError($"An I/O error occurred while saving the file: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while saving the file: {ex.Message}");
            throw new Exception($"An unexpected error occurred while saving the file: {ex.Message}", ex);
        }
    }
}
