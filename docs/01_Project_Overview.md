# MediForm Manager

**Enterprise Medical Form Lifecycle Management Platform**

Version: 1.0  
Status: MVP In Progress  
Last Updated: 2026-08-10

---

# 1. Project Overview

## Purpose

MediForm Manager is a portfolio project inspired by real-world experience maintaining an electronic medical consent system in a hospital environment.

Rather than reproducing an existing product, this project redesigns the platform using modern software architecture, clean domain modeling, and maintainable engineering practices.

The primary goal is to demonstrate software design, backend architecture, database modeling, and application development skills for international software engineering positions.

---

# 2. Objectives

## Primary Goal

Develop a Minimum Viable Product (MVP) capable of managing medical forms with version control and role-based access management.

## Secondary Goal

Evolve the MVP into a metadata-driven platform supporting:

- Dynamic Form Rendering
- Validation Engine
- Medical Order Mapping
- Deployment Workflow
- Audit Logging
- HIS / OCS / EMR Integration
- Cloud Deployment

---

# 3. Problem Statement

Many legacy hospital form management systems suffer from:

- Tight coupling
- Limited maintainability
- Manual validation
- Weak deployment process
- Lack of transaction safety
- Poor scalability

MediForm Manager addresses these limitations through a modular architecture and metadata-driven design.

---

# 4. MVP Scope

## Implemented Foundation

- User management API (create, read, update, activate/deactivate)
- Password hashing
- JWT user authentication
- JWT Bearer authorization for protected endpoints
- Swagger / OpenAPI API verification
- PostgreSQL database with EF Core migrations

## Remaining MVP Scope

- Form Management
- Form Version Management
- Role/permission-based authorization
- Basic Permission Management
- WPF management UI

# 5. Technology Stack

| Layer | Technology |
|--------|------------|
| Backend | ASP.NET Core Web API |
| Desktop Client | WPF (.NET) |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| Authentication | JWT |
| API Documentation | Swagger / OpenAPI |

---

# 6. Planned Architecture

```text
WPF Client
      │
ASP.NET Core Web API
      │
Application Layer
      │
Domain Layer
      │
Infrastructure Layer
      │
PostgreSQL
```

---

# 7. Future Roadmap

Phase 1
- MVP

Phase 2
- Validation Engine
- Medical Order Mapping

Phase 3
- Deployment Workflow
- Audit Logging

Phase 4
- React Frontend
- Docker
- AWS

---

# 8. Repository Structure

```text
mediform-manager/

docs/
src/
README.md
```

---

# 9. License

This project is intended for educational and portfolio purposes only.

No proprietary hospital source code, database schema, or confidential business logic is included.
