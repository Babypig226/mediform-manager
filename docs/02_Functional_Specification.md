# Functional Specification

**Project:** MediForm Manager  
**Version:** 1.0 (MVP In Progress)  
**Last Updated:** 2026-08-10

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
- Create medical forms
- Edit form metadata
- Search forms
- Categorize forms
- Activate / Archive forms

---

## 2.4 Form Version Management

### Features
- Create new version
- View version history
- Restore previous version
- Mark active version

Business Rule:
Only one active version is allowed per form.

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
- Database schema is created and updated using EF Core Migration.

## Remaining

- Users can manage forms.
- Form versions can be created and managed.
- Role/permission-based authorization is applied to relevant operations.
- WPF management UI is implemented.

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
