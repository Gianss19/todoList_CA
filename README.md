# TodoList API

API REST desarrollada con **ASP.NET Core 10** siguiendo los principios de **Clean Architecture** y **SOLID**. El proyecto implementa autenticación mediante **JWT**, persistencia con **Entity Framework Core Code First** y **PostgreSQL**, así como un diseño desacoplado mediante repositorios, casos de uso e inyección de dependencias.

> Proyecto desarrollado con fines de aprendizaje para profundizar en arquitectura de software y desarrollo backend con .NET.

---

# Tecnologías

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- PostgreSQL
- JWT (JSON Web Token)
- BCrypt
- Dependency Injection
- Clean Architecture
- Docker
- Swagger / OpenAPI

---

# Arquitectura

El proyecto está dividido en cuatro capas siguiendo Clean Architecture.

```
TodoList
│
├── todoList.Domain
│   ├── Entities
│   ├── Repositories
│   └── Enums
│
├── todoList.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Services
│   └── UseCases
│
├── todoList.Infrastructure
│   ├── Persistence
│   ├── Configurations
│   ├── Repositories
│   └── Services
│
└── todoList.Presentation
    ├── Controllers
    ├── Program.cs
    └── appsettings.json
```

---

# Principios aplicados

## SOLID

- Single Responsibility Principle
- Open/Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

## Patrones utilizados

- Repository Pattern
- Dependency Injection
- Use Case Pattern
- DTO Pattern

---

# Funcionalidades

## Usuarios

- Registro de usuarios
- Inicio de sesión
- Hash de contraseñas mediante BCrypt
- Activación y desactivación de usuarios
- Cambio de nombre
- Cambio de contraseña

## Tareas

- Crear tareas
- Obtener todas las tareas
- Obtener una tarea por Id
- Cambiar nombre
- Completar tarea
- Eliminar tarea

## Seguridad

- JWT Authentication
- Claims
- Roles
- Policies
- Endpoints protegidos mediante `[Authorize]`

---

# Modelo de datos

## Usuario

| Campo | Tipo |
|--------|------|
| Id | Guid |
| Nombre | string |
| Correo | string |
| PasswordHash | string |
| Rol | Enum |
| Activo | bool |
| FechaCreacion | DateTime |
| FechaActualizacion | DateTime? |

---

## Tarea

| Campo | Tipo |
|--------|------|
| Id | Guid |
| Nombre | string |
| IsCompleted | bool |
| UsuarioId | Guid |
| FechaCreacion | DateTime |
| FechaActualizacion | DateTime? |

Relación:

```
Usuario
   │
   │ 1
   │
   └───────────────*
                  Tarea
```

---

# Autenticación

La autenticación se realiza mediante JWT.

Proceso:

```
Usuario

↓

Login

↓

Validación de contraseña (BCrypt)

↓

Generación del JWT

↓

Cliente almacena el token

↓

Authorization: Bearer <token>

↓

Endpoints protegidos
```

---

# Entity Framework Core

El proyecto utiliza **Code First**.

Características implementadas:

- DbContext
- Fluent API
- IEntityTypeConfiguration
- Relaciones 1:N
- Migraciones
- PostgreSQL

---

# Variables de configuración

El proyecto utiliza **User Secrets** para evitar exponer información sensible en GitHub.

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },

  "Jwt": {
    "SecretKey": "",
    "Issuer": "",
    "Audience": ""
  }
}
```

---

# Ejecutar el proyecto

## 1 Clonar

```bash
git clone https://github.com/Gianss19/todoList_CA.git
```

---

## 2 Restaurar paquetes

```bash
dotnet restore
```

---

## 3 Configurar User Secrets

```bash
dotnet user-secrets init
```

Agregar:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=TodoList;Username=postgres;Password=tu_password"
  },

  "Jwt": {
    "SecretKey": "tu_secret_key",
    "Issuer": "TodoList.Api",
    "Audience": "TodoList.Client"
  }
}
```

---

## 4 Crear la base de datos

```bash
dotnet ef database update
```

---

## 5 Ejecutar

```bash
dotnet run --project todoList.Presentation
```

---

# Aprendizajes

Durante el desarrollo del proyecto se practicaron conceptos como:

- Clean Architecture
- SOLID
- Entity Framework Core Code First
- Fluent API
- Relaciones entre entidades
- PostgreSQL
- JWT
- Claims
- Roles
- Policies
- BCrypt
- Dependency Injection
- Repository Pattern
- Casos de uso
- DTOs
- User Secrets
- Migraciones

---

# Próximas mejoras

- Refresh Tokens
- Paginación
- Filtros y búsqueda
- Logging estructurado
- Unit Testing
- Integration Testing
- Docker Compose
- CI/CD con GitHub Actions
- Rate Limiting
- Validación con FluentValidation
- Result Pattern
- CQRS + MediatR

---

# Licencia

Proyecto desarrollado únicamente con fines educativos y de práctica.