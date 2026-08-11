# MediForm Manager

MediForm Manager is a portfolio project for managing the lifecycle of
medical forms, including users, forms, versions, organizational
relationships, and future deployment/mapping workflows.

The project is designed as a practical backend-focused system based on
experience maintaining medical information software.

## Current Status

### Implemented

- Clean Architecture-based solution structure
- ASP.NET Core Web API with .NET 10
- PostgreSQL and Entity Framework Core
- Domain entities, relationships, Fluent API configurations, and EF Core migrations
- User DTOs, repository contract/implementation, and application service
- User management API: create, list, get by ID, update, activate/deactivate
- Password hashing using ASP.NET Core `PasswordHasher`
- JWT authentication and JWT Bearer authorization
- Swagger / OpenAPI Bearer authentication support
- End-to-end API verification against PostgreSQL
- Form category seed data for standard medical form categories
- Form management API: create, list, get by ID, update
- Form version management API: create, list by form, get by ID, update status
- Form version status validation (`Draft`, `Active`, `Archived`)
- WPF login client integrated with the authentication API
- JWT reuse for authenticated WPF API requests
- WPF user list loaded from the protected User API and displayed in a DataGrid
- Local development secrets separated from source control

### MVP Next Steps

- Enforce the single-active-version rule for form versions
- Global API exception handling and consistent HTTP error responses
- Form component management
- Role/permission-based authorization
- Audit logging
- Expand the WPF management UI to form management

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

### Authentication and authorization

User passwords are never stored as plaintext. Passwords are hashed using
ASP.NET Core `PasswordHasher` before persistence.

Authentication uses signed JWT access tokens. Protected API endpoints use
ASP.NET Core Bearer authentication and `[Authorize]`.

Inactive users cannot authenticate even when valid credentials are provided.

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

New form versions are created in `Draft` status. Version status is currently
restricted to `Draft`, `Active`, or `Archived`. Enforcement of a single active
version per form is a planned MVP refinement.

### WPF client integration

The WPF client authenticates through the Web API, stores the returned JWT
access token in the shared API client, and reuses it for protected requests.
The initial desktop flow supports login and authenticated user-list retrieval.

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

Sensitive values such as database passwords and JWT signing keys are not stored in the repository.
For local development, sensitive configuration is stored using .NET User Secrets.

Example configuration keys:

```text
ConnectionStrings:DefaultConnection
Jwt:SecretKey
```

Non-sensitive JWT configuration such as issuer and audience may remain in `appsettings.json`.

## Roadmap

-   Form components
-   Order-to-form mapping
-   Permission rules
-   Audit logging
-   Deployment scheduling
-   Expanded WPF form management UI
-   Docker
-   Cloud deployment
-   CI/CD

## Project Goal

MediForm Manager is intended to demonstrate maintainable application
architecture, database modeling, versioned medical-form management,
authorization design, and separation of business logic from
infrastructure rather than only CRUD operations.
