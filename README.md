# MediForm Manager

MediForm Manager is a portfolio project for managing the lifecycle of
medical forms, including users, forms, versions, organizational
relationships, and future deployment/mapping workflows.

The project is designed as a practical backend-focused system based on
experience maintaining medical information software.

## Current Status

### Implemented

-   Clean Architecture-based solution structure
-   ASP.NET Core Web API
-   .NET 10
-   PostgreSQL
-   Entity Framework Core
-   Domain entities and relationships
-   Fluent API entity configurations
-   EF Core migrations
-   Database schema creation/update
-   Swagger UI
-   User DTOs
-   User repository contract
-   Local development secrets separated from source control

### MVP In Progress

-   User repository implementation
-   User service and business rules
-   User management API
-   Administrator bootstrap
-   Password hashing
-   JWT authentication and authorization
-   Form management
-   Form version management
-   Swagger API verification

## Architecture

``` text
Client (WPF)
     |
     | HTTP / JSON
     v
API
     |
     v
Application
     |
     | Repository interfaces
     v
Infrastructure
     |
     | EF Core
     v
PostgreSQL

Domain
  ^
  |
Core business entities and rules
```

### Projects

-   **MediFormManager.Domain** --- Core domain entities and business
    concepts. Independent of EF Core, PostgreSQL, HTTP, and UI.
-   **MediFormManager.Application** --- Application use cases, DTOs,
    repository contracts, and application services.
-   **MediFormManager.Infrastructure** --- EF Core, `MediFormDbContext`,
    repository implementations, entity configurations, and migrations.
-   **MediFormManager.Api** --- ASP.NET Core HTTP entry point:
    controllers, dependency injection, authentication, Swagger, and
    configuration.
-   **MediFormManager.Wpf** --- Desktop client that communicates with
    the API over HTTP.

## Key Design Decisions

### Separate Domain from Infrastructure

Domain models remain independent of database technology. Persistence
changes should not require redesigning the core domain.

### DTOs instead of exposing entities directly

API contracts are separated from persistence/domain entities. Internal
fields such as password hashes are not exposed, and create/update/read
models can evolve independently.

### Repository abstraction

Repository contracts are defined in Application, while EF Core
implementations belong to Infrastructure. Application logic depends on
abstractions rather than database technology.

### User lifecycle

Users are not physically deleted when they leave the organization
because historical medical-form ownership and auditability must be
preserved.

`IsActive` represents whether an account can currently use the system.

`IsDeleted` represents logical deletion and is separate from account
availability.

### Form versioning

Forms and form versions are modeled separately so historical versions
can be retained while forms evolve.

## Technology Stack

-   C#
-   .NET 10
-   ASP.NET Core Web API
-   Entity Framework Core
-   PostgreSQL
-   WPF
-   Swagger / OpenAPI
-   Git

## Repository Structure

``` text
mediform-manager/
├── .gitignore
├── README.md
├── docs/
└── MediFormManager/
    ├── MediFormManager.slnx
    └── src/
        ├── MediFormManager.Api/
        ├── MediFormManager.Application/
        ├── MediFormManager.Domain/
        ├── MediFormManager.Infrastructure/
        └── MediFormManager.Wpf/
```

## Local Configuration

Secrets such as database passwords and future JWT signing keys are not
stored in the repository.

For local development, sensitive configuration should be stored using
.NET User Secrets.

Example configuration key:

``` text
ConnectionStrings:DefaultConnection
```

## Roadmap

-   Form components
-   Order-to-form mapping
-   Permission rules
-   Audit logging
-   Deployment scheduling
-   WPF management UI
-   Docker
-   Cloud deployment
-   CI/CD

## Project Goal

MediForm Manager is intended to demonstrate maintainable application
architecture, database modeling, versioned medical-form management,
authorization design, and separation of business logic from
infrastructure rather than only CRUD operations.
