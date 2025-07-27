using Microsoft.AspNetCore.Mvc;
using PhiRedactor.Application.CustomExceptions;
using PhiRedactor.Application.Orchestrators.Interfaces;
using PhiRedactor.Application.Services.Interfaces;

namespace PhiRedactor.Api.Controllers;

//[Authorize(Roles = "Provider")]
[Route("api/redaction/text")]
[ApiController]
public class TextFileRedactionController : ControllerBase
{
    private readonly IRedactorOrchestrator _redactorOrchestrator;
    private readonly ILogger<TextFileRedactionController> _logger;

    public TextFileRedactionController(IRedactorOrchestrator redactorOrchestrator, IFileService fileService, ILogger<TextFileRedactionController> logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(redactorOrchestrator));
        ArgumentNullException.ThrowIfNull(nameof(fileService));
        ArgumentNullException.ThrowIfNull(nameof(logger));

        _redactorOrchestrator = redactorOrchestrator;
        _logger = logger;
    }

    [HttpPost("redact")]
    public async Task<IActionResult> RedactText([FromForm] IFormFileCollection formData)
    {
        string message;
        List<IFormFile> formFiles = formData.ToList();

        List<IFormFile> validatedFiles = new List<IFormFile>();
        try
        {
            message = await _redactorOrchestrator.RedactTextFileUsingRegex(formFiles);
        }
        catch (NoFilesUploadedException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (InvalidFormatException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (EmptyFileException ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Access denied to the output directory: {ex.Message}");
            return StatusCode(500, "Access denied to the output directory. Please check the server configuration.");
        }
        catch (IOException ex)
        {
            _logger.LogError($"An I/O error occurred while saving the file: {ex.Message}");
            return StatusCode(500, "An I/O error occurred while saving the file. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while processing the files: {ex.Message}");
            return StatusCode(500, "There was a problem uploading the files. Please try again in a little bit.");
        }

        _logger.LogInformation(message);
        return Ok(message);
    }
}

