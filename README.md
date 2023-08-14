
# BetwayAuthentication.Api - .NET 6 API Application

## Description

BetwayAuthentication.Api is a backend service for the Betway platform, developed using .NET 6. This API provides authentication-related endpoints and integrates with an in-memory database using Entity Framework.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET SDK 6](https://dotnet.microsoft.com/download/dotnet/6.0)

## Project Structure

The solution is structured into multiple projects:

- **BetwayAuthentication.Api**: Main API project.
- **BetwayAuthentication.DAL**: Data Access Layer, integrating with the Entity Framework and in-memory database.
- **BetwayAuthentication.Tests**: Unit and integration tests for the solution.
- **BetwayAuthentication.Api.sln**: The main solution file which includes the above projects.

## Installation & Setup

1. **Clone the Repository**:
   ```bash
   git clone <repository_url>
   ```

2. **Navigate to the Project Directory**:
   ```bash
   cd path_to/BetwayAuthentication.Api
   ```

3. **Restore the NuGet Packages**:
   ```bash
   dotnet restore
   ```

## Running the Application

1. Start the API server:
   ```bash
   dotnet run --project BetwayAuthentication.Api
   ```

The API should be up and running, ready to serve requests.

## Running Tests

To run the unit and integration tests:
```bash
dotnet test BetwayAuthentication.Tests
```
