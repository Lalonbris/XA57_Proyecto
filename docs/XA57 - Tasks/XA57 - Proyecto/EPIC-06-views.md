# EPIC-06 — Vistas Razor (Frontend Server-Side)

> → Spec: `02-functional-requirements.md` CU-02, CU-03, CU-07  
> → Log: Phase 2 (2026-03-16), Phase 3 (2026-03-20)  
> Estado: 🔄 En progreso  
> Stack: Razor Views (.cshtml) + Bootstrap 5.3 + sistema de diseño `xa57-*`

---

## Layout y Autenticación

- [x] **EPIC-06-01** — `Views/Shared/_Layout.cshtml`
  - → Log: Phase 2 (2026-03-16), Phase 3 (2026-03-20)
  - Navbar con logo "XA57", enlace a catálogo, badge del carrito
  - **Phase 2:** Función global `actualizarIconoCarrito` implementada en JavaScript del layout
  - **Phase 3:** Vista parcial `_LoginPartial` integrada para mostrar estado de sesión (login/logout)
  - Fix Phase 2: ID del elemento badge del carrito corregido para consistencia con React y Razor

- [x] **EPIC-06-02** — Vistas de autenticación (Identity)
  - → Log: Phase 3 (2026-03-20)
  - `Areas/Identity/Pages/Account/Register.cshtml`: rediseñada con sistema `xa57-*`, campos `Nombre` y `Apellido` incluidos, traducida al español
  - `Areas/Identity/Pages/Account/Login.cshtml`: rediseñada con sistema `xa57-*`, traducida al español
  - `Areas/Identity/Pages/Account/Manage/`: todas las páginas de gestión de cuenta rediseñadas
  - CSS: correcciones de alineación y tipografía aplicadas

---

## Vistas del Catálogo Público

- [x] **EPIC-06-03** — `Views/Home/Index.cshtml`
  - Landing page con hero banner y botón "Ver catálogo"
  - Sección de categorías de productos disponibles

- [x] **EPIC-06-04** — `Views/Home/Catalogo.cshtml`
  - → Log: Phase 2 (2026-03-16)
  - Modelo: `@model List<Producto>`
  - Grid de tarjetas con imagen, nombre, precio y botón "Ver detalles"
  - **Phase 2:** Llama a `actualizarIconoCarrito` después de agregar al carrito
  - Imágenes con `loading="lazy"` para rendimiento (P-03)
  - Mensaje de lista vacía si no hay productos

- [x] **EPIC-06-05** — `Views/Productos/Detalle.cshtml` 🔴
  - → Spec: CU-03, CU-04, CU-05
  - Modelo: `@model Producto`
  - Imagen del producto (grande, con zoom)
  - Nombre, descripción y precio del producto
  - **Contenedor para React** con `data-attributes` del `TipoProducto`:
    ```html
    <div id="configurador-root"
         data-producto-id="@Model.Id"
         data-permite-nombre="@(Model.TipoProducto?.PermiteNombre.ToString().ToLower() ?? "false")"
         data-permite-numero-economico="@(Model.TipoProducto?.PermiteNumeroEconomico.ToString().ToLower() ?? "false")"
         data-permite-ruta="@(Model.TipoProducto?.PermiteRuta.ToString().ToLower() ?? "false")"
         data-max-caracteres="@(Model.TipoProducto?.MaxCaracteres ?? 30)">
    </div>
    ```
  - Sección `@section Scripts` que carga el bundle JS del configurador React
  - Si `EsPersonalizable = false`: botón directo "Agregar al carrito" sin configurador

  **Criterios de aceptación:**
  - Los `data-attributes` se inyectan con valores correctos del `TipoProducto`
  - React se monta sin errores de consola
  - Si `TipoProducto` es null, los data-attributes tienen valores por defecto seguros

---

## Vista del Carrito

- [x] **EPIC-06-06** — `Views/Carrito/Index.cshtml`
  - → Log: Phase 2 (2026-03-16)
  - Modelo: `@model CarritoViewModel`
  - Tabla de ítems con columnas: Producto, Modelo, Línea, Operador, No. Económico, Ruta, **Color** (agregado Phase 2), Cantidad, Subtotal
  - Botón "Eliminar" por ítem (POST a `/Carrito/Eliminar/{id}`)
  - Resumen con Total calculado
  - Botón "Confirmar pedido" ⚠️ *(pendiente de implementación — CU-07/CU-08)*
  - Mensaje de carrito vacío con enlace al catálogo
