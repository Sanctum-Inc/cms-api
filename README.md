# CMS API - Case Management System

A comprehensive case management system API built with .NET, following Clean Architecture principles with CQRS pattern using MediatR.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)

## ✨ Features

- **Court Case Management** - Track and manage court cases with detailed information
- **Lawyer Management** - Manage lawyer profiles and specialties
- **Document Management** - Store and retrieve case-related documents
- **User Authentication** - JWT-based authentication and authorization
- **Invoice Management** - Handle billing and invoice items
- **Court Date Scheduling** - Track important court dates and appointments
- **RESTful API** - Clean, well-documented API endpoints
- **Soft Delete** - Data is never permanently deleted, only marked as deleted

## 🛠 Tech Stack

- **.NET 8** - Latest .NET framework
- **PostgreSQL** - Primary database
- **Entity Framework Core** - ORM for database operations
- **MediatR** - CQRS and mediator pattern implementation
- **Mapster** - Object mapping
- **FluentValidation** - Request validation
- **JWT Authentication** - Secure token-based authentication
- **Scalar** - Modern API documentation (OpenAPI/Swagger)
- **xUnit** - Unit and integration testing
- **FluentAssertions** - Readable test assertions

## 🏗 Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
cms-api/
├── src/
│   ├── Api/              # API layer (Controllers, Configuration)
│   ├── Application/      # Business logic (Commands, Queries, Handlers)
│   ├── Domain/           # Domain entities and business rules
│   ├── Contracts/        # DTOs and API contracts
│   └── Infrastructure/   # Data access, external services
└── tests/
    ├── Api.Tests/                # Unit tests
    └── Api.Integration.Tests/    # Integration tests
```

**Key Patterns:**
- **CQRS** - Separation of read and write operations
- **Mediator Pattern** - Decoupled request/response handling
- **Repository Pattern** - Data access abstraction
- **Unit of Work** - Transaction management

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [PostgreSQL](https://www.postgresql.org/download/) (v12 or later)
- [Docker](https://www.docker.com/get-started) (optional, for containerized PostgreSQL)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd cms-api
```

### 2. Set Up PostgreSQL

**Option A: Using Docker**
```bash
docker run --name cms-postgres \
  -e POSTGRES_PASSWORD=your_password \
  -e POSTGRES_DB=cms-db \
  -p 5434:5432 \
  -d postgres:latest
```

**Option B: Local Installation**
- Install PostgreSQL and create a database named `cms-db`
- Note your connection credentials

### 3. Configure Application Settings

Update `src/Api/appsettings.json` with your settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=cms-db;Port=5434;User Id=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "your-super-secret-key-min-32-characters-long",
    "Issuer": "cms-api",
    "Audience": "yourapp_users",
    "ExpireMinutes": 60
  },
  "DocumentStorage": {
    "RootPath": "C:\\CMS\\Uploads"
  }
}
```

**Important:** 
- Never commit real credentials to version control
- Use User Secrets for development: `dotnet user-secrets init`
- Use environment variables for production

### 4. Create Document Storage Directory

```bash
# Windows
mkdir C:\CMS\Uploads

# Linux/Mac
mkdir -p /var/cms/uploads
```

Update the `DocumentStorage:RootPath` in appsettings accordingly.

## 💾 Database Setup

### Run Migrations

```bash
# Navigate to solution root
cd cms-api

# Apply migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

### Create New Migration (if needed)

```bash
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/Api -o Persistence/Migrations
```

### Database Schema

The application automatically creates the following tables:
- `Users` - System users
- `Lawyers` - Lawyer profiles and information
- `CourtCases` - Court case details
- `CourtCaseDates` - Important court dates
- `Documents` - Case-related documents
- `InvoiceItems` - Billing and invoice information

All entities support soft delete with audit tracking (CreatedBy, CreatedAt, UpdatedBy, UpdatedAt, DeletedBy, DeletedAt).

## ▶️ Running the Application

### Development Mode

```bash
# From solution root
dotnet run --project src/Api

# Or with hot reload
dotnet watch --project src/Api
```

The API will be available at:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`

### Production Mode

```bash
dotnet build --configuration Release
dotnet run --project src/Api --configuration Release
```

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Projects

```bash
# Unit tests only
dotnet test tests/Api.Tests

# Integration tests only
dotnet test tests/Api.Integration.Tests
```

### Run with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Test Structure

- **Unit Tests** (`Api.Tests`) - Test individual components in isolation
- **Integration Tests** (`Api.Integration.Tests`) - Test complete request/response flows with database

## 📚 API Documentation

Once the application is running, access the interactive API documentation:

**Scalar UI:** `https://localhost:5001/scalar/v1`

