# Coffee Machine API

This project is designed to emulate a coffee machine.

## Features
- Brew coffee (`/brew-coffee` endpoint)
   - Every fifth call will rreturn 503
   - If the date is April 1st, it will return 418
- Custom date/time provider
- Dependency injection for testing
- CustomStatusCodeResult to allow for empty responses

#Prerequisites
.NET 9.0 SDK

## How to Run
1. Clone the repository.
2. Build using `dotnet build`.
3. Run using `dotnet run`.

## Configuration

The project includes a demo API key in the `appsettings.json` file to ensure it runs out of the box without setup.

**Note**: This is not secure and is done for demonstration purposes. In a production setting, API keys would be stored securely in environment variables or secret management tools.

## TODO/Potential improvements
- Update storage of api key by potentially using either;
  - utilise a cloud based secret management solution (Azure Key Vault/App configuration etc.)
  - Environment variables
- Persistence of request count - DB storage (redis/mssql etc.)
- Caching - response caching of weather temperature
- Add structured logging to solution
