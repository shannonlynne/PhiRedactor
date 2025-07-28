# PhiRedactor

PhiRedactor is a simple app that I built to automatically redact sensitive information from uploaded text files. It scans the file, finds the private info (name, DOB, addresses, SSN, phone, email, and MRN) and replaces it with [REDACTED]. The file is then saved to the Output directory in the root of the project with the original file name with “\_sanitized.txt” added to it.

### Tech Stack

Frontend: React (Create React App), TypeScript, Axios, Material UI

Backend: .NET 6, ASP.NET Core Web API

Tools & Services: Visual Studio, VS Code, CORS, Dependency Injection, Regex, Logging

Dev Tools: Prettier, Git, GitHub

## Top Level File Structure

```bash
PhiRedactor/
├── frontend/                    # React frontend
├── Output/                      # Output directory for redacted files
├── PhiRedactor.Api/             # ASP.NET Core API project
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
├── PhiRedactor.Application/     # Application/business logic layer
│   ├── Services/
│   └── Orchestrators/           # Coordinates the redaction process
├── PhiRedactor.Tests/           # Unit tests for Application layer
├── TestFiles/                   # Sample test input files for redaction
└── README.md                    # Project overview and setup instructions
```

## Sample data

Patient Name: John Doe  
Date of Birth: 01/23/1980  
Social Security Number: 123-45-6789  
Address: 123 Main St, Anytown, USA  
Phone Number: (555) 123-4567  
Email: john.doe@example.com  
Medical Record Number: MRN-0012345  
Order Details:

- Complete Blood Count (CBC)
- Comprehensive Metabolic Panel (CMP)

### Strategy

In the backend, I use a class called PhiRegexService. Inside that, I defined several regular expressions that detect common PHI (Protected Health Information).

Each regex is designed to match real-world formats as accurately as possible while minimizing false positives. The service processes line-by-line and supports both labeled and unlabeled data. If a label (like Name:) is detected, only the value is redacted, not the field name—preserving readability and context. For unlabeled data, full-line redaction will occur based on pattern matches.

The code loops through those patterns and swaps any matches with [REDACTED]. This is a very basic design but it is fast and readable. It’s not designed to cover all PHI or all file formats, but it works well for structured .txt files with field labels.

In the real world, this would a subset of what would need to be done for the the different file formats and setup. This is where I drew the line due to time constraints.

### Assumptions

Text files are structured and labeled (like Patient Name:) and will contain information that needs to be redacted

Regex redaction and PHI provided in the sample text file is enough for this project

Redacted files can live in /Output

Users have basic dev tools (or GitHub Codespaces access)

CORS is needed only during development

You know how to install and run everything but would like me to explain it like I would to someone who did not know

You will use my test files to test. With limited scope, the files that befine with "ValidFile" are the files I used to build the Regex.

### Constraints/Limitations

The four hour time limit

Only supports .txt files

Relies heavily on field labels

No user authentication

Output path needs to be viewable in the project

Not optimized for massive files or performance edge cases

Doesn’t save any data

### What I'd Do With More Time

Application Insights logging

Infrastructure Layer to save data

Azure Blob Storage for Redacted Files

SQL Database for Metadata

Authentication and Authorization with Azure AD

Azure Key Vault for Configuration for secrets

Unit Testing All Services

Frontend Testing

Integration Testing

Handle More File Formats
-Right now the app only accepts plain `.txt` files

Support More File Structures
-No labels, dashes instead of a colon, etc.

Separate Repos for Frontend & Backend

## Running the project Locally

Note: I spent a ton of time trying to set up Github Codespace and did get it working. (I don't count that towards the project.) I just wanted to learn it. However, I had a friend who doesn't use any of these tools (Java dev) follow my instructions to set it up and he was unable to get it working so I omitted it. I know how to use it myself now and if I ever want to use it in the future, I will have more time to get proper instructions on how to use. If you know how to use it of course you can use that instead.

Prerequisites:

1. VS Code

2. Install Node.js (v18+) installed
   You can verify with "node -v" and "npm -v" in your terminal
   Install Node.js.
   If it is not installed:
   Go to https://nodejs.org
   Download the LTS version (v18+)
   Install and restart VS Code if needed
   Open a terminal and check
   "node -v"
   "npm -v"
   If node or npm aren’t recognized, you may need to restart your terminal or add Node to your PATH manually.
   Go to system or user environment variables, go to Path, "Browse", C:\Program Files\nodejs\.

3. NET SDK 6, 8, or 9 (any one of these is fine — tested on 6.0.428)
   Verify with "dotnet --version"
   If you get this error "No .NET SDKs were found."
   Follow the instructions:
   Install the [6.0.428] .NET SDK

Download a .NET SDK:
https://dotnet.microsoft.com/en-us/download/dotnet/6.0

Steps:

1. Clone the repo: https://github.com/shannonlynne/PhiRedactor
   Code -> Clone and grab HTTPs link. Make sure you are in the top PhiRedactor folder.

2. Install any recommended extensions but you don't need Docker or Prettier. A pop up in the bottom right will prompt you.

3. Create a file called .env in the frontend (frontend/.env)
   Copy the example contents from .env.example but replace contents with:
   REACT_APP_API_BASE_URL=http://localhost:5000

Open terminal:
"cd .."
"cd ..PhiRedactor.Api"
"dotnet run"

Swagger: http://localhost:5000/swagger

Open another terminal:
"cd frontend"
"npm install"
"npm start"
It should open on its own

Frontend: http://localhost:3000

4. In the repo open the TestFiles folder. See "frontend/TestFiles" for test filesThere are 4 files to test.

### Final Notes

I did spend a bit more time on this than expected mostly because I took my time learning my way through the React project. React is still fairly new to me though Material UI and Axios helped keep things quite simple.

I also triple-checked the project requirements and aimed to make the README as informative as possible.
