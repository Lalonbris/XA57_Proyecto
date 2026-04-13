# TASK-000 — Configuración Inicial del Proyecto

> → Spec: `00-project-overview.md` §3, §4  
> → Log: Implícito en todas las fases  
> Estado: ✅ Completada

---

- [x] **TASK-000-01** — Crear solución Visual Studio y estructura de carpetas
  - Solución `XA57.sln` creada con proyecto `ASP.NET Core MVC .NET 8`
  - Estructura de capas: `Domain/`, `Application/`, `Infrastructure/`, `Controllers/`, `ViewModels/`, `Views/`, `configurador-client/`, `Migrations/`

- [x] **TASK-000-02** — Instalar dependencias NuGet
  - `Microsoft.EntityFrameworkCore` 8.x
  - `Npgsql.EntityFrameworkCore.PostgreSQL` 8.x
  - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (agregado en Phase 3)
  - `Microsoft.EntityFrameworkCore.Tools` y `Design`

- [x] **TASK-000-03** — Configurar conexión a PostgreSQL
  - Connection string en `appsettings.json`
  - `AppDbContext` registrado en `Program.cs`

- [x] **TASK-000-04** — Inicializar repositorio Git + GitHub
  - `.gitignore` configurado para .NET + Node.js
  - Repositorio remoto en GitHub activo

- [x] **TASK-000-05** — Inicializar proyecto React (`configurador-client/`)
  - Proyecto React creado en `configurador-client/`
  - Build documentado en Development Log (2026-03-16)

- [x] **TASK-000-06** — Configurar inyección de dependencias
  - Repositorios y servicios registrados en `Program.cs`
  - `ITipoProductoRepository` registrado (agregado en Phase 1 — 2026-03-13)
  - Autenticación por Identity configurada (Phase 3 — 2026-03-20)

---
---

# EPIC-01 — Base de Datos

> → Spec: `04-data-model.md`  
> → Log: Phase 1 (2026-03-13), Phase 2 (2026-03-16), Phase 3 (2026-03-20)  
> Estado: ✅ Completada

---

- [x] **EPIC-01-01** — Crear base de datos PostgreSQL
  - Base de datos `xa57_db` creada con usuario dedicado

- [x] **EPIC-01-02** — Migración inicial (EF Core)
  - Todas las tablas del esquema creadas vía `dotnet ef database update`
  - Tablas: `Usuarios`, `Categorias`, `TiposProducto`, `Productos`, `PersonalizacionesProducto`, `ModelosAutobus`, `Lineas`, `Pedidos`, `Carritos`, `ItemsCarrito`, `Ordenes`, `ItemsOrden`

- [x] **EPIC-01-03** — Migración Phase 2: columnas `Color` y `ColorHex`
  - → Log: Phase 2 (2026-03-16)
  - Columnas `Color` (string) y `ColorHex` (string) agregadas a tabla `Pedidos`
  - Migración creada y aplicada exitosamente

- [x] **EPIC-01-04** — Migración Phase 3: Identity (`AspNetUsers`)
  - → Log: Phase 3 (2026-03-20)
  - Columnas `Nombre` y `Apellido` agregadas a tabla `AspNetUsers`
  - Identity tables creadas: `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, etc.

- [x] **EPIC-01-05** — Índices de rendimiento
  - → Spec: `03-non-functional-requirements.md P-04`
  - Índices en columnas frecuentemente filtradas: `Activo`, `Estado`, `Email`

- [x] **EPIC-01-06** — Datos semilla (Seed Data)
  - → Spec: `04-data-model.md §2`
  - `TiposProducto`: Busito de Peluche, Llavero, Almohada, Cinta
  - `ModelosAutobus`: Irizar i8, Volvo 9800, Scania Touring, Mercedes-Benz Travego
  - `Lineas`: ETN, Omnibus de México, Primera Plus, Estrella Roja, Futura
  - `Categorias` y `Productos` de muestra
  - Usuario Administrador seed