The API documentation includes:
- All available endpoints
- Request/response schemas
- Authentication requirements
- Example requests

### Main API Endpoints

#### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and receive JWT token

#### Lawyers
- `GET /api/lawyer` - Get all lawyers
- `GET /api/lawyer/{id}` - Get lawyer by ID
- `POST /api/lawyer` - Create new lawyer
- `PUT /api/lawyer/{id}` - Update lawyer
- `DELETE /api/lawyer/{id}` - Delete lawyer (soft delete)

#### Court Cases
- `GET /api/courtcase` - Get all court cases
- `GET /api/courtcase/{id}` - Get court case by ID
- `POST /api/courtcase` - Create new court case
- `PUT /api/courtcase/{id}` - Update court case
- `DELETE /api/courtcase/{id}` - Delete court case (soft delete)

#### Documents
- `GET /api/document` - Get all documents
- `GET /api/document/{id}` - Download document
- `POST /api/document` - Upload document
- `DELETE /api/document/{id}` - Delete document

## 📁 Project Structure

```
cms-api/
├── src/
│   ├── Api/
│   │   ├── Controllers/          # API endpoints
│   │   ├── Configuration/        # Startup configuration
│   │   └── Program.cs           # Application entry point
│   │
│   ├── Application/
│   │   ├── Common/              # Shared application logic
│   │   ├── [Feature]/
│   │   │   ├── Commands/        # Write operations (Create, Update, Delete)
│   │   │   ├── Queries/         # Read operations (Get, List)
│   │   │   └── Validators/      # Input validation
│   │   └── DependencyInjection.cs
│   │
│   ├── Domain/
│   │   ├── Common/              # Base entities, interfaces
│   │   ├── Users/               # User entity
│   │   ├── Lawyers/             # Lawyer entity
│   │   ├── CourtCases/          # Court case entity
│   │   ├── Documents/           # Document entity
│   │   └── [Other Entities]/
│   │
│   ├── Contracts/
│   │   ├── [Feature]/
│   │   │   ├── Requests/        # API request DTOs
│   │   │   └── Responses/       # API response DTOs
│   │   └── Common/
│   │
│   └── Infrastructure/
│       ├── Persistence/
│       │   ├── ApplicationDBContext.cs
│       │   ├── Migrations/
│       │   └── Repositories/
│       ├── Authentication/
│       └── DependencyInjection.cs
│
└── tests/
    ├── Api.Tests/               # Unit tests
    └── Api.Integration.Tests/   # Integration tests
```

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication:

1. Register or login to receive a JWT token
2. Include the token in subsequent requests:
   ```
   Authorization: Bearer <your-jwt-token>
   ```

Token configuration in `appsettings.json`:
- **ExpireMinutes:** Token validity duration (default: 60 minutes)
- **Key:** Secret key for signing tokens (must be at least 32 characters)

## 🤝 Contributing

1. Create a feature branch: `git checkout -b feature/your-feature-name`
2. Make your changes following existing code patterns
3. Write/update tests for your changes
4. Ensure all tests pass: `dotnet test`
5. Commit with clear messages: `git commit -m "Add feature: description"`
6. Push and create a Pull Request

### Code Standards

- Follow Clean Architecture principles
- Use CQRS pattern for all operations
- Add validators for all commands
- Write integration tests for new endpoints
- Document public APIs with XML comments
- Use meaningful variable and method names

## 📝 License

[Add your license information here]

## 👥 Authors

[Add author/team information here]

## 🐛 Troubleshooting

### Database Connection Issues

**Error:** "Cannot connect to PostgreSQL"
- Verify PostgreSQL is running
- Check connection string in appsettings.json
- Ensure port 5434 is not blocked by firewall

### Migration Issues

**Error:** "A connection was successfully established with the server, but then an error occurred"
- Ensure database exists
- Run: `dotnet ef database update --project src/Infrastructure --startup-project src/Api`

### JWT Token Issues

**Error:** "Unauthorized 401"
- Ensure you're sending the token in the Authorization header
- Check token hasn't expired (default: 60 minutes)
- Verify JWT Key is correctly configured

### Document Upload Issues

**Error:** "Path not found"
- Ensure DocumentStorage:RootPath directory exists
- Check application has write permissions to the directory

## 📞 Support

For issues and questions:
- Create an issue in the repository
- [Add contact information or support channels]

---

**Built with ❤️ using Clean Architecture and .NET**
