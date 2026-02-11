# Product.API

Modern .NET 8 Web API for managing products using **Clean Architecture /
Onion Architecture**.

------------------------------------------------------------------------

## Features

-   CRUD operations for Products
-   Repository Pattern
-   Entity Framework Core with SQL Server
-   Soft-delete support (optional -- via global query filter)
-   xUnit + Moq unit tests (controllers & repositories)
-   Layered structure: Core, Application, Infrastructure, API, Tests
-   Partial updates (PATCH-like behavior in PUT)
-   In-memory / SQLite in-memory testing for repositories
-   Project focused on demonstrating **how xUnit works in real-world
    .NET applications**

------------------------------------------------------------------------

## Project Structure

-   **Product.Core** → Domain entities & interfaces
-   **Product.Application** → Business logic & services
-   **Product.Infrastructure** → EF Core, repositories, database access
-   **Product.API** → Controllers & API configuration
-   **Product.Tests** → xUnit unit tests

------------------------------------------------------------------------

## Technologies

-   .NET 8
-   ASP.NET Core Web API
-   Entity Framework Core 8 (SQL Server)
-   xUnit 2.9
-   Moq 4.20
-   Microsoft.EntityFrameworkCore.InMemory
-   SQLite (for repository testing)

------------------------------------------------------------------------

## Getting Started

### Prerequisites

-   .NET 8 SDK
-   SQL Server (Express / LocalDB recommended)

------------------------------------------------------------------------

## Installation

``` bash
git clone <your-repo-url>
cd Product.API
```

------------------------------------------------------------------------

## Running Unit Tests

Unit tests are located in **Product.Tests**

``` bash
# Run all tests
dotnet test

# Run specific project
dotnet test Product.Tests/Product.Tests.csproj

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

------------------------------------------------------------------------

## API Endpoints

  Method   Endpoint             Description
  -------- -------------------- ----------------------------
  GET      /api/products        Get all products
  GET      /api/products/{id}   Get product by ID
  POST     /api/products        Create new product
  PUT      /api/products/{id}   Update product (partial)
  DELETE   /api/products/{id}   Delete product (soft/hard)

------------------------------------------------------------------------

# xUnit Setup (Required Packages)

This project is specially structured to demonstrate **xUnit testing in
.NET 8**.

Install required NuGet packages in **Product.Tests**:

``` bash
dotnet add Product.Tests package xunit
dotnet add Product.Tests package xunit.runner.visualstudio
dotnet add Product.Tests package Moq
dotnet add Product.Tests package Microsoft.NET.Test.Sdk
dotnet add Product.Tests package coverlet.collector
dotnet add Product.Tests package Microsoft.EntityFrameworkCore.InMemory
dotnet add Product.Tests package Microsoft.Data.Sqlite
```

------------------------------------------------------------------------

# How xUnit Works in This Project

### 1) Test Project Structure

    Product.Tests
     ├── Controllers
     │     └── ProductsControllerTests.cs
     ├── Repositories
     │     └── ProductRepositoryTests.cs
     └── Fixtures / TestHelpers

------------------------------------------------------------------------

### 2) xUnit Core Concepts Used

#### Fact

Used for single test cases.

``` csharp
[Fact]
public async Task GetProduct_ReturnsProduct_WhenIdExists()
```

#### Theory

Used for parameterized testing.

``` csharp
[Theory]
[InlineData(1)]
[InlineData(2)]
```

------------------------------------------------------------------------

### 3) Arrange -- Act -- Assert Pattern

All tests follow standard AAA pattern:

``` csharp
// Arrange
var repository = new ProductRepository(context);

// Act
var result = await repository.GetByIdAsync(id);

// Assert
Assert.NotNull(result);
```

------------------------------------------------------------------------

### 4) Mocking using Moq

Dependencies like repositories or services are mocked.

``` csharp
var mockRepo = new Mock<IProductRepository>();
mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
        .ReturnsAsync(product);
```

------------------------------------------------------------------------

### 5) Testing Controllers

-   Mock services
-   Validate response types
-   Validate HTTP status codes

``` csharp
Assert.IsType<OkObjectResult>(response);
```

------------------------------------------------------------------------

### 6) Testing Repositories

Uses:

-   EF Core InMemory
-   SQLite in-memory provider

``` csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb")
    .Options;
```

------------------------------------------------------------------------

### 7) Why xUnit?

-   Built-in support in .NET
-   Parallel test execution
-   Constructor-based setup
-   Cleaner syntax vs NUnit/MSTest
-   Industry standard for ASP.NET Core testing

------------------------------------------------------------------------

### 8) Test Execution Flow

1.  Test runner discovers tests
2.  xUnit creates test class instance
3.  Constructor runs (setup)
4.  Test executes
5.  Assert validates result
6.  Test runner reports outcome

------------------------------------------------------------------------

## Best Practices Followed

-   Isolation of tests
-   No external DB dependency
-   Mocking external services
-   Clean AAA structure
-   Deterministic test cases
-   Coverage support enabled

------------------------------------------------------------------------

## Future Enhancements

-   Integration tests using WebApplicationFactory
-   TestContainers for real DB testing
-   CI pipeline test automation
-   Mutation testing

------------------------------------------------------------------------

## Author

Senior .NET Developer focused on:

-   Clean Architecture
-   Unit Testing
-   Scalable APIs
-   Production-grade patterns
