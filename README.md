# BackEndDevFinalProject: User Management API

## Project Overview
This project is a User Management API built with ASP.NET Core Minimal API for TechHive Solutions. It allows HR and IT departments to efficiently create, update, retrieve, and delete user records. The API was developed as a final project for a back-end development course, with a focus on leveraging Microsoft Copilot for code generation, enhancement, and debugging.

## Features
- CRUD operations for user management (GET, POST, PUT, DELETE)
- In-memory user store with seeded test users
- Input validation to ensure only valid user data is processed
- Custom middleware for:
  - Error handling (returns consistent JSON error responses)
  - Authentication (JWT Bearer token validation)
  - Logging (logs HTTP requests and responses)
- Swagger/OpenAPI documentation with JWT support
- Static file serving from the `wwwroot` directory

## Technology Stack
- .NET 9 (ASP.NET Core Minimal API)
- Swagger/OpenAPI for API documentation
- Microsoft Copilot for AI-assisted development

## Setup Instructions
1. **Prerequisites:**
   - [.NET 9 SDK](https://dotnet.microsoft.com/download) installed
2. **Clone the repository:**
   ```powershell
   git clone <your-repo-url>
   cd BackEndDevFinalProject
   ```
3. **Restore dependencies and build:**
   ```powershell
   dotnet restore
   dotnet build
   ```
4. **Run the API:**
   ```powershell
   dotnet run
   ```
5. **Access Swagger UI:**
   - Navigate to `https://localhost:5001/swagger` (or the URL shown in the console) to explore and test the API endpoints.

## API Endpoints
- `GET /users` - Retrieve all users
- `GET /users/{id}` - Retrieve a user by ID
- `POST /users` - Add a new user
- `PUT /users/{id}` - Update an existing user
- `DELETE /users/{id}` - Delete a user by ID

All endpoints require a valid JWT Bearer token for authentication.

## Middleware Pipeline
1. Error handling middleware
2. Authentication middleware
3. Logging middleware

## Testing

### Testing with Swagger UI

To send the Authorization token "your-secret-token" in Swagger UI, follow these steps:

1. Open Swagger UI in your browser (usually at /swagger or /swagger/index.html).
2. Click the "Authorize" button (often a lock icon at the top right).
3. In the value field, enter: your-secret-token
4. Click "Authorize" and then "Close".

Now, all your API requests from Swagger UI will include the Authorization header with your token.

### Testing with Postman

To send the Authorization token "your-secret-token" in Postman, follow these steps:

1. Open Postman and create a new request.
2. Set the method to GET (or whatever your endpoint expects).
3. Paste the URL.
4. Go to the Authorization tab.
   - a. Type: Bearer Token
   - b. Token: your-secret-token
5. Send the request and inspect the response.

### Testing with REST Client

If testing is done in Visual Studio Code with the REST Client extension installed, the API can be tested with the following test files located within the project:

- TestApi.http
- UserApi.http

The API includes validation and robust error handling for edge cases.

## Educational Context
This project was developed as part of a back-end development course, with activities focused on:
- Writing and enhancing API code with Copilot
- Debugging and validating code with Copilot
- Implementing and managing middleware with Copilot

Regarding how Copilot was used in this project's development process, transcriptions of the prompts that were used and the conversations that occurred were captured and saved in text files. These files can be found in the `/wwwroot/docs` folder in the project.

## License
This project is licensed under the GNU General Public License.

---
