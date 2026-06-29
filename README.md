# todoList

> Proyecto backend desarrollado en **C# / .NET 10** con el objetivo de aprender y aplicar **Clean Architecture**, **principios SOLID**, **Inyección de Dependencias** y el **Patrón Repositorio** mediante un dominio sencillo de gestión de tareas.

---

## Descripción

Este proyecto tiene un dominio funcional simple de manera intencional.

El objetivo no es desarrollar otra aplicación de tareas, sino construir una base arquitectónica sólida que sirva como preparación para proyectos empresariales y sistemas SaaS de mayor complejidad.

Toda la lógica de negocio permanece desacoplada de frameworks, bases de datos y servicios externos siguiendo los principios de **Clean Architecture**.

---

## Objetivos

* Aprender Clean Architecture desde cero.
* Aplicar principios SOLID.
* Diseñar entidades con lógica de negocio.
* Implementar Casos de Uso.
* Comprender la Inyección de Dependencias.
* Implementar el Patrón Repositorio.
* Separar la lógica de negocio de la infraestructura.
* Construir bases sólidas para futuros proyectos backend.

---

## Arquitectura

```text
Presentation (ASP.NET Core API)
        │
        ▼
Application (Casos de Uso)
        │
        ▼
Domain (Entidades y Reglas de Negocio)
        ▲
        │
Infrastructure (Persistencia y Servicios Externos)
```

### Domain

* Entidad rica (`Tarea`)
* Reglas de negocio
* Contrato del repositorio (`ITareasRepository`)

### Application

* Casos de uso
* DTOs
* Inversión de dependencias
* Orquestación de la aplicación

### Infrastructure

* Persistencia mediante archivos JSON
* Implementación del repositorio
* Servicios externos
* Configuración mediante `IOptions`

### Presentation

* API REST con ASP.NET Core
* Controladores
* Inyección de Dependencias
* Swagger / OpenAPI

---

## Tecnologías

* .NET 10
* C#
* ASP.NET Core
* Clean Architecture
* Dependency Injection
* Repository Pattern
* System.Text.Json

### Próximamente

* PostgreSQL
* Entity Framework Core
* Redis
* xUnit
* Docker
* JWT
* CI/CD

---

## Estado del proyecto

### Completado

* Capa Domain
* Capa Application
* Capa Infrastructure (persistencia JSON)

### En desarrollo

* Capa Presentation
* Controladores
* Configuración de Inyección de Dependencias

### Planeado

* Pruebas unitarias
* Pruebas de integración
* PostgreSQL
* Entity Framework Core
* Redis
* Docker Compose
* JWT
* CI/CD

---

## Filosofía del proyecto

Este repositorio documenta mi proceso de aprendizaje como desarrollador Backend .NET.

La prioridad del proyecto no es desarrollar funcionalidades rápidamente, sino comprender en profundidad la arquitectura de software, el diseño orientado a objetos y las buenas prácticas utilizadas en aplicaciones empresariales.

---

## Estructura del proyecto

```text
todoList
│
├── Domain
│
├── Application
│   ├── DTO
│   ├── Interfaces
│   └── UseCases
│
├── Infrastructure
│
└── Presentation
```

---

## Licencia

Este proyecto se distribuye bajo la licencia MIT.
# todoList

> Backend project developed in **C# / .NET 10** to learn and apply **Clean Architecture**, **SOLID principles**, **Dependency Injection**, and **Repository Pattern** through a simple task management domain.

---

## Overview

This project is intentionally simple from a business perspective.

Its purpose is **not** to build the next Todo application, but to establish a solid architectural foundation before developing larger enterprise systems.

The project follows **Clean Architecture**, where business rules remain independent of frameworks, databases, and external services.

---

## Objectives

* Learn Clean Architecture from scratch.
* Apply SOLID principles.
* Design rich domain entities.
* Implement Use Cases (Application Layer).
* Understand Dependency Injection.
* Implement the Repository Pattern.
* Separate business logic from infrastructure.
* Practice software architecture before building larger SaaS projects.

---

## Current Architecture

```text
Presentation (ASP.NET Core API)
        │
        ▼
Application (Use Cases)
        │
        ▼
Domain (Entities & Business Rules)
        ▲
        │
Infrastructure (Persistence & External Services)
```

### Domain

* Rich entity (`Tarea`)
* Business rules
* Repository abstraction (`ITareasRepository`)

### Application

* Use Cases
* DTOs
* Dependency inversion
* Application orchestration

### Infrastructure

* JSON file persistence
* Repository implementation
* External services
* Configuration through `IOptions`

### Presentation

* ASP.NET Core Web API
* Controllers
* Dependency Injection
* Swagger/OpenAPI

---

## Technologies

* .NET 10
* C#
* ASP.NET Core
* Clean Architecture
* Dependency Injection
* Repository Pattern
* System.Text.Json
* Docker (future)
* PostgreSQL (planned)
* Redis (planned)
* xUnit (planned)

---

## Current Status

### Completed

* Domain Layer
* Application Layer
* Infrastructure Layer (JSON persistence)

### In Progress

* Presentation Layer
* Controllers
* Dependency Injection configuration

### Planned

* Unit Tests
* Integration Tests
* PostgreSQL
* Entity Framework Core
* Redis Cache
* JWT Authentication
* Docker Compose
* CI/CD

---

## Learning Goals

This repository documents my learning journey toward becoming a Backend .NET Developer.

Rather than focusing on delivering features quickly, the emphasis is on understanding architectural decisions, software design principles, and maintainable code.

---

## Project Structure

```text
todoList
│
├── Domain
│
├── Application
│   ├── DTO
│   ├── Interfaces
│   └── UseCases
│
├── Infrastructure
│
└── Presentation
```

---

## License

This project is released under the MIT License.
