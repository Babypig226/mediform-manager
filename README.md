# MediForm Manager

**Medical Form Lifecycle Management | ASP.NET Core · React · WPF · PostgreSQL**

MediForm Manager is a portfolio project inspired by experience developing and maintaining hospital information software. It explores how medical consent forms can be managed as **versioned, metadata-driven definitions**, rather than as tightly coupled screens and documents.

The emphasis is on backend architecture, data modeling, form-version lifecycle, rule evaluation, and practical integration between a desktop management client and a web-based form designer.

> **Project status (October 2026):** V1 demonstration build. This is a portfolio prototype, **not a production-ready medical system**. Clinical values used in the demo come from a mock provider; no live hospital system is connected.

## Features

### Implemented and exercised

- **Users and authentication:** user CRUD/activation, hashed passwords using ASP.NET Core `PasswordHasher`, JWT login, Bearer-protected API endpoints, and WPF login/logout flow.
- **Form lifecycle:** medical form and version APIs, version history, and `Draft`, `Active`, `Archived` status model.
- **Form designer:** React-based editor for form fields, labels, prompts, choices, ordering, and draft schema persistence.
- **Versioned editing:** draft-only edits, server-side validation, and optimistic concurrency using an expected update timestamp.
- **Dynamic runtime:** schema-driven form rendering, field validation, and conditional rules for visibility/required state and related interactions.
- **Clinical data mapping (mock):** data-dictionary entries can be assigned to fields; a mock clinical provider supplies sample patient/encounter values for runtime prefilling.
- **Desktop integration:** WPF Form Library and version selection launch React designer/runtime views through WebView2, with authentication handoff for the local demonstration environment.
- **Containerized backend:** multi-stage Docker image for ASP.NET Core API and Docker Compose PostgreSQL 17 with a persistent named volume and health check.

### Verified integration flows

- WPF login → Form Library → select version → React designer/runtime → WPF logout and re-login.
- Save and retrieve draft form schema, including field-to-dictionary mapping.
- Docker Compose API + PostgreSQL: applied **six EF Core migrations**, restored sample development data, successfully logged in to obtain a JWT, and used it to retrieve a protected form through `GET /api/Forms`.

**Scope note:** Docker testing covers the **API and PostgreSQL**. The WPF desktop client and Vite-based React UI have been run separately in the local development environment; they are **not packaged as a single containerized desktop distribution**.

## Architecture

```text
WPF Desktop Client ──HTTP/JWT──> ASP.NET Core API
       │                              │
       └── WebView2 ──> React UI      ▼
                                Application
                               services + DTOs
                               repository/provider
                                  interfaces
                                     ▲
                                     │ implemented by
                               Infrastructure
                             EF Core / repositories
                               / mock provider
                                     │
                                     ▼
                                PostgreSQL

Domain: entities and business concepts shared by core layers
```

The HTTP call path and **compile-time dependency direction** are different: Application declares the abstractions it needs, and Infrastructure implements them. The API composes the dependencies. The goal is to keep application use cases independent of a specific persistence implementation.

### Solution projects

| Project | Responsibility |
| --- | --- |
| `MediFormManager.Domain` | Domain entities and concepts |
| `MediFormManager.Application` | Use cases, DTOs, repository and provider interfaces |
| `MediFormManager.Infrastructure` | EF Core DbContext, PostgreSQL configurations/migrations, implementations |
| `MediFormManager.Api` | REST endpoints, authentication, dependency injection, configuration |
| `MediFormManager.Wpf` | Windows desktop management client and WebView2 host |
| `MediFormManager.Web` | React / TypeScript form designer and renderer |

## Technology

C# / .NET 10 · ASP.NET Core Web API · EF Core · PostgreSQL 17 · JWT Bearer · WPF · WebView2 · React · TypeScript · Vite · Docker / Docker Compose

## Run the API and database with Docker

**Requirements:** Docker Desktop with Linux containers and Docker Compose.

