# XA57 — Contexto del Proyecto para Agente

> Este documento consolida toda la documentación del proyecto XA57 para que el agente tenga contexto completo antes de ejecutar cualquier tarea de desarrollo.

---

## 1. Descripción General del Proyecto

**XA57** es una plataforma de comercio electrónico especializada en la venta de **coleccionables de transporte** (miniaturas de autobuses, llaveros, almohadas, peluches) con capacidad de personalización por parte del cliente.

### Stack Tecnológico
- **Backend:** ASP.NET Core MVC (.NET 8)
- **Frontend:** React 19
- **ORM:** Entity Framework Core con Npgsql
- **Base de datos:** PostgreSQL
- **Arquitectura:** Capas — Domain, Application, Infrastructure, ViewModels

### Contexto Académico
- **Institución:** Instituto Tecnológico Superior del Sur de Guanajuato
- **Carrera:** Ingeniería en Sistemas Computacionales
- **Materia:** Ingeniería de Software
- **Docente:** Fernando José Martínez López
- **Metodología:** Proceso Unificado (RUP) / Larman

---

## 2. Glosario del Dominio

| Término | Definición | Sinónimos |
|---|---|---|
| Cliente | Persona que accede al sistema para explorar, personalizar y comprar productos | Usuario final, Comprador |
| Administrador | Usuario que gestiona catálogo, pedidos y configuraciones | Admin, Gestor |
| Producto | Artículo del sistema que puede ser personalizado y adquirido | Artículo |
| Catálogo de Productos | Conjunto de productos disponibles para consulta y selección | Inventario |
| Personalización | Proceso de modificar características del producto (cromática, rotulación, número) | Configuración |
| Modelo | Diseño base del autobús elegible para personalización (ej. Irizar i8, Volvo 9800) | Diseño |
| Línea | Categoría/cromática del autobús dentro del catálogo | Gama |
| Configuración de Colores | Selección de la cromática del producto | Selección de colores |
| Rotulación y Números | Textos, nombres o números agregados al producto | Etiquetado |
| Vista Previa | Representación visual del producto antes de confirmar el pedido | Previsualización |
| Carrito de Compras | Almacén temporal de productos seleccionados antes del pedido | Carrito |
| Pedido | Solicitud formal de compra de uno o más productos personalizados | Orden |
| Estado del Pedido | Situación actual: pendiente → en producción → enviado | Seguimiento |
| Envío a Manufactura | Proceso de enviar un pedido confirmado a producción | Producción |
| Opciones de Personalización | Conjunto de atributos configurables por producto | Parámetros |

---

## 3. Modelo de Dominio

### Entidades Principales

```
Cliente
  - Puede realizar uno o varios Pedidos
  - Cada Pedido pertenece a un único Cliente

Pedido
  - Puede incluir uno o varios Productos
  - Cada Producto puede tener una Personalización asociada

Producto
  - Artículo base del catálogo
  - Requiere obligatoriamente Línea/Cromática y Modelo para venderse (RN-02)

Personalización
  - Modelo de autobús seleccionado
  - Línea/Cromática
  - Color (nombre + hex)
  - Número económico / serial
  - Rotulación / texto personalizado
  - Notas especiales
  - Posición visual del número

Administrador
  - Gestiona Productos, Personalizaciones y Pedidos
```

---

## 4. Casos de Uso

### 4.1 Actor: Cliente

| ID | Nombre | Descripción Breve |
|---|---|---|
| CU-01 | Registrarse | Crear cuenta con datos básicos para acceder a compra y seguimiento |
| CU-02 | Explorar catálogo | Navegar categorías y productos disponibles |
| CU-03 | Seleccionar producto | Elegir un producto específico e iniciar personalización |
| CU-04 | Configurar producto personalizado | ⭐ Definir modelo, línea, cromática, rotulación, número, notas |
| CU-05 | Visualizar vista previa | Ver representación visual del producto configurado |
| CU-06 | Agregar producto al carrito | Confirmar configuración y añadir al carrito |
| CU-07 | Realizar pedido personalizado | ⭐ Confirmar carrito y generar orden de compra |
| CU-08 | Pagar pedido | Introducir datos de pago y procesar transacción |
| CU-09 | Consultar estado del pedido | Ver avance del pedido desde confirmación hasta entrega |

### 4.2 Actor: Administrador

