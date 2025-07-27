using PhiRedactor.Application.Services.Interfaces;
using System.Text.RegularExpressions;

namespace PhiRedactor.Application.Services
{
    public class PhiRegexService : IRegexService
    {
        /// <summary>
        /// These are the patterns I used. There are certainly more and these wouldn't work if the file was setup a different way.
        /// While being aware of that, I also started to think how those possibilities would literally be endless. 
        /// </summary>
        private static readonly Dictionary<string, string> Patterns = new()
        {
            { "PatientName", @"(?<=\bPatient Name[:\s]*)([A-Z][a-z]+(?:\s[A-Z][a-z]+)*|[A-Z][a-z]+,\s*[A-Z][a-z]+)" },
            { "DateOfBirth", @"(?<=\bDate of Birth[:\s]*)(\d{1,2}[/-]\d{1,2}[/-]\d{2,4})" },
            { "SSN", @"(?<=\bSocial Security Number[:\s]*)(\d{3}-\d{2}-\d{4})" },
            { "Address", @"(?<=\bAddress[:]\s*)(.+)" },
            { "Phone", @"(?<=\bPhone Number[:\s]*)(\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4})" },
            { "Email", @"(?<=\bEmail[:\s]*)([A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,})" },
            { "MRN", @"(?<=\bMedical Record Number[:\s]*)(MRN-[\w\d]+)" }
        };

        /// <summary>
        /// Redacts sensitive information from the uploaded files using regex patterns.
        /// </summary>
        /// <param name="input">The file with sensitive data.</param>
        /// <returns>The file with sensitive info redacted.</returns>
        public string RedactPatterns(string input)
        {
            ArgumentNullException.ThrowIfNull(nameof(input));

            if (input.Length == 0)
            {
                return input;
            }

            string redactedString = input;

            foreach (KeyValuePair<string, string> pattern in Patterns)
            {
                redactedString = Regex.Replace(redactedString, pattern.Value, "[REDACTED]", RegexOptions.IgnoreCase);
            }

            return redactedString;
        }
    }
}
