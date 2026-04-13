# XA57 — Contratos de API

> **Spec Driven Development · Documento 06**  
> Versión: 1.0 | Fecha: 2026-03-27  
> Fuente: Diagramas de secuencia DS-01, DS-02, DS-03, DS-04

---

## Convenciones

- **Base URL:** `https://{host}`
- **Formato de respuesta:** JSON (para endpoints de API) o HTML (para endpoints Razor)
- **Autenticación:** Por sesión ASP.NET Core (cookie)
- Todos los endpoints retornan `Content-Type: application/json` salvo indicación contraria

---

## 1. Catálogo — `HomeController` / `ProductosController`

### GET /Home/Catalogo

**Descripción:** Carga el grid de productos activos del catálogo público.  
**Actor:** Usuario (sin autenticación requerida)  
**Retorna:** Vista Razor `Catalogo.cshtml`

**Flujo interno:**
```
HomeController → ProductoService.ObtenerTodosAsync()
              → ProductoRepository.ObtenerTodosAsync()
              → SELECT p.*, t.*
                FROM productos p
                LEFT JOIN tipos_producto t ON p.tipo_producto_id = t.id
                WHERE p.activo = true
```

**Respuesta:** Vista HTML con grid de productos.

---

### GET /Productos/Detalle/{id}

**Descripción:** Muestra el detalle de un producto e inicializa el componente React de personalización.  
**Parámetros de ruta:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | int | ID del producto |

**Flujo interno:**
```
ProductosController → ProductoService.ObtenerPorIdAsync(id)
                   → ProductoRepository.ObtenerPorIdAsync(id)
                   → SELECT p.*, t.*
                     FROM productos p
                     LEFT JOIN tipos_producto t ON p.tipo_producto_id = t.id
                     WHERE p.id = {id}
```

**Respuesta:** Vista HTML `Detalle.cshtml` con `data-attributes` del `TipoProducto` para React.

```html
<!-- Ejemplo de data-attributes inyectados por Razor -->
<div id="configurador-root"
     data-producto-id="5"
     data-permite-nombre="true"
     data-permite-numero-economico="true"
     data-permite-ruta="false"
     data-max-caracteres="25">
</div>
```

**Errores:**

| Código | Condición |
|---|---|
| `404 Not Found` | El producto no existe o no está activo |

---

## 2. Configurador API — `ConfiguradorApiController`

Estos endpoints son consumidos exclusivamente por el componente React al montarse. Las 3 llamadas se realizan en paralelo.

### GET /api/configurador/modelos

**Descripción:** Retorna la lista de modelos de autobús disponibles para personalización.

**Respuesta `200 OK`:**
```json
[
  {
    "id": 1,
    "nombre": "Irizar i8",
    "fabricante": "Irizar"
  },
  {
    "id": 2,
    "nombre": "Volvo 9800",
    "fabricante": "Volvo"
  },
  {
    "id": 3,
    "nombre": "Scania Touring",
    "fabricante": "Scania"
  }
]
```

**Flujo interno:**
```
ConfiguradorApiController → ModeloAutobusService.ObtenerActivosAsync()
                         → SELECT * FROM modelos_autobus
                           WHERE activo = true
                           ORDER BY nombre
```

---

### GET /api/configurador/lineas

**Descripción:** Retorna la lista de líneas/cromáticas disponibles.

**Respuesta `200 OK`:**
```json
[
  {
    "id": 1,
    "nombre": "ETN",
    "colorPrimario": "#FF0000",
    "colorSecundario": "#FFFFFF",
    "nombreOperador": "ETN Turistar"
  },
  {
    "id": 2,
    "nombre": "Omnibus de México",
    "colorPrimario": "#003087",
    "colorSecundario": "#C8A800",
    "nombreOperador": null
  }
]
```

**Flujo interno:**
```
ConfiguradorApiController → LineaService.ObtenerActivasAsync()
                         → SELECT * FROM lineas
                           WHERE activa = true
                           ORDER BY nombre
```

---

### GET /api/configurador/producto/{id}

**Descripción:** Retorna las flags de personalización del `TipoProducto` asociado al producto.

**Parámetros de ruta:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | int | ID del producto |

**Respuesta `200 OK`:**
```json
{
  "id": 5,
  "nombre": "Busito de Peluche Irizar",
  "precio": 350.00,
  "tipoProducto": {
    "id": 1,
    "nombre": "Busito de Peluche",
    "permiteNombre": true,
    "permiteNumeroEconomico": true,
    "permiteRuta": true,
    "maxCaracteres": 25
  }
}
```

**Flujo interno:**
```
ConfiguradorApiController → ProductoService.ObtenerPorIdAsync(id)
                         → SELECT p.*, t.*
                           FROM productos p
                           JOIN tipos_producto t ON p.tipo_producto_id = t.id
                           WHERE p.id = {id}
```

**Errores:**

| Código | Condición |
|---|---|
| `404 Not Found` | El producto no existe |

---

## 3. Carrito — `CarritoController`

### POST /Carrito/Agregar

**Descripción:** Agrega un producto personalizado al carrito. Crea un registro de `Pedido` con `Estado = "Recibido"`.