| ID | Nombre | Descripción Breve |
|---|---|---|
| CU-10 | Administrar catálogo de productos | ⭐ Agregar, modificar o eliminar productos y categorías |
| CU-11 | Agregar nuevas categorías | Crear categorías nuevas en el catálogo |
| CU-12 | Configurar opciones de personalización | Definir modelos, líneas y atributos visuales disponibles |
| CU-13 | Gestionar pedidos | Consultar y hacer seguimiento de todos los pedidos |
| CU-14 | Enviar pedido a manufactura | Enviar pedido confirmado al proceso de producción |
| CU-15 | Actualizar estado del pedido | Cambiar el estado del pedido en cada etapa |

---

## 5. Casos de Uso Detallados (Formato Completo)

### CU-04: Configurar Producto Personalizado

**Precondiciones:**
- Usuario con acceso activo a la plataforma
- Catálogo disponible y actualizado
- Opciones de personalización habilitadas

**Postcondiciones:**
- Producto personalizado almacenado en el carrito
- Configuración registrada en el sistema

**Flujo Principal:**
1. Usuario accede al catálogo
2. Usuario selecciona un producto base
3. Sistema presenta opciones de personalización
4. Usuario selecciona el **modelo de autobús**
5. Usuario selecciona la **línea de transporte**
6. Usuario define: cromática, rotulación, número económico, ruta
7. Sistema valida la compatibilidad de la configuración
8. Sistema genera vista previa en tiempo real
9. Usuario confirma la personalización
10. Sistema agrega el producto al carrito

**Flujos Alternativos:**
- `7A` — Configuración inválida: sistema notifica error → usuario corrige → retorna a paso 6
- `9A` — Cancelación: sistema descarta cambios y retorna al catálogo

**Contrato de Operación:**
```
configurarProductoPersonalizado(
  productoId,
  colorNombre,
  colorHex,
  numeroSerie,
  posicionTexto,
  notasEspeciales,
  cantidad,
  personalizacionActiva
)
```

---

### CU-07: Realizar Pedido Personalizado

**Precondiciones:**
- Al menos un producto personalizado en el carrito
- Usuario con sesión activa

**Postcondiciones:**
- Orden de compra generada con identificador único
- Notificación de confirmación enviada al usuario

**Flujo Principal:**
1. Usuario accede al carrito
2. Sistema muestra resumen del pedido
3. Usuario confirma/actualiza dirección de envío
4. Usuario selecciona método de pago
5. Sistema procesa la transacción
6. Sistema valida el resultado del pago
7. Sistema genera identificador único de la orden
8. Sistema notifica confirmación al usuario

**Flujos Alternativos:**
- `5A` — Transacción rechazada: usuario selecciona método alternativo → retorna a paso 4
- `3A` — Dirección inválida: usuario corrige → continúa en paso 4

**Contrato de Operación:**
```
realizarPedido(direccionEnvio, metodoPago)
```

---

### CU-10: Administrar Catálogo de Productos

**Precondiciones:**
- Administrador autenticado con credenciales válidas

**Postcondiciones:**
- Catálogo actualizado
- Cambios reflejados inmediatamente en la plataforma

**Flujo Principal:**
1. Administrador accede al sistema
2. Ingresa al módulo de gestión de catálogo
3. Selecciona acción: agregar / modificar / eliminar
4. Ingresa o actualiza información del producto
5. Sistema valida los datos
6. Sistema almacena los cambios
7. Sistema actualiza el catálogo público

**Flujos Alternativos:**
- `5A` — Datos inválidos: sistema muestra errores → administrador corrige → retorna a paso 4

**Contrato de Operación:**
```
administrarCatalogo(accion, datosProducto)
```

---

## 6. Especificación Complementaria (Requisitos No Funcionales)

### 6.1 Funcionalidad
- **Selector de Base obligatorio:** El sistema requiere seleccionar Modelo y Línea antes de habilitar campos opcionales (nombre, número, ruta)
- **Persistencia de personalización:** Si el usuario abandona y regresa, su configuración del carrito debe mantenerse
- **Logging de errores:** Registro de fallos de transacción y errores de servidor en base de datos para auditoría
- **Protección de datos:** Historial de pedidos y datos personales del cliente deben estar encriptados

