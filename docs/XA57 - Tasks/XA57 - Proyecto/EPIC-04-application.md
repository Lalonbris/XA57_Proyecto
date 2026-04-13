# EPIC-04 — Application Layer (Servicios, DTOs y Excepciones)

> → Spec: `05-architecture.md §4.2`, `06-api-contracts.md`, `07-business-rules.md`  
> → Log: Phase 1 (2026-03-13), Phase 2 (2026-03-16)  
> Estado: ✅ Completada

---

## Excepciones

- [x] **EPIC-04-00** — `Application/Exceptions/ValidationException.cs`
  - → Log: Phase 1 (2026-03-13)
  - Excepción custom para manejo consistente de errores de validación en la capa de servicios
  - Usada por `PedidoService` y `ProductoService` en lugar de `ArgumentException` genérica
  - Capturada en los controllers para retornar respuestas `400 Bad Request` con mensaje descriptivo

---

## DTOs

- [x] **EPIC-04-01** — `Application/DTOs/CarritoItemDto.cs`
  - → Log: Phase 2 (2026-03-16)
  - Propiedades originales: `ProductoId`, `ModeloAutobusId`, `LineaId`, `NombreOperador`, `NumeroEconomico`, `Ruta`, `NotasEspeciales`, `Cantidad`
  - **Propiedades agregadas en Phase 2:** `Color` (string?), `ColorHex` (string?)

- [x] **EPIC-04-02** — `Application/DTOs/PedidoResultDto.cs`
  - Propiedades: `Mensaje` (string), `PedidoId` (int)
  - Fix: nullable warning corregido (Phase 1)

---

## Interfaces de Servicios

- [x] **EPIC-04-03** — `Application/Interfaces/IProductoService.cs`
  - `ObtenerTodosAsync() : Task<List<Producto>>`
  - `ObtenerPorIdAsync(int id) : Task<Producto?>`
  - **Agregados en Phase 1:** `AgregarAsync(Producto) : Task`, `ActualizarAsync(Producto) : Task`

- [x] **EPIC-04-04** — `Application/Interfaces/IPedidoService.cs`
  - `AgregarAsync(CarritoItemDto) : Task<PedidoResultDto>`
  - `ObtenerCarritoAsync() : Task<List<Pedido>>`
  - `EliminarItemAsync(int id) : Task`
  - `ContarItemsAsync() : Task<int>`

- [x] **EPIC-04-05** — `Application/Interfaces/ILineaService.cs`
  - `ObtenerActivasAsync() : Task<List<Linea>>`
  - `ObtenerPorIdAsync(int id) : Task<Linea?>`

- [x] **EPIC-04-06** — `Application/Interfaces/IModeloAutobusService.cs`
  - `ObtenerActivosAsync() : Task<List<ModeloAutobus>>`
  - `ObtenerPorIdAsync(int id) : Task<ModeloAutobus?>`

---

## Implementaciones de Servicios

- [x] **EPIC-04-07** — `Application/Services/ProductoService.cs`
  - → Log: Phase 1 (2026-03-13)
  - Implementa `IProductoService`
  - `ObtenerTodosAsync()` y `ObtenerPorIdAsync()`: delegan al repositorio
  - **Agregados en Phase 1:**
    - `AgregarAsync(Producto producto)`: valida `Nombre` no vacío, `Precio > 0`, `TipoProductoId` existe; lanza `ValidationException` si falla
    - `ActualizarAsync(Producto producto)`: mismas validaciones + verifica que el producto existe

- [x] **EPIC-04-08** — `Application/Services/PedidoService.cs`
  - → Log: Phase 1 (2026-03-13) y Phase 2 (2026-03-16)
  - Implementa `IPedidoService`
  - Inyecta `IPedidoRepository`, `IProductoRepository`, `ITipoProductoRepository`
  - `AgregarAsync(CarritoItemDto item)`:
    1. Valida `Cantidad > 0` → `ValidationException`
    2. Verifica que `ProductoId` exista y esté activo → `ValidationException`
    3. Obtiene `TipoProducto` del producto para leer `MaxCaracteres`
    4. Valida longitud de `NombreOperador`, `NumeroEconomico`, `Ruta` contra `MaxCaracteres`
    5. **Phase 2:** Mapea `Color` y `ColorHex` del DTO a la entidad `Pedido`
    6. Construye `Pedido` con `Estado = "Recibido"` (RN-10)
    7. Persiste y retorna `PedidoResultDto`
  - `ObtenerCarritoAsync()`, `EliminarItemAsync()`, `ContarItemsAsync()`: delegan al repositorio

- [x] **EPIC-04-09** — `Application/Services/LineaService.cs`
  - Implementa `ILineaService`; delega a `ILineaRepository`
  - Retorna líneas activas ordenadas por nombre

- [x] **EPIC-04-10** — `Application/Services/ModeloAutobusService.cs`
  - Implementa `IModeloAutobusService`; delega a `IModeloAutobusRepository`
  - Retorna modelos activos ordenados por nombre
