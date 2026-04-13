# XA57 — Modelo del Dominio

> **Spec Driven Development · Documento 01**  
> Versión: 1.0 | Fecha: 2026-03-27

---

## 1. Propósito

Este documento describe los conceptos principales del dominio del negocio de XA57, sus atributos y las relaciones entre ellos. Es la fuente de verdad para nomenclatura, identidad de entidades y reglas de asociación.

---

## 2. Entidades del Dominio

### 2.1 Cliente

Persona que utiliza la plataforma para explorar el catálogo, personalizar productos y realizar pedidos.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre del cliente |
| `Apellido` | string | Apellido del cliente |
| `Email` | string (unique) | Correo electrónico — usado como identificador de login |
| `PasswordHash` | string | Contraseña cifrada |
| `Rol` | string | `"Cliente"` o `"Administrador"` |
| `FechaRegistro` | DateTime | Fecha de alta en el sistema |

**Comportamientos:**
- `Login() : bool` — valida credenciales
- `CambiarPassword() : void` — actualiza hash de contraseña

---

### 2.2 Administrador

Usuario responsable de gestionar el catálogo, las opciones de personalización y el seguimiento de pedidos. Comparte la entidad `Usuario` con el `Cliente`, diferenciado por el atributo `Rol = "Administrador"`.

---

### 2.3 Producto

Artículo base ofrecido en el sistema que puede ser seleccionado y personalizado.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre del producto |
| `Descripcion` | string? | Descripción opcional |
| `Precio` | decimal | Precio base |
| `Stock` | int | Unidades disponibles |
| `ImagenUrl` | string? | URL de imagen principal |
| `EsPersonalizable` | bool | Indica si admite configuración |
| `CategoriaId` | int (FK) | Referencia a Categoría |
| `TipoProductoId` | int? (FK) | Referencia a TipoProducto (controla campos de personalización) |
| `Activo` | bool | Visibilidad en catálogo |

**Comportamientos:**
- `ActualizarStock() : void`
- `EstaDisponible() : bool`

---

### 2.4 TipoProducto

Controla qué campos de personalización están disponibles según el tipo de artículo (busito, llavero, almohada, etc.).

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre del tipo (ej. "Busito de Peluche") |
| `MaxCaracteres` | int | Límite de caracteres para textos personalizados |
| `PermiteNombre` | bool | Habilita campo de nombre del operador |
| `PermiteNumeroEconomico` | bool | Habilita campo de número económico |
| `PermiteRuta` | bool | Habilita campo de ruta/destino |

---

### 2.5 Categoría

Agrupación de productos dentro del catálogo.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre de la categoría |
| `Descripcion` | string | Descripción opcional |

**Comportamientos:**
- `ObtenerProductos() : List<Producto>`

---

### 2.6 ModeloAutobus

Diseño base del vehículo que el cliente puede elegir para iniciar la personalización.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre del modelo (ej. "Irizar i8", "Volvo 9800") |
| `Fabricante` | string? | Fabricante del vehículo |
| `Activo` | bool | Disponible para selección |

---

### 2.7 Linea

Categoría cromática/variante del producto; representa una línea de transporte específica con su identidad visual.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `Nombre` | string | Nombre de la línea (ej. "ETN", "Estrella Roja") |
| `ColorPrimario` | string? | Hex del color principal |
| `ColorSecundario` | string? | Hex del color secundario |
| `NombreOperador` | string? | Nombre predeterminado del operador |
| `LogoUrl` | string? | URL del logotipo de la línea |
| `Activa` | bool | Disponible para selección |

---

### 2.8 Personalización

Conjunto de configuraciones aplicadas a un producto: modelo, línea, colores, rotulación y números identificativos.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `ProductoId` | int (FK) | Producto base |
| `TipoOpcion` | string | Tipo de personalización (ej. "Color", "Rotulación") |
| `ValorOpcion` | string | Valor seleccionado |
| `PrecioExtra` | decimal | Costo adicional por esta opción |