### 6.2 Usabilidad
- **Flujo de compra:** Seleccionar Producto → Elegir Cromática → Personalizar Datos → Agregar al Carrito
- Indicar claramente qué campos son obligatorios vs opcionales
- Catálogo visual con imágenes de alta calidad; soporte para zoom en detalles de cromáticas

### 6.3 Fiabilidad
- La información de personalización debe viajar íntegra desde el carrito hasta la Orden de Compra (sin truncamiento)
- Validar y rechazar caracteres especiales no imprimibles que puedan romper el proceso de manufactura/sublimación

### 6.4 Rendimiento
- Carga de imágenes entre cromáticas: **máximo 2 segundos**
- Vista previa del producto: generación en tiempo real

### 6.5 Soporte / Escalabilidad
- Agregar una nueva Línea/Cromática debe hacerla disponible automáticamente para todos los tipos de producto compatibles (busito, almohada, llavero) — sin configuración manual por producto
- La base de datos debe soportar nuevos tipos de coleccionables (tazas, gorras) reutilizando las mismas Líneas y Modelos existentes

### 6.6 Restricciones de Implementación
- Plataforma: Aplicación Web (navegador)
- Base de datos: Relacional (PostgreSQL) para garantizar integridad entre Productos, Líneas y Personalizaciones

### 6.7 Interfaces Externas
- El sistema debe generar un reporte/vista de **lista de picking** para el administrador con todos los detalles de personalización de cada pedido (para manufactura)

---

## 7. Reglas de Negocio

| ID | Regla | Fuente |
|---|---|---|
| RN-01 | **Catálogo abierto:** Todos los productos son visibles y comprables por cualquier usuario registrado. Sin restricciones por rol. | Política de Ventas |
| RN-02 | **Dependencia de diseño:** Un producto NO puede venderse sin Línea/Cromática y Modelo asignados. | Inventario |
| RN-03 | **Límite de personalización:** Textos personalizados limitados a máximo de caracteres según área física imprimible del producto (ej. 20 caracteres). | Manufactura |
| RN-04 | **Validación de contenido:** El sistema rechaza automáticamente palabras obscenas o prohibidas en campos de texto. | Política de Uso |
| RN-05 | **Venta final:** Productos personalizados no son sujetos a devolución. El sistema debe requerir confirmación explícita del usuario sobre sus datos antes de pagar. | Política de Devoluciones |

---

## 8. Consideraciones de Dominio

- **Propiedad intelectual:** Los diseños aplicados a productos textiles son representaciones artísticas, no suplantación de vehículos reales. No requieren licencias de operación de transporte.
- **Naturaleza del producto:** Cada ítem del carrito es potencialmente único (personalización individual). El flujo de backend es distinto a un e-commerce de stock fijo.

---

## 9. Arquitectura de Componentes

```
Frontend
  ├── React 19              → configurador interactivo (Fetch API / JSON)
  └── Razor Views (.cshtml) → vistas del servidor (catálogo, carrito, detalle)

Backend
  └── Entity Framework Core
        └── ORM · Npgsql Provider → PostgreSQL

Flujo: React 19 --Fetch/JSON--> Razor Views --> EF Core --> PostgreSQL
```

---

## 10. Diagrama de Clases de Diseño (por capas)

### Domain · Entities

```
TipoProducto
  +Id, +Nombre, +MaxCaracteres : int
  +PermiteNombre : bool
  +PermiteNumeroEconomico : bool
  +PermiteRuta : bool

Producto
  +Id, +Nombre, +Descripcion?, +Precio : decimal
  +ImagenUrl?, +TipoProductoId?, +Activo : bool

ModeloAutobus
  +Id, +Nombre, +Fabricante?, +Activo : bool

Linea
  +Id, +Nombre, +ColorPrimario?, +ColorSecundario?
  +NombreOperador?, +LogoUrl?, +Activa : bool

Pedido                                  ← tabla principal del carrito (estado temporal)
  +Id, +ProductoId, +ModeloAutobusId?
  +LineaId?, +NombreOperador?
  +NumeroEconomico?, +Ruta?
  +NotasEspeciales?, +Cantidad
  +Estado : string, +FechaCreacion : DateTime
```

Relaciones de dominio:
- `Producto 0..* --> 0..1 TipoProducto`
- `Pedido 0..* --> 1 Producto`
- `Pedido 0..* --> 0..1 ModeloAutobus`
- `Pedido 0..* --> 0..1 Linea`

