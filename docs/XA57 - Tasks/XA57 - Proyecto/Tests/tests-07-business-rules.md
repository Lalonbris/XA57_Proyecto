# XA57 — Reglas de Negocio

> **Spec Driven Development · Documento 07**  
> Versión: 1.0 | Fecha: 2026-03-27  
> Fuente: Especificación Complementaria XA57 v1.0

---

## 1. Catálogo

### RN-01 — Catálogo Abierto

**Fuente:** Política de Ventas

> Todos los productos, líneas y modelos son visibles y adquiribles por cualquier usuario (registrado o visitante para navegación). No existen restricciones de visualización por rol.

| Atributo | Valor |
|---|---|
| **Aplica a** | Módulos de catálogo, exploración y detalle de producto |
| **Implementación** | Los endpoints `GET /Home/Catalogo` y `GET /Productos/Detalle/{id}` no requieren autenticación |
| **Excepción** | Agregar al carrito y realizar pedido sí requieren sesión activa |

---

### RN-02 — Dependencia de Diseño (Modelo + Línea Obligatorios)

**Fuente:** Inventario / Manufactura

> Un producto físico (ej. Busito de Peluche) **no puede venderse** sin tener asignados una "Línea/Cromática" y un "Modelo" específicos.

| Atributo | Valor |
|---|---|
| **Aplica a** | CU-04 (Configurar producto), CU-06 (Agregar al carrito) |
| **Implementación** | Validación en React antes del POST + validación en `PedidoService.AgregarAsync()` |
| **Error** | Si `ModeloAutobusId` o `LineaId` son nulos al intentar agregar al carrito → `400 Bad Request` |

---

### RN-03 — Límites de Personalización (Texto)

**Fuente:** Manufactura

> Los textos personalizados (Nombres, Rutas, Números Económicos) tienen un límite de caracteres definido por el `TipoProducto.MaxCaracteres`. El texto no debe truncarse; si supera el límite, debe rechazarse con un mensaje claro.

| Atributo | Valor |
|---|---|
| **Aplica a** | Campos de texto en CU-04 |
| **Implementación** | Validación en React (conteo en tiempo real) + validación en backend |
| **Límites de ejemplo** | Busito de Peluche: 25 chars · Llavero: 15 chars · Almohada: 30 chars |
| **Error** | Texto que exceda `MaxCaracteres` → campo marcado en error, no se permite agregar al carrito |

---

### RN-04 — Caracteres Permitidos en Personalización

**Fuente:** Manufactura (proceso de sublimación)

> Los textos personalizados (NombreOperador, NumeroEconomico, Ruta, NotasEspeciales) **no deben contener caracteres especiales no imprimibles** que puedan causar errores en el proceso de manufactura/sublimación.

| Atributo | Valor |
|---|---|
| **Aplica a** | Todos los campos de texto del configurador |
| **Implementación** | Regex de validación en React y en el backend (Service/Controller) |
| **Regex sugerida** | `^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]+$` |
| **Error** | Caracteres no permitidos → campo marcado en error |

---

### RN-05 — Integridad del Texto en la Orden

**Fuente:** Manufactura

> La información de personalización debe viajar íntegra desde el carrito hasta la Orden de Compra final. No puede haber truncamiento de ningún campo (ej. si el usuario escribe "Expreso Futura", no debe guardarse solo "Expreso").

| Atributo | Valor |
|---|---|
| **Aplica a** | Persistencia de `Pedido`, generación de `Orden` |
| **Implementación** | Longitudes de columna SQL definidas por `TipoProducto.MaxCaracteres`; nunca `VARCHAR` menor al límite permitido |
| **Verificación** | Pruebas de integración con textos de longitud máxima |

---

## 2. Catálogo y Escalabilidad

### RN-06 — Disponibilidad Automática de Nueva Línea

**Fuente:** Catálogo / Escalabilidad

> Al agregar una nueva "Línea de Autobús", esta debe quedar disponible automáticamente para **todos** los tipos de productos compatibles (Busito, Almohada, Llavero) sin necesidad de configurar producto por producto.

| Atributo | Valor |
|---|---|
| **Aplica a** | CU-12 (Configurar opciones de personalización) |
| **Implementación** | La relación Línea–Pedido es directa (FK `LineaId`); el configurador React carga todas las líneas activas sin filtro por tipo de producto |
| **Efecto** | Agregar una `Linea` con `Activa = true` la hace visible para todos los productos personalizables |

---

### RN-07 — Extensibilidad de Tipos de Coleccionable

**Fuente:** Visión del Producto

