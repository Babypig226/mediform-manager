# Functional Specification

**Project:** MediForm Manager  
**Version:** 1.0 (MVP In Progress)  
**Last Updated:** 2026-08-11

---

# 1. Introduction

This document defines the functional requirements of the MediForm Manager MVP.

The MVP focuses on delivering a maintainable foundation while keeping the architecture extensible for future enterprise features.

---

# 2. Functional Modules

## 2.1 Authentication

### Objective
Provide secure user authentication.

### Features
- User login — implemented
- JWT token generation — implemented
- Password hashing — implemented
- Inactive-account login rejection — implemented
- JWT Bearer protection for API endpoints — implemented
- Logout — future
- Refresh token — future

---

## 2.2 User Management

### Features
- Create users — implemented
- List users / retrieve user by ID — implemented
- Update users — implemented
- Activate / Deactivate users — implemented
- Department assignment — implemented in the user model/API
- Job position assignment — implemented in the user model/API
- Role assignment — implemented in the user model/API

User records are retained rather than physically deleted so historical relationships can be preserved.

---

## 2.3 Form Management

### Features
- Create medical forms — implemented
- List forms / retrieve form by ID — implemented
- Edit form metadata — implemented
- Categorize forms — implemented
- Activate / deactivate forms — implemented
- Search forms — future

---

## 2.4 Form Version Management

### Features
- Create new version — implemented
- View version history — implemented
- Retrieve version by ID — implemented
- Update version status — implemented
- Restrict status values to `Draft`, `Active`, and `Archived` — implemented
- Restore previous version — future
- Mark active version — partially implemented through status update

Business Rule:
Only one active version is allowed per form. Enforcement of this rule is still pending.

---

## 2.5 Authorization

Role-based access control (RBAC).

Permissions include:
- View Forms
- Create Forms
- Edit Forms
- Deploy Forms
- Manage Users

---

## 2.6 WPF Desktop Client

### Features
- Login through the Web API — implemented
- Receive and retain JWT access token for the client session — implemented
- Send Bearer authentication on protected API requests — implemented
- Retrieve users from the protected User API — implemented
- Display users in a WPF DataGrid — implemented
- Form management UI — remaining MVP work

# 3. Non-functional Requirements

- Clean Architecture
- RESTful API
- PostgreSQL
- Entity Framework Core
- Swagger/OpenAPI
- Dependency Injection
- Repository Pattern (initial implementation)

---

# 4. Future Features

- Dynamic Form Builder
- Validation Rule Engine
- Medical Order Mapping
- Form Component Designer
- Deployment Scheduler
- Approval Workflow
- Audit Logging
- Dashboard
- React Frontend
- Docker
- AWS Deployment

---

# 5. MVP Acceptance Criteria

## Verified

- User authentication works.
- JWT authentication and Bearer authorization are configured.
- Invalid passwords and inactive accounts are rejected.
- Protected endpoints reject unauthenticated requests.
- Swagger endpoints are available and can send Bearer tokens.
- User create/read/update flows are verified against PostgreSQL.
- Form create/read/update flows are verified against PostgreSQL.
- Form versions can be created, queried, and updated.
- New form versions default to `Draft`.
- Invalid form-version status values are rejected by application logic.
- WPF login works against the authentication API.
- The WPF client reuses the JWT for protected requests.
- The WPF client retrieves and displays the user list.
- Database schema is created and updated using EF Core Migration.

## Remaining

- Only one active version is enforced per form.
- Application validation errors are mapped to consistent HTTP error responses.
- Role/permission-based authorization is applied to relevant operations.
- Form components are managed.
- WPF form management UI is implemented.

# 6. Out of Scope

The following features are intentionally excluded from the MVP:

- HIS / EMR integration
- Dynamic HTML rendering
- Electronic signature
- Batch processing
- Notification service
- Cloud deployment

These features are reserved for future development phases.

---

# 7. Design Philosophy

The MVP prioritizes software architecture over feature quantity.

Every implemented feature should be maintainable, testable, and extensible.

---

# 8. License

This document describes an original software architecture inspired by practical experience.

No proprietary workflow, database schema, or confidential implementation from any employer is reproduced.