### Application · DTOs

```
CarritoItemDto
  +ProductoId, +ModeloAutobusId?, +LineaId?
  +NombreOperador?, +NumeroEconomico?
  +Ruta?, +NotasEspeciales?, +Cantidad

PedidoResultDto
  +Mensaje : string, +PedidoId : int
```

### Application · Interfaces y Services

| Interface | Métodos |
|---|---|
| `IProductoService` | `ObtenerTodosAsync()`, `ObtenerPorIdAsync(id)` |
| `IPedidoService` | `AgregarAsync(item)`, `ObtenerCarritoAsync()`, `EliminarItemAsync(id)`, `ContarItemsAsync()` |
| `ILineaService` | `ObtenerActivasAsync()`, `ObtenerPorIdAsync(id)` |
| `IModeloAutobusService` | `ObtenerActivosAsync()`, `ObtenerPorIdAsync(id)` |

Implementaciones: `ProductoService`, `PedidoService`, `LineaService`, `ModeloAutobusService`

### Infrastructure · Repositories

| Interface | Implementación |
|---|---|
| `IProductoRepository` | `ProductoRepository` |
| `IPedidoRepository` | `PedidoRepository` |
| `ILineaRepository` | `LineaRepository` |
| `IModeloAutobusRepository` | `ModeloAutobusRepository` |

```
AppDbContext
  +Productos : DbSet<Producto>
  +Pedidos : DbSet<Pedido>
  +Lineas : DbSet<Linea>
  +ModelosAutobus : DbSet<ModeloAutobus>
  +TiposProducto : DbSet<TipoProducto>
```

### Controllers (Presentation)

```
HomeController
  +Index() : IActionResult
  +Catalogo() : IActionResult
  → depende de: IProductoService

ProductosController
  +Detalle(id), +Catalogo() : IActionResult
  → depende de: IProductoService

CarritoController
  +Index(), +Agregar(item), +Eliminar(id), +Cantidad() : IActionResult
  → depende de: IPedidoService
  → usa: CarritoViewModel { Items: List<Pedido>, Total: decimal }

ConfiguradorApiController   ← API JSON para React
  +GetModelos(), +GetLineas(), +GetProducto(id) : IActionResult
  → depende de: IModeloAutobusService, ILineaService, IProductoService
```

---

## 11. Diagrama de Clases Conceptual

### Paquete: Identidad
```
Usuario
  +Id, +Nombre, +Apellido, +Email, +PasswordHash
  +Rol : string, +FechaRegistro : DateTime
  +Login() : bool, +CambiarPassword() : void
```

### Paquete: Catálogo
```
Categoria
  +Id, +Nombre, +Descripcion
  +ObtenerProductos() : List<Producto>

Producto
  +Id, +Nombre, +Descripcion, +Precio : decimal
  +Stock : int, +ImagenUrl, +EsPersonalizable : bool
  +CategoriaId (FK)
  +ActualizarStock(), +EstaDisponible() : bool

PersonalizacionProducto
  +Id, +ProductoId (FK), +TipoOpcion, +ValorOpcion
  +PrecioExtra : decimal
  +AplicarPersonalizacion() : void
```

### Paquete: Carrito
```
Carrito
  +Id, +UsuarioId (FK), +FechaCreacion : DateTime
  +AgregarItem(), +EliminarItem(), +CalcularTotal() : decimal, +Vaciar()

ItemCarrito
  +Id, +CarritoId (FK), +ProductoId (FK)
  +Cantidad, +PrecioUnitario : decimal
  +CalcularSubtotal() : decimal
```

### Paquete: Pedidos
```
Orden
  +Id, +UsuarioId (FK), +FechaOrden : DateTime
  +Estado : string, +Total : decimal, +DireccionEnvio
  +CalcularTotal(), +CambiarEstado()

ItemOrden
  +Id, +OrdenId (FK), +ProductoId (FK)
  +PersonalizacionId (FK), +Cantidad, +PrecioUnitario : decimal
  +CalcularSubtotal() : decimal
```

