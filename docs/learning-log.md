# Learning Log — MiniFinancialSupport

A day-by-day log of what was built and the concepts learned, kept in English
for interview practice (with short Spanish notes).

---

## Day 1 — Project foundation & first working endpoint

### What we built
- Solution with **layered (Clean) Architecture**: `Domain`, `Application`, `Infrastructure`, `Api`.
- `Customer` entity + DTOs (`CreateCustomerRequest`, `CustomerResponse`).
- **EF Core** wired to **SQL Server** (`ApplicationDbContext`, connection string, DI).
- First **migration** (`InitialCreate`) → real `Customers` table in SQL Server.
- **Controllers + Swagger**, dependency-injected `ICustomerService` / `CustomerService`.
- Working endpoints: `POST /api/customers` (201 Created) and `GET /api/customers` (200 OK).

### Concepts learned (interview-ready)

| Concept | English | Español |
|---|---|---|
| Clean Architecture | Dependencies point inward; `Domain` depends on nothing, so business rules are isolated and testable. | Las dependencias apuntan adentro; `Domain` no depende de nada. |
| Entity vs DTO | Entity = how data is stored (maps to a table). DTO = how data travels in/out of the API. Never expose the entity. | Entity = cómo se guarda. DTO = cómo se comunica. |
| Data types matter | `IsActive` is a yes/no → `bool`, never `string`. Pick the type that matches the meaning. | Elige el tipo según el significado del dato. |
| EF Core (ORM) | Maps C# classes to SQL so I don't write SQL by hand. `DbContext` is the bridge; `DbSet<T>` is a table. | ORM: mapea clases C# a SQL. |
| Migration | A versioned file that builds the DB schema from my entities. `migrations add` → recipe; `database update` → runs it. | Control de versiones del esquema de la BD. |
| Dependency Injection | Depend on an interface (`ICustomerService`), not the concrete class → swappable & testable. `AddScoped` = one instance per request. | Depender de la interfaz, no de la clase concreta. |
| REST status codes | `201 Created` for POST, `200 OK` for GET. | Códigos de estado correctos. |
| AsNoTracking | For read-only queries; EF doesn't track changes → faster. | Solo lectura, más rápido. |

### Debugging win (great interview story)
Hit a `ReflectionTypeLoadException` at startup. Root cause: a **transitive package
version conflict** — `Microsoft.AspNetCore.OpenApi` pulled `Microsoft.OpenApi` 2.x while
Swashbuckle expected 1.x. Fix: removed the unused `Microsoft.AspNetCore.OpenApi` package.

### Workflow lessons
- "Done" means **verified**, not assumed — always re-check the file and the build (key QA mindset).
- When a file is open in Visual Studio, saving there can overwrite external edits → make edits in one place.
- Small, atomic git commits with clear English messages.

---

## Day 2 — Plan (next session)
- **Auth**: JWT login + roles (Admin, QA, Support, User).
- **FluentValidation** on customer input (required fields, valid email, no duplicates).
- **Global error-handling middleware** + standard `ApiResponse<T>`.
- Finish **Customers**: `GET /{id}`, `PUT`, `PATCH /{id}/inactive` (409 on duplicates, 404 when missing).
- If time: start **Accounts** (deposit / withdraw).
