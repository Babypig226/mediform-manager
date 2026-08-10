# Functional Specification

**Project:** MediForm Manager  
**Version:** 1.0 (Draft)  
**Last Updated:** 2026-07-28

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
- User login
- JWT token generation
- Password hashing
- Logout (future)
- Refresh token (future)

---

## 2.2 User Management

### Features
- Create users
- Update users
- Activate / Deactivate users
- Department assignment
- Job position assignment
- Role assignment

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

- User authentication works.
- JWT authentication is configured.
- Users can manage forms.
- Form versions can be created.
- Swagger endpoints are available.
- Database schema is created using EF Core Migration.

---

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
