#Coffee Machine API

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
