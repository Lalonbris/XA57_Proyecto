# EPIC-09 — Panel de Administración

> → Spec: `02-functional-requirements.md` CU-10 a CU-15  
> → Spec: `07-business-rules.md` RN-08, RN-11, RN-12  
> Estado: ⏳ Pendiente  
> Prioridad: 🟡 Media | Depende de: EPIC-08 ✅

---

## Repositorios y Servicios de Soporte

- [ ] **EPIC-09-01** — Extender repositorios para operaciones de administración 🟡
  - Agregar a `IProductoRepository`:
    ```csharp
    Task AgregarAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task<List<Producto>> ObtenerTodosIncluidosInactivosAsync();
    ```
  - Agregar a `IPedidoRepository`:
    ```csharp
    Task<List<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdConDetallesAsync(int id);
    Task ActualizarEstadoAsync(int id, string nuevoEstado);
    ```
  - Implementar en los repositorios correspondientes

---

## AdminController — Gestión del Catálogo

- [ ] **EPIC-09-02** — `Controllers/AdminController.cs` — Catálogo 🟡
  - → Spec: CU-10, CU-11, CU-12
  - Atributo `[Authorize(Roles = "Administrador")]` en el controlador completo (RN-08)
  - `Catalogo()`: lista todos los productos (activos e inactivos)
  - `CrearProducto()` GET: muestra formulario con listas de `Categorias` y `TiposProducto`
  - `CrearProducto(Producto)` POST: llama a `ProductoService.AgregarAsync()`; valida con `ValidationException`
  - `EditarProducto(int id)` GET: carga producto existente en formulario
  - `EditarProducto(Producto)` POST: llama a `ProductoService.ActualizarAsync()`
  - `ToggleActivo(int id)` POST: invierte `Activo`; cambio inmediato en catálogo público (CU-10)
  - `Lineas()`: CRUD de líneas y cromáticas (CU-12, RN-06 — nueva línea disponible para todos los productos)
  - `Modelos()`: CRUD de modelos de autobús
  - `Categorias()`: CRUD de categorías (CU-11)

  **Criterios de aceptación:**
  - Solo accesible con `Rol = "Administrador"` (RN-08); sin rol → `403` o redirect
  - Crear producto con `Nombre` vacío → error de validación visible, sin persistir
  - Agregar nueva línea → disponible inmediatamente en `/api/configurador/lineas` (RN-06)
  - Desactivar un producto → desaparece del catálogo público en la siguiente carga

---

## AdminController — Gestión de Pedidos y Manufactura

- [ ] **EPIC-09-03** — `Controllers/AdminController.cs` — Pedidos 🟡
  - → Spec: CU-13, CU-14, CU-15
  - `Pedidos()`: lista todos los pedidos con filtro por estado (GET con query param `?estado=Recibido`)
  - `DetallePedido(int id)`: muestra todos los atributos de personalización del pedido
  - `EnviarManufactura(int id)` POST:
    - Valida transición `Recibido → En producción` (RN-11)
    - Cambia estado y persiste
    - Redirige a `DetallePedido`
  - `ActualizarEstado(int id, string nuevoEstado)` POST:
    - Llama a `ValidarTransicionEstado(estadoActual, nuevoEstado)` (RN-11)
    - Si transición inválida → retorna `BadRequest` con mensaje descriptivo
    - Si válida → persiste y redirige
  - `ListaPicking(int id)`: vista de impresión del pedido para manufactura (RN-12)

  **Validador de estados (RN-11):**
  ```csharp
  private static readonly Dictionary<string, string> _transicionesValidas = new() {
      ["Recibido"]      = "En producción",
      ["En producción"] = "Enviado",
      ["Enviado"]       = "Entregado"
  };
  public bool EsTransicionValida(string actual, string nuevo)
      => _transicionesValidas.TryGetValue(actual, out var siguiente) && siguiente == nuevo;
  ```

  **Criterios de aceptación:**
  - Transición inválida (ej. `Recibido → Entregado`) → error visible, sin cambio en BD
  - No se puede retroceder un estado (RN-11)

---

## Vistas del Panel de Administración

- [ ] **EPIC-09-04** — `Views/Admin/_AdminLayout.cshtml` 🟡
  - Layout específico para el panel con menú lateral de administración
  - Aplica sistema de diseño `xa57-*`

- [ ] **EPIC-09-05** — `Views/Admin/Catalogo.cshtml` 🟡
  - Tabla de productos con columnas: Nombre, Categoría, Precio, Tipo, Activo
  - Botones de acción: Editar, Activar/Desactivar
  - Botón "Agregar producto"

- [ ] **EPIC-09-06** — `Views/Admin/Pedidos.cshtml` 🟡
  - Tabla de pedidos con filtros por estado
  - Columnas: ID, Producto, Cliente, Estado, Fecha
  - Enlace a "Ver detalle" por pedido

- [ ] **EPIC-09-07** — `Views/Admin/DetallePedido.cshtml` 🟡
  - Muestra todos los atributos de personalización: Producto, Modelo, Línea, Color, ColorHex, Operador, No. Económico, Ruta, Notas
  - Selector de cambio de estado con validación de transición
  - Botón "Enviar a manufactura" (si estado es `Recibido`)
  - Botón "Ver Lista de Picking"

- [ ] **EPIC-09-08** — `Views/Admin/ListaPicking.cshtml` 🔴
  - → Spec: RN-12
  - Vista de impresión (CSS `@media print`, sin navbar ni footer)
  - Muestra todos los campos requeridos por manufactura:

  | Campo | Fuente |
  |---|---|
  | Producto (nombre y tipo) | `Producto.Nombre`, `TipoProducto.Nombre` |
  | Modelo de autobús | `ModeloAutobus.Nombre`, `ModeloAutobus.Fabricante` |
  | Línea / Cromática | `Linea.Nombre`, `Linea.ColorPrimario`, `Linea.ColorSecundario` |
  | Color seleccionado | `Pedido.Color`, `Pedido.ColorHex` |
  | Nombre del operador | `Pedido.NombreOperador` |
  | Número económico | `Pedido.NumeroEconomico` |
  | Ruta / destino | `Pedido.Ruta` |
  | Notas especiales | `Pedido.NotasEspeciales` |
  | Cantidad | `Pedido.Cantidad` |
  | Fecha del pedido | `Pedido.FechaCreacion` |

  **Criterios de aceptación:**
  - Todos los campos se muestran sin truncamiento (RN-05)
  - La vista se imprime correctamente sin elementos de navegación
  - Los colores hex se muestran como swatches visuales y como valor textual
