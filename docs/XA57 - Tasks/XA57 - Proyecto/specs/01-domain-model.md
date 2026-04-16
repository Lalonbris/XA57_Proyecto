# XA57 — Modelo del Dominio

> **Spec Driven Development · Documento 01**  
> Versión: 1.2 | Fecha: 2026-04-16  
> Actualizado:
> - v1.1: Refleja uso de ASP.NET Core Identity para gestión de roles
> - v1.2: Incluye campos de personalización (Color, ColorHex) en Pedido para mostrar personalización completa en carrito

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
| `Rol` | string | Rol de usuario gestionado mediante ASP.NET Core Identity (valores: `"Cliente"` o `"Administrador"`). |
| `FechaRegistro` | DateTime | Fecha de alta en el sistema |

**Comportamientos:**
- `Login() : bool` — valida credenciales
- `CambiarPassword() : void` — actualiza hash de contraseña

---

### 2.2 Administrador

Usuario responsable de gestionar el catálogo, las opciones de personalización y el seguimiento de pedidos. Comparte la entidad `Usuario` con el `Cliente`, diferenciado por el atributo `Rol = "Administrador"` gestionado mediante ASP.NET Core Identity.

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
| `CategoriaId` | int? (FK) | Referencia a Categoría |
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
| `Descripcion` | string? | Descripción opcional |

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

### 2.8 PersonalizaciónProducto

Define opciones de personalización disponibles para un producto: modelo, línea, colores, rotulación y números identificativos.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `TipoOpcion` | string | Tipo de personalización (ej. "Color", "Rotulación") |
| `ValorOpcion` | string | Valor seleccionado |
| `PrecioExtra` | decimal | Costo adicional por esta opción |

---

### 2.9 Pedido

Solicitud formal de compra realizada por un cliente. Representa un ítem en el carrito con todos los atributos de personalización seleccionados por el usuario.

**Nota:** Los campos de personalización se almacenan directamente en el Pedido para que el carrito muestre al usuario exactamente lo que personalizó, mejorando la experiencia de usuario.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `ProductoId` | int (FK) | Producto solicitado |
| `ModeloAutobusId` | int? (FK) | Modelo seleccionado |
| `LineaId` | int? (FK) | Línea/cromática seleccionada |
| `NombreOperador` | string? | Texto personalizado del operador |
| `NumeroEconomico` | string? | Número económico o serial |
| `Color` | string? | Nombre del color seleccionado (ej. "Rojo", "Azul") |
| `ColorHex` | string? | Código hexadecimal del color (ej. "#FF0000") |
| `Ruta` | string? | Ruta o destino |
| `NotasEspeciales` | string? | Observaciones adicionales |
| `Cantidad` | int | Unidades del ítem |
| `Estado` | string | `"Recibido"` → `"En producción"` → `"Enviado"` → `"Entregado"` |
| `FechaCreacion` | DateTime | Marca de tiempo de creación |

**Estados posibles:**
- `Recibido`: Pedido recibido, pendiente de confirmación
- `En producción`: Pedido confirmado, siendo personalizado/fabricado
- `Enviado`: Pedido enviado al cliente
- `Entregado`: Pedido entregado al cliente

---

### 2.10 Carrito

Contenedor temporal de ítems antes de confirmar el pedido. Un usuario tiene un único carrito activo.

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

### 2.11 ItemCarrito

Ítem individual dentro del carrito de compras.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `CarritoId` | int (FK) | Carrito al que pertenece |
| `ProductoId` | int (FK) | Producto seleccionado |
| `Cantidad` | int | Cantidad de unidades |

---

### 2.12 Orden

Solicitud formal de compra confirmada por el cliente. Compuesta por uno o más ItemOrden.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `UsuarioId` | int (FK) | Cliente que realizó la orden |
| `FechaCreacion` | DateTime | Fecha de confirmación |
| `Estado` | string | Estado de la orden |

---

### 2.13 ItemOrden

Producto específico dentro de una orden, puede incluir una personalización aplicada.

| Atributo | Tipo | Descripción |
|---|---|---|
| `Id` | int | Identificador único |
| `OrdenId` | int (FK) | Orden a la que pertenece |
| `ProductoId` | int (FK) | Producto solicitado |
| `Cantidad` | int | Cantidad de unidades |
| `PersonalizacionId` | int? (FK) | Personalización aplicada (opcional) |

---

## 3. Mapa de Relaciones

```
Usuario (ApplicationUser)
    ├───── posee 0..1 ──────── Carrito
    │         └───── compuesto de 1..* ──── ItemCarrito
    │
    └───── realiza 0..* ──────── Orden
              └───── compuesto de 1..* ──── ItemOrden
                       └───── aplica 0..1 ──── PersonalizacionProducto

Categoría
    └───── contiene 0..* ──────── Producto
              ├───── tiene opciones 0..* ──── PersonalizacionProducto
              └───── es de tipo 0..1 ──── TipoProducto

Pedido (ítem del carrito con personalización)
    ├───── referencia 1 ──────── Producto
    ├───── basado en 0..1 ──────── ModeloAutobus
    ├───── cromática 0..1 ──────── Linea

Administrador ──────────── gestiona ──────────── Productos, Pedidos, Lineas
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

---

## 5. Notas de Implementación

### 5.1 Gestión de Roles
Los roles de usuario se gestionan mediante **ASP.NET Core Identity** (IdentityRole, RoleManager), no como un campo string en la entidad Usuario.

### 5.2 Almacenamiento de Personalización en Pedido
Para mejorar la **experiencia de usuario**, los campos de personalización (Color, ColorHex, NombreOperador, NumeroEconomico, etc.) se almacenan **directamente en la entidad Pedido**. Esto permite que:
- El carrito muestre exactamente lo que el usuario personalizó
- No se pierda la información si el catálogo de personalizaciones cambia
- Sea más simple consultar y visualizar la personalización sin joins adicionales

### 5.3 Flujo de Carrito vs Orden
- **Carrito**: Almacena temporalmente los productos que el usuario ha seleccionado/configurado
- **Pedido**: Representa cada ítem individual en el carrito (denormalizado para mostrar personalización completa)
- **Orden**: Representa la confirmación formal de compra, compuesta por uno o más ItemOrden