Relaciones:
- `Categoria 1 o-- 0..* Producto`
- `Producto 1 o-- 0..* PersonalizacionProducto`
- `Usuario 1 o-- 0..1 Carrito`
- `Carrito 1 *-- 1..* ItemCarrito`
- `ItemCarrito 0..* --> 1 Producto`
- `Usuario 1 o-- 0..* Orden`
- `Orden 1 *-- 1..* ItemOrden`
- `ItemOrden 0..* --> 1 Producto`
- `ItemOrden 0..* --> 0..1 PersonalizacionProducto`

---

## 12. Esquema de Base de Datos (ER)

### Identidad
```sql
Usuarios (
  Id SERIAL PK,
  Nombre VARCHAR(100), Apellido VARCHAR(100),
  Email VARCHAR(150) UNIQUE,
  PasswordHash TEXT, Rol VARCHAR(20),
  FechaRegistro TIMESTAMP
)
```

### Catálogo
```sql
Categorias (
  Id SERIAL PK,
  Nombre VARCHAR(100), Descripcion TEXT
)

Productos (
  Id SERIAL PK,
  Nombre VARCHAR(150), Descripcion TEXT,
  Precio DECIMAL(10,2), Stock INT,
  ImagenUrl TEXT, EsPersonalizable BOOLEAN,
  CategoriaId INT FK→Categorias
)

PersonalizacionesProducto (
  Id SERIAL PK,
  ProductoId INT FK→Productos,
  TipoOpcion VARCHAR(100), ValorOpcion VARCHAR(100),
  PrecioExtra DECIMAL(10,2)
)
```

### Carrito
```sql
Carritos (
  Id SERIAL PK,
  UsuarioId INT FK→Usuarios,
  FechaCreacion TIMESTAMP
)

ItemsCarrito (
  Id SERIAL PK,
  CarritoId INT FK→Carritos,
  ProductoId INT FK→Productos,
  Cantidad INT, PrecioUnitario DECIMAL(10,2)
)
```

### Pedidos
```sql
Ordenes (
  Id SERIAL PK,
  UsuarioId INT FK→Usuarios,
  FechaOrden TIMESTAMP,
  Estado VARCHAR(50), Total DECIMAL(10,2),
  DireccionEnvio TEXT
)

ItemsOrden (
  Id SERIAL PK,
  OrdenId INT FK→Ordenes,
  ProductoId INT FK→Productos,
  PersonalizacionId INT FK→PersonalizacionesProducto (nullable),
  Cantidad INT, PrecioUnitario DECIMAL(10,2)
)
```

> ⚠️ **Nota del implementador:** Actualmente `Pedidos` (EF) actúa como tabla temporal de carrito con `Estado = "Recibido"`. La migración a `Ordenes` / `ItemsOrden` separados es parte del flujo CU-07/CU-08 aún pendiente.

---

## 13. Diagramas de Secuencia

### DS-01 — Explorar Catálogo (CU-02 / CU-03)

```
GET /Home/Catalogo
  HomeController → ProductoService.ObtenerTodosAsync()
    → ProductoRepository → DB:
        SELECT p.*, t.* FROM productos p
        LEFT JOIN tipos_producto t WHERE p.activo = true
    ← List<Producto>
  ← Vista Catalogo.cshtml (grid de productos)

GET /Productos/Detalle/{id}  [usuario hace click en "Ver"]
  ProductosController → ProductoService.ObtenerPorIdAsync(id)
    → ProductoRepository → DB:
        SELECT p.*, t.* FROM productos p
        LEFT JOIN tipos_producto t WHERE p.id = {id}
    ← Producto
  ← Vista Detalle.cshtml
      + monta componente React con data-attributes de TipoProducto
```

---

### DS-02 — Configurar Producto (CU-04 / CU-05)

> Al montar el componente React, el configurador hace **3 llamadas paralelas**:

```
React (mount) → GET /api/configurador/modelos
  ConfiguradorApiController → ModeloAutobusService.ObtenerActivosAsync()
    → DB: SELECT * FROM modelos_autobus WHERE activo = true ORDER BY nombre
  ← JSON [{ id, nombre, fabricante }]

React (mount) → GET /api/configurador/lineas
  ConfiguradorApiController → LineaService.ObtenerActivasAsync()
    → DB: SELECT * FROM lineas WHERE activa = true ORDER BY nombre
  ← JSON [{ id, nombre, colorPrimario, colorSecundario, nombreOperador }]

React (mount) → GET /api/configurador/producto/{id}
  ConfiguradorApiController → ProductoService.ObtenerPorIdAsync(id)
    → DB: SELECT p.*, t.* FROM productos p JOIN tipos_producto t WHERE p.id = {id}
  ← JSON { tipoProducto: { permiteNombre, permiteNumeroEconomico, permiteRuta, maxCaracteres } }
```