These commands assume the working directory is `MediFormManager/` (the folder containing `Dockerfile`, `compose.yaml`, and `src/`).

1. Create a local `.env` file alongside `compose.yaml`:

   ```dotenv
   POSTGRES_USER=mediform
   POSTGRES_PASSWORD=<generate-a-strong-local-password>
   POSTGRES_DB=mediform_manager
   JWT_SECRET_KEY=<generate-a-long-random-secret>
   ```

   This is an **example only**. Replace the placeholders locally. Never commit `.env`, database backups, or credentials. The Compose configuration maps PostgreSQL to `127.0.0.1:55432` and the API to `127.0.0.1:8080`.

2. Start services:

   ```powershell
   docker compose config --quiet
   docker compose up -d --build
   docker compose ps
   ```

3. For a **fresh database**, apply EF Core migrations from the host using the Docker database connection (`127.0.0.1:55432`). The existing development workflow uses `dotnet-ef` and the Infrastructure project as the migration target. **Do not run a database update with the local User Secrets connection string accidentally pointing at another database.**

4. To log in and query protected endpoints, provide an account through the application's existing user-creation/seed workflow or restore an **authorized, synthetic demo-only** PostgreSQL backup. Docker Compose does **not** create a default administrator account.

5. Check the API endpoint using `http://127.0.0.1:8080/api/Forms`. Without a JWT, `401 Unauthorized` is expected. After logging in at `POST /api/Auth/login` using JSON fields `loginId` and `password`, supply `Authorization: Bearer <accessToken>` to access protected routes.

To stop services without removing the database volume:

```powershell
docker compose down
```

**Do not use `docker compose down -v` unless you intentionally want to delete the container database volume.**

### Local development configuration

For the API launched directly through Visual Studio or `dotnet run`, sensitive values are kept in **.NET User Secrets** under `MediFormManager.Api`, using these keys:

```text
ConnectionStrings:DefaultConnection
Jwt:SecretKey
```

`Jwt:Issuer` and `Jwt:Audience` are non-secret configuration settings. In Docker Compose the database connection and signing key are provided via environment variables instead; User Secrets are **not copied into the image**.

Current WebView2 integration uses a local Vite development server, and the desktop client's configured API endpoint is the local HTTPS development API. The Docker API on port `8080` is an independently tested backend, not a drop-in replacement for the complete desktop development configuration.

## Design decisions

- **Versioned schemas:** forms and versions are separate records so earlier definitions can be retained.
- **Interface-based data access:** Application contracts are implemented by Infrastructure, supporting isolated service testing and technology-specific integrations.
- **Metadata-driven rendering:** form behavior is described through persisted components, options, mappings, and rules rather than per-form UI code.
- **Soft deletion:** retention-oriented domain modeling differentiates logical deletion from account activation.
- **Secrets outside source control:** User Secrets for local .NET development and ignored `.env` settings for Docker.

## Known limitations / next steps

- Enforce and fully regression-test the **single-active-version-per-form** invariant.
- Expand role- and form-level authorization; JWT authentication alone is not full RBAC.
- Replace mock clinical values with a secure, configurable hospital-system integration layer (not yet connected to a real HIS/OCS/EMR).
- Add audit trail, deployment workflow, medical-order-to-form mapping, and robust production error handling.
- Harden logout/session management, WebView2 token exchange, TLS, secret storage, and deployment configuration before considering real users or data.
- Package the web UI for a production-style run instead of relying on the Vite development server.

## Documentation

The repository also contains project overview, functional specification, and ERD documents. Some are **earlier V1 planning snapshots (August 2026)**; they describe intended scope and may not reflect the more recent React designer, rule engine, and Docker demonstration work. EF Core migrations and the current implementation are authoritative for the physical schema.

## Intended use

Educational and portfolio demonstration only. The project is an original redesign informed by professional domain experience; it does not include proprietary employer source code or real patient data. Do not connect it to production hospital systems or use it to process clinical data without a separate security, privacy, and regulatory review.
