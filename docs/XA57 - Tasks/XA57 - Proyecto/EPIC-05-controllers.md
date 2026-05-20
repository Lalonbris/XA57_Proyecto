# EPIC-05 — Controllers y ViewModels

> → Spec: `06-api-contracts.md`, `05-architecture.md §4.4`  
> → Log: Phase 2 (2026-03-16)  
> Estado: 🔄 En progreso  
> Prioridad: 🔴 Alta | Depende de: EPIC-04 ✅

---

## ViewModels

- [x] **EPIC-05-01** — `ViewModels/CarritoViewModel.cs`
  - `Items : List<Pedido>`
  - `Total : decimal` calculado como `Items.Sum(i => i.Cantidad * i.Producto.Precio)`

---

## HomeController

- [x] **EPIC-05-02** — `Controllers/HomeController.cs`
  - Inyecta `IProductoService`
  - `Index()`: retorna vista principal
  - `Catalogo()`: obtiene productos activos y retorna vista con `List<Producto>`
  - No requiere autenticación (RN-01 — catálogo abierto)

---

## ProductosController

- [x] **EPIC-05-03** — `Controllers/ProductosController.cs`
  - Inyecta `IProductoService`
  - `Detalle(int id)`:
    - Obtiene producto con `TipoProducto` incluido
    - Retorna `NotFound()` si el producto no existe
    - Inyecta `data-attributes` del `TipoProducto` para el componente React
  - No requiere autenticación (RN-01)

---

## CarritoController

- [x] **EPIC-05-04** — `Controllers/CarritoController.cs`
  - → Log: Phase 2 (2026-03-16)
  - Atributo `[Authorize]` en el controlador
  - `Index()`: retorna `CarritoViewModel` con ítems y total
  - `Agregar([FromBody] CarritoItemDto)`:
    - Captura `ValidationException` → `BadRequest` con mensaje
    - Retorna `200 OK` con `PedidoResultDto` en éxito
  - `Eliminar(int id)`: elimina ítem y redirige a `Index`
  - `Cantidad()`: retorna conteo de ítems como JSON para el badge

---

## ConfiguradorApiController

- [x] **EPIC-05-05** — `Controllers/ConfiguradorApiController.cs` 🔴
  - → Spec: `06-api-contracts.md §2`
  - Ruta base: `[Route("api/configurador")]`
  - Inyectar `IModeloAutobusService`, `ILineaService`, `IProductoService`
  - **No requiere autenticación** (catálogo público, RN-01)

  ### `GET /api/configurador/modelos`
  ```csharp
  [HttpGet("modelos")]
  public async Task<IActionResult> GetModelos()
  {
      var modelos = await _modeloService.ObtenerActivosAsync();
      return Ok(modelos.Select(m => new { m.Id, m.Nombre, m.Fabricante }));
  }
  ```

  ### `GET /api/configurador/lineas`
  ```csharp
  [HttpGet("lineas")]
  public async Task<IActionResult> GetLineas()
  {
      var lineas = await _lineaService.ObtenerActivasAsync();
      return Ok(lineas.Select(l => new {
          l.Id, l.Nombre, l.ColorPrimario, l.ColorSecundario, l.NombreOperador
      }));
  }
  ```

  ### `GET /api/configurador/producto/{id}`
  ```csharp
  [HttpGet("producto/{id}")]
  public async Task<IActionResult> GetProducto(int id)
  {
      var producto = await _productoService.ObtenerPorIdAsync(id);
      if (producto == null) return NotFound();
      return Ok(new {
          producto.Id, producto.Nombre, producto.Precio,
          TipoProducto = producto.TipoProducto == null ? null : new {
              producto.TipoProducto.Id,
              producto.TipoProducto.Nombre,
              producto.TipoProducto.PermiteNombre,
              producto.TipoProducto.PermiteNumeroEconomico,
              producto.TipoProducto.PermiteRuta,
              producto.TipoProducto.MaxCaracteres
          }
      });
  }
  ```

  **Criterios de aceptación:**
  - `GET /api/configurador/modelos` → JSON array con `id`, `nombre`, `fabricante`
  - `GET /api/configurador/lineas` → JSON array con `id`, `nombre`, `colorPrimario`, `colorSecundario`, `nombreOperador`
  - `GET /api/configurador/producto/{id}` → objeto con sección `tipoProducto` que incluye las flags booleanas
  - `GET /api/configurador/producto/999` → `404 Not Found`
  - Sin autenticación requerida en los tres endpoints