Interacción del usuario:
```
Usuario selecciona ModeloAutobus  → Vista previa actualizada
Usuario selecciona Línea/Cromática → Vista previa con colores de línea
[si permiteNombre]          → Usuario ingresa NombreOperador
[si permiteNumeroEconomico] → Usuario ingresa NumeroEconomico
[si permiteRuta]            → Usuario ingresa Ruta
Usuario ajusta cantidad
Usuario agrega notas especiales (opcional)
→ Configuración lista para agregar al carrito
```

---

### DS-03 — Agregar al Carrito (CU-06)

```
Usuario click "Agregar al carrito"
  React → validarConfiguracion()
    [inválida] → muestra errores de validación
    [válida]   →
      POST /Carrito/Agregar { productoId, modeloAutobusId, lineaId,
                              nombreOperador, numeroEconomico,
                              ruta, notasEspeciales, cantidad }
        CarritoController → PedidoService.AgregarAsync(CarritoItemDto)
          → new Pedido { ..., Estado = "Recibido" }
          → PedidoRepository.AgregarAsync(pedido)
            → DB: INSERT INTO "Pedidos" (...)
          ← PedidoResultDto { mensaje, pedidoId }
        ← 200 OK { mensaje, pedidoId }

      GET /Carrito/Cantidad
        → PedidoService.ContarItemsAsync()
          → DB: SELECT COUNT(*) FROM "Pedidos"
        ← n

  ← Badge del carrito actualizado + Toast "Producto agregado al carrito"
```

---

### DS-04 — Ver Carrito / Gestionar Pedido (CU-07)

```
GET /Carrito
  CarritoController → PedidoService.ObtenerCarritoAsync()
    → PedidoRepository.ObtenerConProductosAsync()
      → DB: SELECT p.*, prod.*, m.*, l.*
            FROM "Pedidos" p
            JOIN productos prod ON p.producto_id
            LEFT JOIN modelos_autobus m ON p.modelo_autobus_id
            LEFT JOIN lineas l ON p.linea_id
    ← List<Pedido>
  ← Vista Carrito/Index.cshtml
      Resumen: Producto · Modelo · Línea · Operador · No. Económico · Ruta · Total

[Eliminar ítem]
  POST /Carrito/Eliminar/{id}
    → PedidoService.EliminarItemAsync(id)
      → DB: DELETE FROM "Pedidos" WHERE id = {id}
    ← Redirect → GET /Carrito

[Confirmar pedido]  ← PENDIENTE DE IMPLEMENTAR (CU-07 / CU-08)
  Procesamiento de pago y generación de orden
```

---

```
Cliente          → id, nombre, email, password, dirección, historial_pedidos
Administrador    → id, nombre, email, password, rol
Producto         → id, nombre, descripción, precio, imagen_base, tipo (busito/llavero/almohada)
Modelo           → id, nombre (ej. "Irizar i8", "Volvo 9800")
Linea            → id, nombre, imagen_cromática, modelo_id (FK)
Personalización  → id, producto_id (FK), linea_id (FK), color_nombre, color_hex,
                   numero_serie, posicion_texto, notas_especiales, cantidad
Carrito          → id, cliente_id (FK)
ItemCarrito      → id, carrito_id (FK), personalizacion_id (FK)
Pedido           → id, cliente_id (FK), dirección_envío, método_pago,
                   estado (pendiente/en_producción/enviado), fecha, id_único
DetallePedido    → id, pedido_id (FK), personalizacion_id (FK)
Log              → id, tipo_error, descripción, fecha, stack_trace
```

---

## 14. Resumen de Prioridades de Implementación

1. **Módulo de personalización** (CU-04) — núcleo del negocio
2. **Catálogo con imágenes y zoom** — la venta entra por la vista
3. **Carrito con persistencia** — datos de personalización no deben perderse
4. **Flujo de pedido y pago** (CU-07, CU-08)
5. **Panel admin** — catálogo, personalización, gestión de pedidos, lista de picking
6. **Notificaciones** — confirmación de pedido al cliente
7. **Logging y auditoría**