**Body (JSON):**
```json
{
  "productoId": 5,
  "modeloAutobusId": 1,
  "lineaId": 2,
  "nombreOperador": "Juan Pérez",
  "numeroEconomico": "105",
  "ruta": "México - Guadalajara",
  "notasEspeciales": "Llanta blanca por favor",
  "cantidad": 1
}
```

**Campos del body:**

| Campo | Tipo | Obligatorio | Descripción |
|---|---|---|---|
| `productoId` | int | ✅ | ID del producto base |
| `modeloAutobusId` | int | ✅ | ID del modelo de autobús seleccionado |
| `lineaId` | int | ✅ | ID de la línea/cromática seleccionada |
| `nombreOperador` | string? | ❌ | Texto del nombre del operador (si `PermiteNombre`) |
| `numeroEconomico` | string? | ❌ | Número económico / serial (si `PermiteNumeroEconomico`) |
| `ruta` | string? | ❌ | Ruta o destino (si `PermiteRuta`) |
| `notasEspeciales` | string? | ❌ | Observaciones adicionales |
| `cantidad` | int | ✅ | Cantidad de unidades (mínimo: 1) |

**Respuesta `200 OK`:**
```json
{
  "mensaje": "Producto agregado al carrito",
  "pedidoId": 42
}
```

**Flujo interno:**
```
React → POST /Carrito/Agregar
     → CarritoController → PedidoService.AgregarAsync(CarritoItemDto)
                        → new Pedido { ..., Estado = "Recibido" }
                        → PedidoRepository.AgregarAsync(pedido)
                        → INSERT INTO "Pedidos" (...)
                        → Retorna Id generado
     → Respuesta: PedidoResultDto { mensaje, pedidoId }
```

**Errores:**

| Código | Condición |
|---|---|
| `400 Bad Request` | Configuración inválida (campos requeridos faltantes, caracteres no permitidos) |
| `404 Not Found` | Producto, modelo o línea no encontrado |

---

### GET /Carrito/Cantidad

**Descripción:** Retorna el número total de ítems en el carrito. Usado por React para actualizar el badge.

**Respuesta `200 OK`:**
```json
3
```

**Flujo interno:**
```
CarritoController → PedidoService.ContarItemsAsync()
               → SELECT COUNT(*) FROM "Pedidos"
               → Retorna n
```

---

### GET /Carrito

**Descripción:** Carga la vista del carrito con todos los ítems y sus detalles.

**Respuesta:** Vista Razor `Carrito/Index.cshtml`

**Datos mostrados por ítem:**
- Producto (nombre, imagen, precio)
- Modelo de autobús seleccionado
- Línea/cromática seleccionada
- Nombre del operador
- Número económico
- Ruta
- Subtotal

**Flujo interno:**
```
CarritoController → PedidoService.ObtenerCarritoAsync()
               → PedidoRepository.ObtenerConProductosAsync()
               → SELECT p.*, prod.nombre, prod.precio,
                         m.nombre AS modelo, l.nombre AS linea
                  FROM "Pedidos" p
                  JOIN productos prod ON p.producto_id = prod.id
                  LEFT JOIN modelos_autobus m ON p.modelo_autobus_id = m.id
                  LEFT JOIN lineas l ON p.linea_id = l.id
```

---

### POST /Carrito/Eliminar/{id}

**Descripción:** Elimina un ítem del carrito.

**Parámetros de ruta:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | int | ID del Pedido a eliminar |

**Respuesta:** Redirect a `GET /Carrito` con el carrito actualizado.

**Flujo interno:**
```
CarritoController → PedidoService.EliminarItemAsync(id)
               → PedidoRepository.EliminarAsync(id)
               → DELETE FROM "Pedidos" WHERE id = {id}
```

**Errores:**

| Código | Condición |
|---|---|
| `404 Not Found` | El ítem no existe |

---

## 4. Resumen de Endpoints

| Método | Ruta | Tipo respuesta | Autenticación | Caso de uso |
|---|---|---|---|---|
| `GET` | `/Home/Catalogo` | HTML (Razor) | No | CU-02 |
| `GET` | `/Productos/Detalle/{id}` | HTML (Razor) | No | CU-03 |
| `GET` | `/api/configurador/modelos` | JSON | No | CU-04 |
| `GET` | `/api/configurador/lineas` | JSON | No | CU-04 |
| `GET` | `/api/configurador/producto/{id}` | JSON | No | CU-04 |
| `POST` | `/Carrito/Agregar` | JSON | Sí | CU-06 |
| `GET` | `/Carrito/Cantidad` | JSON | Sí | CU-06 |
| `GET` | `/Carrito` | HTML (Razor) | Sí | CU-07 |
| `POST` | `/Carrito/Eliminar/{id}` | Redirect | Sí | CU-07 |

---

## 5. Validaciones de Entrada (Reglas Aplicables)

| Campo | Regla | Error |
|---|---|---|
| `productoId` | > 0, producto activo existente | `400` |
| `modeloAutobusId` | > 0, modelo activo existente | `400` |
| `lineaId` | > 0, línea activa existente | `400` |
| `nombreOperador` | Sin caracteres no imprimibles; longitud ≤ `MaxCaracteres` | `400` |
| `numeroEconomico` | Alfanumérico; longitud ≤ 20 | `400` |
| `ruta` | Sin caracteres especiales; longitud ≤ 150 | `400` |
| `cantidad` | Entero ≥ 1 | `400` |
