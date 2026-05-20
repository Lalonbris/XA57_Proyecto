# XA57 — Backlog de Tareas

> Generado a partir de los documentos SDD v1.0  
> Última actualización: 2026-05-20 | Metodología: Proceso Unificado (RUP)

---

## Estado del Proyecto

| Fase | Descripción | Estado |
|---|---|---|
| Phase 1 — Service Validation | Validaciones en servicios, excepciones custom, repositorios nuevos | ✅ Completada (2026-03-13) |
| Phase 2 — Cart Functionality | Personalización de color, badge en tiempo real, build React | ✅ Completada (2026-03-16) |
| Phase 3 — User Registration (CU-01) | ASP.NET Identity, registro/login, diseño xa57, localización | ✅ Completada (2026-03-20) |
| Phase 4 — Catálogo y Configurador | Vistas públicas, ConfiguradorApiController, React UI completo | ✅ Completada (2026-05-20) |
| Phase 5 — Panel de Administración | CRUD catálogo, gestión de pedidos, lista de picking | ✅ Completada (2026-05-20) |
| Phase 6 — Calidad y Despliegue | NFR, logging, HTTPS, optimización | 🔄 Mayormente completada |

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

### 2026-05-20 — Phase 4: Catálogo y Configurador ✅

- **ConfiguradorApiController:** Endpoints `GET /api/configurador/modelos`, `/lineas`, `/producto/{id}` implementados.
- **Vista Detalle.cshtml:** Contenedor React con data-attributes inyectados desde TipoProducto.
- **React Components:** `ModeloSelector`, `LineaSelector`, `PersonalizacionForm`, `VistaPrevia`, `CantidadControl`, `AgregarCarritoBtn` (integrado en App.js).
- **Hook useConfiguradorData:** Carga paralela de modelos, líneas y producto.
- **Validaciones React:** `utils/validaciones.js` con reglas RN-02, RN-03, RN-04.
- **Build React:** Integración con Razor via `wwwroot/react/` y `@section Scripts`.

### 2026-05-20 — Phase 5: Panel de Administración ✅

- **AdminController:** CRUD completo para Productos, TiposProducto, Líneas, Modelos, Pedidos, Usuarios.
- **Admin Views:** `_AdminLayout`, `Index`, `Productos`, `TiposProducto`, `Lineas`, `Modelos`, `Pedidos`, `DetallePedido`, `ListaPicking`, `Usuarios`.
- **Gestión de Pedidos:** Filtro por estado, cambio de estado con validación de transición RN-11, lista de picking con swatches de color.
- **Usuarios Admin:** Crear usuario con rol, editar, eliminar, agregar/quitar roles.
- **RN-11:** Transiciones de estado validadas tanto en `PedidoService` como en `AdminController`.

### 2026-05-20 — Phase 6: Calidad y Despliegue 🔄

- **Serilog:** Configurado con sinks Console + File (rotación diaria, 30 días de retención).
- **HTTPS:** `app.UseHttpsRedirection()` y `app.UseHsts()` configurados en `Program.cs`.
- **Logging:** `PedidoService` registra creación de pedidos y cambios de estado. Intentos de transición inválida quedan como Warning.
- **Lazy Loading:** `loading="lazy"` agregado en todas las imágenes del catálogo e Index.
- **0 Warnings:** Proyecto compila con 0 advertencias y 0 errores.

---

## Estructura del Backlog

```
TASK-000  Configuración inicial              ✅ Completada
EPIC-01   Base de datos                      ✅ Completada
EPIC-02   Domain Layer                       ✅ Completada
EPIC-03   Infrastructure Layer               ✅ Completada
EPIC-04   Application Layer                  ✅ Completada
EPIC-05   Controllers y ViewModels           ✅ Completada
EPIC-06   Vistas Razor                       ✅ Completada
EPIC-07   Módulo React — Configurador        ✅ Completada
EPIC-08   Autenticación y Usuarios           ✅ Completada
EPIC-09   Panel de Administración            ✅ Completada
EPIC-10   Validaciones y Reglas de Negocio   ✅ Completada
EPIC-11   No Funcionales                     🔄 Mayormente completada
```

---

## Tareas Pendientes (EPIC-11)

| ID | Tarea | Estado | Notas |
|---|---|---|---|
| EPIC-11-04 | Verificación de rendimiento de consultas (EXPLAIN ANALYZE) | ⏳ | Requiere BD con datos de prueba |
| EPIC-11-05 | Pruebas End-to-End (flujos 1–4) | ⏳ | Requiere entorno con BD poblada |

---

## Tareas Completadas (esta sesión)

| ID | Tarea | Cambios |
|---|---|---|
| EPIC-09-07 | `Views/Admin/DetallePedido.cshtml` | Vista completa con breadcrumb, badges, swatches de color, selector de cambio de estado con validación, botón Enviar a Manufactura |
| EPIC-09-02 | Acción `DetallePedido` en AdminController | Nueva acción GET que retorna vista con pedido completo (incluye Producto, Modelo, Línea) |
| EPIC-11-01 | Logging con Serilog | Paquete Serilog.AspNetCore, configuración Console + File, logs en `Logs/`, `UseSerilogRequestLogging()` |
| EPIC-11-01 | Logging en PedidoService | Inyectado `ILogger<PedidoService>`, registra creación de pedidos, cambios de estado, intentos inválidos |
| EPIC-11-03 | Lazy loading imágenes | `loading="lazy"` agregado en `Catalogo.cshtml` e `Index.cshtml` |
| Fix | 5 warnings de compilación | Nullable en UsuarioCreacionDto, null reference en AdminController, función no usada en Pedidos.cshtml, string? en DetallePedido.cshtml |
| Fix | Enlace a DetallePedido | ID del pedido en `Pedidos.cshtml` ahora es link a la vista de detalle |

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

---

## Índice de Archivos

| Archivo | Épica | Estado |
|---|---|---|
| `tasks/TASK-000-setup.md` | Configuración inicial del proyecto | ✅ |
| `tasks/EPIC-01-database.md` | Base de datos, migraciones, seed data | ✅ |
| `tasks/EPIC-02-domain.md` | Entidades e interfaces del Domain Layer | ✅ |
| `tasks/EPIC-03-infrastructure.md` | AppDbContext y repositorios | ✅ |
| `tasks/EPIC-04-application.md` | Servicios, DTOs y excepciones | ✅ |
| `tasks/EPIC-05-controllers.md` | Controllers y ViewModels | ✅ |
| `tasks/EPIC-06-views.md` | Vistas Razor | ✅ |
| `tasks/EPIC-07-react.md` | Módulo React Configurador | ✅ |
| `tasks/EPIC-08-auth.md` | Autenticación con ASP.NET Identity | ✅ |
| `tasks/EPIC-09-admin.md` | Panel de Administración | ✅ |
| `tasks/EPIC-10-business-rules.md` | Validaciones y Reglas de Negocio | ✅ |
| `tasks/EPIC-11-nfr.md` | No Funcionales | 🔄 |
| `tasks/RESUMEN-ORDEN-EJECUCION.md` | Orden de ejecución y estimados | — |
