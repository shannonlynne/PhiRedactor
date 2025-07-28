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

# Running the project

## I have included 2 sets of directions. The easiest way is through Github Codespace but I also supplied directions for running it locally.

### GitHub Codespaces

Easiest Way to Run (One Click)

Go to the repo and click the green "Code" butto
Select "Open with Codespaces" → "New Codespace"

After a few minutes, the app will be ready

Navigate to the frontend/ folder
Create a file called .env
Copy the example contents from .env.example
REACT_APP_API_BASE_URL=https://<your-codespace-name>-5000.app.github.dev
(Replace <your-codespace-name> with your actual preview URL)

Restart the frontend
"cd frontend" then
"npm start"

"Ctrl-Shift-P", Run Task in Command Palette then Run Both (Frontend + Backend)

Make Backend Port Public - a popup should come up in the bottom right and there will be an option to “Make Public” but you can also change it in the Ports tab in the bottom panel where the Terminal is located.
Right-click port 5000, choose Port Visibility -> Public
\*Without this, CORS will fail and the app won’t work!

In the bottom right it should prompt you to open the browser on 3000 but if not, here are the links for the front end and swagger if you want to test there.

Frontend: https://<your-codespace>-3000.app.github.dev

Backend: https://<your-codespace>-5000.app.github.dev/swagger

### Run Locally

You can also run the app on your machine.

Prerequisites:

1. Install Node.js (v18+) installed
   You can verify with node -v and npm -v in your terminal
   Install Node.js.
   If it is not installed:
   Go to https://nodejs.org
   Download the LTS version (v18+)
   Install and restart VS Code if needed
   Open a terminal and check
   "node -v"
   "npm -v"
   If node or npm aren’t recognized, you may need to restart your terminal or add Node to your PATH manually.

2. NET SDK 6, 8, or 9 (any one of these is fine — tested on 6.0.428)
   Verify with "dotnet --version"

3. VS Code (or Visual Studio 2022+ if preferred)

Create a file called .env in the frontend (frontend/.env)
Copy the example contents from .env.example but replace contents with:
REACT_APP_API_BASE_URLL=http://localhost:5000
Open terminal:
"cd PhiRedactor.Api"
dotnet run
Open another terminal:
"cd frontend"
"npm install"
"npm start"

Frontend: http://localhost:3000

Backend: http://localhost:5000/swagger

### Final Notes

I did spend a bit more time on this than expected mostly because I took my time learning my way through the React project. React is still fairly new to me though Material UI and Axios helped keep things quite simple.

I also triple-checked the project requirements and aimed to make the README as informative as possible.

I probably spent more time wrangling Codespaces than anything else! I've never used it before, and while it’s a bit quirky, I had one file in the wrong place and it took a bit to realize it. However, I really don’t consider that part of the actual project. It was just something I wanted to learn for my own development. Now I know how it works, and what not to do.
