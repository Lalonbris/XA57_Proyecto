# XA57 — Backlog de Tareas

> Generado a partir de los documentos SDD v1.0  
> Última actualización: 2026-03-27 | Metodología: Proceso Unificado (RUP)

---

## Estado del Proyecto

| Fase | Descripción | Estado |
|---|---|---|
| Phase 1 — Service Validation | Validaciones en servicios, excepciones custom, repositorios nuevos | ✅ Completada (2026-03-13) |
| Phase 2 — Cart Functionality | Personalización de color, badge en tiempo real, build React | ✅ Completada (2026-03-16) |
| Phase 3 — User Registration (CU-01) | ASP.NET Identity, registro/login, diseño xa57, localización | ✅ Completada (2026-03-20) |
| Phase 4 — Catálogo y Configurador | Vistas públicas, ConfiguradorApiController, React UI completo | 🔄 En progreso |
| Phase 5 — Panel de Administración | CRUD catálogo, gestión de pedidos, lista de picking | ⏳ Pendiente |
| Phase 6 — Calidad y Despliegue | NFR, rendimiento, logging, HTTPS | ⏳ Pendiente |

---

## Development Log

### 2026-03-13 — Phase 1: Service Validation ✅

- **Custom Exception:** Creada `Application/Exceptions/ValidationException.cs` para manejo consistente de errores.
- **PedidoService Validation:**
  - Inyectados `IProductoRepository` e `ITipoProductoRepository`.
  - Validaciones en `AgregarAsync`: `Cantidad > 0`, `ProductoId` existe y activo, textos no exceden `MaxCaracteres` de `TipoProducto`.
- **ProductoService:** Agregados `AgregarAsync` y `ActualizarAsync` con validaciones de `Nombre`, `Precio`, `TipoProductoId`.
- **Nuevos Repositorios:** `ITipoProductoRepository` y `TipoProductoRepository` para tabla `tipos_producto`.
- **DI:** `ITipoProductoRepository` registrado en `Program.cs`.
- **Fix:** Nullable warning corregido en `PedidoResultDto`.
- **Build:** Proyecto compila exitosamente.

### 2026-03-16 — Phase 2: Cart Functionality ✅

- **Personalización de color:** Propiedades `Color` y `ColorHex` agregadas a entidad `Pedido` y `CarritoItemDto`; migración aplicada a tabla `Pedidos`.
- **PedidoService:** Actualizado para mapear las nuevas propiedades del DTO a la entidad.
- **Badge en tiempo real:** Función global `actualizarIconoCarrito` implementada en `_Layout.cshtml`; llamada desde React (`App.js`) y desde `Catalogo.cshtml`.
- **Fix:** Inconsistencias en el ID del elemento badge del carrito corregidas.
- **Build React:** Flujo de compilación del frontend documentado en este archivo.

### 2026-03-20 — Phase 3: User Registration (CU-01) ✅

- **Scaffolding:** Páginas de ASP.NET Core Identity generadas: `Register`, `Login`, `Account Management`.
- **User Model:** `ApplicationUser` extendido con `Nombre` y `Apellido`.
- **UI/UX Redesign:** Páginas completamente rediseñadas y traducidas al sistema de diseño `xa57-*`.
- **Migración BD:** Columnas `Nombre` y `Apellido` agregadas a tabla `AspNetUsers`.
- **Integración:** Vista parcial `_LoginPartial` agregada al layout principal.
- **Localización:** Mensajes de validación de Identity traducidos al español.
- **Fix:** Correcciones de CSS en alineación y tipografía.

---

## Estructura del Backlog

```
TASK-000  Configuración inicial              ✅ Completada
EPIC-01   Base de datos                      ✅ Completada
EPIC-02   Domain Layer                       ✅ Completada
EPIC-03   Infrastructure Layer               ✅ Completada
EPIC-04   Application Layer                  ✅ Completada
EPIC-05   Controllers y ViewModels           🔄 En progreso
EPIC-06   Vistas Razor                       🔄 En progreso
EPIC-07   Módulo React — Configurador        🔄 En progreso
EPIC-08   Autenticación y Usuarios           ✅ Completada (Phase 3)
EPIC-09   Panel de Administración            ⏳ Pendiente
EPIC-10   Validaciones y Reglas de Negocio   🔄 Parcialmente completada
EPIC-11   No Funcionales                     ⏳ Pendiente
```

---

## Leyenda

| Símbolo | Significado |
|---|---|
| `[x]` | Tarea completada |
| `[ ]` | Tarea pendiente |
| `🔴` | Prioridad Alta |
| `🟡` | Prioridad Media |
| `🟢` | Prioridad Baja |
| `⚠️` | Decisión pendiente o riesgo |
| `→ Spec` | Referencia a documento SDD |
| `→ Log` | Referencia a entrada del Development Log |

---

## Índice de Archivos

| Archivo | Épica | Estado |
|---|---|---|
| `tasks/TASK-000-setup.md` | Configuración inicial del proyecto | ✅ |
| `tasks/EPIC-01-database.md` | Base de datos, migraciones, seed data | ✅ |
| `tasks/EPIC-02-domain.md` | Entidades e interfaces del Domain Layer | ✅ |
| `tasks/EPIC-03-infrastructure.md` | AppDbContext y repositorios | ✅ |
| `tasks/EPIC-04-application.md` | Servicios, DTOs y excepciones | ✅ |
| `tasks/EPIC-05-controllers.md` | Controllers y ViewModels | 🔄 |
| `tasks/EPIC-06-views.md` | Vistas Razor | 🔄 |
| `tasks/EPIC-07-react.md` | Módulo React Configurador | 🔄 |
| `tasks/EPIC-08-auth.md` | Autenticación con ASP.NET Identity | ✅ |
| `tasks/EPIC-09-admin.md` | Panel de Administración | ⏳ |
| `tasks/EPIC-10-business-rules.md` | Validaciones y Reglas de Negocio | 🔄 |
| `tasks/EPIC-11-nfr.md` | No Funcionales | ⏳ |
| `tasks/RESUMEN-ORDEN-EJECUCION.md` | Orden de ejecución y estimados | — |
