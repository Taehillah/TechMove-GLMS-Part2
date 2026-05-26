# TechMove Global Logistics Management System (GLMS)

## Project Overview

TechMove GLMS is a Part 2 monolithic enterprise application prototype for managing logistics clients, contracts, signed agreement documents, and service requests. The prototype uses ASP.NET Core MVC with Entity Framework Core and SQL Server while keeping business rules in injectable services instead of placing them directly in controllers.

## Assignment Context

This project was built for the EAPD7111wPOE Part 2 assignment. It demonstrates a clean monolithic MVC architecture, database-backed CRUD operations, file upload validation, LINQ filtering, external API integration through HttpClient, Azure deployment readiness, and xUnit business-logic testing.

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- SQL Server / Azure SQL Database
- Entity Framework Core
- Bootstrap
- HttpClient
- xUnit
- Azure App Service compatible configuration

## Architecture Summary

The solution remains monolithic as required for Part 2:

- `TechMoveGLMS.Web` contains MVC controllers, Razor views, EF Core models, DbContext, migrations, and service classes.
- `TechMoveGLMS.Tests` contains xUnit tests for business logic.
- Controllers handle HTTP flow and delegate rules, file validation, file saving, and currency conversion to services.
- EF Core uses `ApplicationDbContext`, SQL Server, seeded sample data, and an initial migration.

## Features

- Client full CRUD with validation.
- Contract full CRUD linked to clients.
- Signed agreement PDF upload to `wwwroot/uploads/contracts`.
- PDF-only validation that rejects `.exe` and invalid content types.
- Contract details page with agreement download link.
- Service Request full CRUD linked to contracts.
- Service requests blocked for `Expired` and `OnHold` contracts.
- USD to ZAR conversion using a configured exchange rate API endpoint.
- Contract filtering by status, start date, and end date using `IQueryable` and LINQ `Where` clauses.
- Bootstrap responsive tables, forms, validation summaries, and navigation.

## Setup Instructions

1. Open the solution:

   ```bash
   cd TechMoveGLMS
   dotnet restore
   ```

2. Confirm the SQL Server connection string in `TechMoveGLMS.Web/appsettings.Development.json`.

3. Apply the database migration:

   ```bash
   dotnet ef database update --project TechMoveGLMS.Web
   ```

4. Run the application:

   ```bash
   dotnet run --project TechMoveGLMS.Web
   ```

## Migration Commands

Create a new migration after model changes:

```bash
dotnet ef migrations add MigrationName --project TechMoveGLMS.Web
```

Apply migrations:

```bash
dotnet ef database update --project TechMoveGLMS.Web
```

## Configuration

The default connection string is stored under:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TechMoveGLMS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

The currency API endpoint is stored under:

```json
"CurrencyApi": {
  "UsdToZarEndpoint": "https://open.er-api.com/v6/latest/USD"
}
```

For Azure App Service, override both values using App Service Application Settings instead of hardcoding production secrets or connection strings.

## Azure Deployment Instructions

1. Create an Azure SQL Database.
2. Copy the Azure SQL connection string.
3. In Azure App Service, add an application setting or connection string named `ConnectionStrings__DefaultConnection`.
4. Add the currency API endpoint as `CurrencyApi__UsdToZarEndpoint`.
5. Publish from Visual Studio:
   - Right-click `TechMoveGLMS.Web`.
   - Select `Publish`.
   - Choose `Azure App Service`.
   - Select the target subscription, resource group, and app service.
   - Publish the application.
6. Run EF Core migrations against Azure SQL using the Package Manager Console, command line, or Visual Studio publish migration option.

Uploaded files are saved under `wwwroot/uploads/contracts`. For a production Azure deployment, keep uploaded files in persistent App Service storage or replace this with Azure Blob Storage if the application scales across multiple instances.

## Test Execution

Run all tests:

```bash
dotnet test
```

The test project verifies:

- USD to ZAR calculation.
- PDF validation accepts valid PDF uploads.
- `.exe` uploads are rejected with a clear exception.
- Active contracts allow service requests.
- Expired and on-hold contracts block service requests.

## Demo Flow Suggestions for Lecturer Video

1. Open the dashboard and show the navigation.
2. Create a client.
3. Create a contract linked to that client.
4. Upload a valid PDF signed agreement.
5. Open contract details and download the PDF.
6. Use Contract Search to filter by status and date.
7. Create a service request for an active contract and show USD to ZAR conversion fields after saving.
8. Attempt to create a service request for an expired or on-hold contract and show the validation message.
9. Run `dotnet test` and show the passing business-rule tests.