> La plataforma debe permitir agregar nuevos tipos de coleccionables (ej. tazas, gorras) reutilizando las líneas y modelos ya cargados, sin refactorización mayor.

| Atributo | Valor |
|---|---|
| **Aplica a** | Estructura del modelo de datos |
| **Implementación** | Crear un nuevo registro en `TiposProducto` con las flags correspondientes (`PermiteNombre`, `PermiteNumeroEconomico`, `PermiteRuta`, `MaxCaracteres`) y asociarlo a los nuevos `Productos` |
| **Sin impacto en** | Lógica de servicios, repositorios, o vistas existentes |

---

## 3. Usuarios y Acceso

### RN-08 — Separación de Roles

**Fuente:** Modelo de dominio

> El sistema reconoce dos roles: `"Cliente"` y `"Administrador"`. Los administradores tienen acceso al panel de gestión; los clientes solo acceden al catálogo y a su propio historial.

| Rol | Acceso |
|---|---|
| `Cliente` | Catálogo, configurador, carrito, historial de pedidos propios |
| `Administrador` | Todo lo anterior + gestión de catálogo, líneas, modelos, pedidos de todos los usuarios, lista de picking, actualización de estados |

---

### RN-09 — Protección de Datos Personales

**Fuente:** Seguridad

> Aunque el catálogo es público, los datos del cliente (dirección de envío, historial de pedidos con personalizaciones) deben estar protegidos. Las contraseñas se almacenan únicamente como hash.

| Atributo | Valor |
|---|---|
| **Implementación** | Protocolo HTTPS obligatorio en producción; `PasswordHash` en tabla `Usuarios`; datos de sesión protegidos |

---

## 4. Pedidos y Manufactura

### RN-10 — Estado Inicial del Pedido

> Todo pedido recién agregado al carrito inicia con `Estado = "Recibido"`.

| Atributo | Valor |
|---|---|
| **Aplica a** | `PedidoService.AgregarAsync()` |
| **Implementación** | Valor por defecto en la entidad y en la columna SQL |

---

### RN-11 — Flujo de Estado del Pedido

> Los estados de un pedido siguen una secuencia unidireccional. No se puede retroceder un estado.

```
Recibido → En producción → Enviado → Entregado
```

| Atributo | Valor |
|---|---|
| **Aplica a** | CU-15 (Actualizar estado del pedido) |
| **Implementación** | Validación en `PedidoService` al actualizar estado; rechazar transiciones inválidas |

---

### RN-12 — Lista de Picking para Manufactura

> Al enviar un pedido a manufactura (CU-14), el sistema debe generar una **Lista de Picking** que incluya todos los atributos de personalización de forma clara y sin ambigüedad.

**Campos obligatorios en la Lista de Picking:**

| Campo | Fuente |
|---|---|
| Producto (nombre y tipo) | `Producto.Nombre`, `TipoProducto.Nombre` |
| Modelo de autobús | `ModeloAutobus.Nombre`, `ModeloAutobus.Fabricante` |
| Línea / Cromática | `Linea.Nombre`, `Linea.ColorPrimario`, `Linea.ColorSecundario` |
| Nombre del operador | `Pedido.NombreOperador` |
| Número económico | `Pedido.NumeroEconomico` |
| Ruta / destino | `Pedido.Ruta` |
| Notas especiales | `Pedido.NotasEspeciales` |
| Cantidad | `Pedido.Cantidad` |
| Fecha del pedido | `Pedido.FechaCreacion` |

---

## 5. Resumen de Reglas

| ID | Nombre | Área | Criticidad |
|---|---|---|---|
| RN-01 | Catálogo Abierto | Ventas | Alta |
| RN-02 | Dependencia de Diseño (Modelo + Línea) | Manufactura | Alta |
| RN-03 | Límites de Personalización (texto) | Manufactura | Alta |
| RN-04 | Caracteres Permitidos | Manufactura | Alta |
| RN-05 | Integridad del Texto en Orden | Manufactura | Alta |
| RN-06 | Disponibilidad Automática de Nueva Línea | Catálogo | Alta |
| RN-07 | Extensibilidad de Tipos de Coleccionable | Arquitectura | Media |
| RN-08 | Separación de Roles | Seguridad | Alta |
| RN-09 | Protección de Datos Personales | Seguridad | Alta |
| RN-10 | Estado Inicial del Pedido | Pedidos | Alta |
| RN-11 | Flujo de Estado del Pedido | Pedidos | Alta |
| RN-12 | Lista de Picking para Manufactura | Manufactura | Alta |
