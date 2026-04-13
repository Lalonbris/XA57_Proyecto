# EPIC-02 — Domain Layer

> → Spec: `01-domain-model.md`, `04-data-model.md`  
> → Log: Phase 1 (2026-03-13), Phase 2 (2026-03-16)  
> Estado: ✅ Completada  
> **Regla:** Las entidades de Domain no referencian EF Core, ASP.NET ni ningún framework externo.

---

- [x] **EPIC-02-01** — Entidad `Usuario` / `ApplicationUser`
  - → Log: Phase 3 (2026-03-20)
  - `ApplicationUser` extiende `IdentityUser` con `Nombre` y `Apellido`
  - Migración aplicada a `AspNetUsers`

- [x] **EPIC-02-02** — Entidad `Categoria`
  - Propiedades: `Id`, `Nombre`, `Descripcion`
  - Navegación: `ICollection<Producto>`

- [x] **EPIC-02-03** — Entidad `TipoProducto`
  - Propiedades: `Id`, `Nombre`, `MaxCaracteres`, `PermiteNombre`, `PermiteNumeroEconomico`, `PermiteRuta`
  - Usado por `PedidoService` para validar límites de texto (Phase 1)

- [x] **EPIC-02-04** — Entidad `Producto`
  - Propiedades completas incluyendo `TipoProductoId` (FK), `Activo`
  - Validaciones de `Nombre`, `Precio`, `TipoProductoId` implementadas en `ProductoService` (Phase 1)

- [x] **EPIC-02-05** — Entidad `PersonalizacionProducto`
  - Propiedades: `Id`, `ProductoId`, `TipoOpcion`, `ValorOpcion`, `PrecioExtra`

- [x] **EPIC-02-06** — Entidad `ModeloAutobus`
  - Propiedades: `Id`, `Nombre`, `Fabricante`, `Activo`

- [x] **EPIC-02-07** — Entidad `Linea`
  - Propiedades: `Id`, `Nombre`, `ColorPrimario`, `ColorSecundario`, `NombreOperador`, `LogoUrl`, `Activa`

- [x] **EPIC-02-08** — Entidad `Pedido`
  - → Log: Phase 2 (2026-03-16)
  - Propiedades originales: `Id`, `ProductoId`, `ModeloAutobusId`, `LineaId`, `NombreOperador`, `NumeroEconomico`, `Ruta`, `NotasEspeciales`, `Cantidad`, `Estado`, `FechaCreacion`
  - **Propiedades agregadas en Phase 2:** `Color` (string?), `ColorHex` (string?)
  - Navegaciones: `Producto`, `ModeloAutobus?`, `Linea?`

- [x] **EPIC-02-09** — Entidades `Carrito` e `ItemCarrito`
  - `Carrito`: `Id`, `UsuarioId`, `FechaCreacion`
  - `ItemCarrito`: `Id`, `CarritoId`, `ProductoId`, `Cantidad`, `PrecioUnitario`

- [x] **EPIC-02-10** — Entidades `Orden` e `ItemOrden`
  - `Orden`: `Id`, `UsuarioId`, `FechaOrden`, `Estado`, `Total`, `DireccionEnvio`
  - `ItemOrden`: `Id`, `OrdenId`, `ProductoId`, `PersonalizacionId?`, `Cantidad`, `PrecioUnitario`

- [x] **EPIC-02-11** — Interfaces de Repositorios
  - `IProductoRepository`: `ObtenerTodosAsync`, `ObtenerPorIdAsync`
  - `IPedidoRepository`: `AgregarAsync`, `ObtenerConProductosAsync`, `EliminarAsync`, `ContarAsync`
  - `ILineaRepository`: `ObtenerActivasAsync`, `ObtenerPorIdAsync`
  - `IModeloAutobusRepository`: `ObtenerActivosAsync`, `ObtenerPorIdAsync`
  - **`ITipoProductoRepository`** (agregado Phase 1): `ObtenerPorIdAsync`

---
---

# EPIC-03 — Infrastructure Layer

> → Spec: `04-data-model.md §4`, `05-architecture.md §4.3`  
> → Log: Phase 1 (2026-03-13), Phase 2 (2026-03-16), Phase 3 (2026-03-20)  
> Estado: ✅ Completada

---

- [x] **EPIC-03-01** — `AppDbContext`
  - Hereda de `IdentityDbContext<ApplicationUser>` (actualizado en Phase 3 para Identity)
  - Todos los `DbSet<T>` declarados
  - Configuraciones `OnModelCreating`: UNIQUE en `Email`, índices, valores default

- [x] **EPIC-03-02** — `ProductoRepository`
  - Implementa `IProductoRepository`
  - `ObtenerTodosAsync()`: incluye `TipoProducto`, filtra `Activo = true`
  - `ObtenerPorIdAsync(id)`: incluye `TipoProducto`, retorna `null` si no existe

- [x] **EPIC-03-03** — `PedidoRepository`
  - Implementa `IPedidoRepository`
  - `AgregarAsync`: persiste con `SaveChangesAsync`, retorna Id generado
  - `ObtenerConProductosAsync`: incluye `Producto`, `ModeloAutobus`, `Linea`
  - `EliminarAsync`: safe (no lanza si ID no existe)
  - `ContarAsync`: `COUNT(*)` sobre `Pedidos`

- [x] **EPIC-03-04** — `LineaRepository`
  - Implementa `ILineaRepository`
  - `ObtenerActivasAsync()`: filtra `Activa = true`, ordenado por `Nombre`

- [x] **EPIC-03-05** — `ModeloAutobusRepository`
  - Implementa `IModeloAutobusRepository`
  - `ObtenerActivosAsync()`: filtra `Activo = true`, ordenado por `Nombre`

- [x] **EPIC-03-06** — `TipoProductoRepository`
  - → Log: Phase 1 (2026-03-13)
  - Implementa `ITipoProductoRepository`
  - `ObtenerPorIdAsync(id)`: acceso a tabla `tipos_producto`
  - Registrado en `Program.cs` vía DI
