using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhiRedactor.Application.Orchestrators;
using PhiRedactor.Application.Services;
using System.Text;

namespace PhiRedactor.Tests;

/// <summary>
/// Just wanted to do a test to make sure the orchestrator works but I would have unit tested each service in the real world as well.
/// </summary>
[TestClass]
public class RedactorOrchestratorTests
{
    private PhiRedactorOrchestrator _redactorOrchestrator = null!;
    private PhiRegexService _phiRegexService = null!;
    private TextFileService _textFileService = null!;
    private ILogger<PhiRedactorOrchestrator> _redactorLogger = null!;
    private ILogger<TextFileService> _textFileServiceLogger = null!;

    [TestInitialize]
    public void Setup()
    {
        Dictionary<string, string> inMemorySettings = new Dictionary<string, string>
    {
        { "RedactionSettings:OutputDirectory", "C:\\TestOutput" }
    };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
        .Build();

        _redactorLogger = NullLogger<PhiRedactorOrchestrator>.Instance;
        _textFileServiceLogger = NullLogger<TextFileService>.Instance;
        _phiRegexService = new PhiRegexService();
        _textFileService = new TextFileService(configuration, _textFileServiceLogger);
        _redactorOrchestrator = new PhiRedactorOrchestrator(_phiRegexService, _textFileService, _redactorLogger);
    }

    [TestMethod]
    public async Task RedactorOrchestrator_ShouldRedactAndSaveFiles()
    {
        // Arrange
        string content = "Name: John Smith\nSSN: 012-34-1323\nPhone:555-123-4444\n";
        string fileName = "test.txt";
        IFormFile formFile = CreateTestFormFile(content, fileName);
        List<IFormFile> files = new List<IFormFile> { formFile };

        // Act
        string result = await _redactorOrchestrator.RedactTextFileUsingRegex(files);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Equals("File was redacted and saved successfully"));
    }

    private IFormFile CreateTestFormFile(string content, string fileName)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        MemoryStream stream = new MemoryStream(bytes);

        return new FormFile(stream, 0, bytes.Length, name: "file", fileName: fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
    }
}