---

### 2.9 Pedido / Orden

Solicitud formal de compra realizada por un cliente. En la implementación de diseño, `Pedido` representa un ítem en el carrito con todos los atributos de personalización.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `ProductoId` | int (FK) | Producto solicitado |
| `ModeloAutobusId` | int? (FK) | Modelo seleccionado |
| `LineaId` | int? (FK) | Línea/cromática seleccionada |
| `NombreOperador` | string? | Texto personalizado del operador |
| `NumeroEconomico` | string? | Número económico o serial |
| `Ruta` | string? | Ruta o destino |
| `NotasEspeciales` | string? | Observaciones adicionales |
| `Cantidad` | int | Unidades del ítem |
| `Estado` | string | `"Recibido"` → `"En producción"` → `"Enviado"` → `"Entregado"` |
| `FechaCreacion` | DateTime | Marca de tiempo de creación |

---

### 2.10 Carrito

Contenedor temporal de ítems antes de confirmar el pedido.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `UsuarioId` | int (FK) | Dueño del carrito |
| `FechaCreacion` | DateTime | Fecha de creación |

**Comportamientos:**
- `AgregarItem() : void`
- `EliminarItem() : void`
- `CalcularTotal() : decimal`
- `Vaciar() : void`

---

## 3. Mapa de Relaciones

```
Usuario ──────────────── posee 0..1 ──────────────── Carrito
Usuario ──────────────── realiza 0..* ──────────────── Orden
Carrito ──────────────── compuesto de 1..* ──────────── ItemCarrito
Orden ──────────────────── compuesto de 1..* ──────────── ItemOrden
Categoría ──────────────── contiene 0..* ──────────────── Producto
Producto ──────────────── tiene opciones 0..* ──────────── PersonalizaciónProducto
Producto ──────────────── es de tipo 0..1 ──────────────── TipoProducto
Pedido ──────────────────── referencia 1 ──────────────── Producto
Pedido ──────────────────── basado en 0..1 ──────────────── ModeloAutobus
Pedido ──────────────────── cromática 0..1 ──────────────── Linea
ItemOrden ──────────────── aplica 0..1 ──────────────── PersonalizaciónProducto
Administrador ──────────── gestiona ──────────────── Productos, Pedidos, Lineas
```

---

## 4. Glosario del Dominio

| Término | Definición | Alias / Sinónimos |
|---|---|---|
| **Cliente** | Persona que accede al sistema para explorar, personalizar y comprar | Usuario final, Comprador |
| **Administrador** | Usuario encargado de gestionar catálogo, pedidos y configuraciones | Admin, Gestor |
| **Catálogo de Productos** | Conjunto de productos disponibles para consulta y selección | Inventario, Lista de productos |
| **Producto** | Artículo que puede ser personalizado y adquirido | Artículo |
| **Modelo** | Diseño base del vehículo (ej. Irizar i8, Scania) | Diseño |
| **Línea** | Categoría o variante cromática del producto | Gama, Cromática |
| **Personalización** | Proceso para modificar características del producto | Configuración |
| **Configuración de Colores** | Selección de esquema cromático del producto | Selección de colores |
| **Rotulación y Números** | Textos, nombres o números impresos en el producto | Etiquetado |
| **Vista Previa del Producto** | Representación visual antes de confirmar el pedido | Previsualización |
| **Carrito de Compras** | Espacio temporal de productos antes del pedido | Carrito |
| **Pedido** | Solicitud formal de compra de uno o más productos | Orden |
| **Pago** | Proceso de liquidación del pedido | Transacción |
| **Número Económico** | Identificador numérico de una unidad de transporte | Serial |
| **Manufactura bajo demanda** | Producción del producto tras recibir el pedido | — |
| **Estado del Pedido** | Etapa actual del pedido: Recibido → En producción → Enviado → Entregado | — |
