namespace PhiRedactor.Application.Services.Interfaces;

public interface IRegexService
{
    /// <summary>
    /// Redacts patterns from the input string based on predefined regex patterns saved as a dictionary.
    /// </summary>
    /// <param name="input">The string needing redaction.</param>
    /// <returns>The string with PHI redacted.</returns>
    string RedactPatterns(string input);
}
