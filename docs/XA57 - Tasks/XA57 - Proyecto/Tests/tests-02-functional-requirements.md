# XA57 — Requisitos Funcionales (Casos de Uso)

> **Spec Driven Development · Documento 02**  
> Versión: 1.0 | Fecha: 2026-03-27

---

## Visión General del Modelo de Casos de Uso

| ID | Caso de Uso | Actor | Prioridad |
|---|---|---|---|
| CU-01 | Registrarse | Cliente | Alta |
| CU-02 | Explorar catálogo | Cliente | Alta |
| CU-03 | Seleccionar producto | Cliente | Alta |
| CU-04 | Configurar producto personalizado | Cliente | Alta |
| CU-05 | Visualizar vista previa del producto | Cliente | Alta |
| CU-06 | Agregar producto al carrito | Cliente | Alta |
| CU-07 | Realizar pedido personalizado | Cliente | Alta |
| CU-08 | Pagar pedido | Cliente | Alta |
| CU-09 | Consultar estado del pedido | Cliente | Media |
| CU-10 | Administrar catálogo de productos | Administrador | Alta |
| CU-11 | Agregar nuevas categorías de productos | Administrador | Media |
| CU-12 | Configurar opciones de personalización | Administrador | Alta |
| CU-13 | Gestionar pedidos | Administrador | Alta |
| CU-14 | Enviar pedido a manufactura | Administrador | Alta |
| CU-15 | Actualizar estado del pedido | Administrador | Alta |

---

## CU-01: Registrarse

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** Un usuario crea una cuenta para acceder a las funciones de compra y seguimiento de pedidos.

**Precondiciones:**
- El usuario no tiene una cuenta registrada con el mismo email.

**Postcondiciones:**
- Se crea un registro de `Usuario` con `Rol = "Cliente"` en la base de datos.
- El usuario puede iniciar sesión y acceder al sistema.

**Flujo principal:**
1. El usuario accede a la opción de registro.
2. El sistema solicita: nombre, apellido, email, contraseña.
3. El usuario proporciona los datos.
4. El sistema valida que el email no esté registrado y que los datos sean correctos.
5. El sistema almacena al usuario con su `PasswordHash` y fecha de registro.
6. El sistema confirma que la cuenta fue creada y redirige al login.

**Flujos alternativos:**
- **4A. Email ya registrado:** El sistema informa que el email ya existe y solicita uno diferente.
- **4B. Datos inválidos:** El sistema indica los campos con error y el usuario los corrige.

---

## CU-02: Explorar Catálogo

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** El usuario navega por el catálogo abierto de productos disponibles.

**Precondiciones:**
- El catálogo tiene al menos un producto activo.

**Postcondiciones:**
- El usuario visualiza los productos y puede seleccionar uno.

**Flujo principal:**
1. El usuario accede a `GET /Home/Catalogo`.
2. El sistema invoca `ProductoService.ObtenerTodosAsync()`.
3. El repositorio consulta `SELECT p.*, t.* FROM productos p LEFT JOIN tipos_producto t WHERE p.activo = true`.
4. El sistema renderiza la vista con grid de productos (imágenes, nombre, precio).
5. El usuario puede filtrar por categoría o navegar entre productos.

**Requisitos de UI:**
- Las imágenes deben ser de alta calidad y permitir zoom.
- El catálogo debe cargar en menos de 2 segundos.

---

## CU-03: Seleccionar Producto

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** El usuario elige un producto específico del catálogo para ver su detalle e iniciar la personalización.

**Precondiciones:**
- El catálogo está disponible y actualizado.

**Postcondiciones:**
- El sistema muestra el detalle del producto y monta el configurador React.

**Flujo principal:**
1. El usuario hace clic en "Ver" sobre un producto.
2. El sistema ejecuta `GET /Productos/Detalle/{id}`.
3. `ProductoService.ObtenerPorIdAsync(id)` recupera el producto con su `TipoProducto`.
4. El sistema renderiza `Detalle.cshtml` con los datos del producto.
5. Razor inyecta en `data-attributes` las flags de `TipoProducto` para el componente React.
6. El componente React se monta con las opciones de personalización disponibles.

---

## CU-04: Configurar Producto Personalizado ⭐

**Actor principal:** Cliente  
**Prioridad:** Alta  
**Implementado con:** React 19 (configurador-client) + `ConfiguradorApiController`

**Descripción:** El usuario selecciona un producto base y define sus características personalizadas: modelo de autobús, línea/cromática, nombre del operador, número económico y ruta. El sistema valida la configuración y la persiste en el carrito.

**Precondiciones:**
- El usuario tiene acceso activo a la plataforma.
- El catálogo está disponible y actualizado.
- El producto seleccionado admite opciones de personalización (`EsPersonalizable = true`).

**Postcondiciones:**
- El producto personalizado queda almacenado en el carrito.
- La configuración se registra en el sistema para su posterior procesamiento.

**Flujo principal:**
1. Al montarse el componente React, se hacen 3 llamadas en paralelo:
   - `GET /api/configurador/modelos` → lista de modelos de autobús activos.
   - `GET /api/configurador/lineas` → lista de líneas activas con colores.
   - `GET /api/configurador/producto/{id}` → flags de personalización del `TipoProducto`.
2. El sistema presenta las opciones disponibles según `TipoProducto`.
3. El usuario selecciona el `ModeloAutobus`.
4. El usuario selecciona la `Linea` / cromática (la vista previa actualiza colores en tiempo real).
5. Si `PermiteNombre = true` → el usuario ingresa el nombre del operador.
6. Si `PermiteNumeroEconomico = true` → el usuario ingresa el número económico.
7. Si `PermiteRuta = true` → el usuario ingresa la ruta/destino.
8. El usuario ajusta la cantidad y agrega notas especiales (opcional).
9. El sistema valida la configuración.
10. El usuario confirma y agrega al carrito (ver CU-06).

**Flujos alternativos:**
- **9A. Configuración inválida:** El sistema notifica los errores (campos vacíos obligatorios, caracteres no permitidos). El usuario corrige y vuelve al paso correspondiente.
- **10A. Cancelación:** El sistema descarta los cambios y retorna al catálogo.

**Reglas de validación:**
- El campo `ModeloAutobus` es obligatorio.
- La `Linea`/cromática es obligatoria (ver RN-02).
- Los textos no deben contener caracteres especiales no imprimibles (ver RN-03).
- Los textos no deben truncarse: se almacenan completos.

**Requisitos de UI:**
- El flujo debe ser: `Seleccionar Producto → Elegir Cromática → Personalizar Datos → Agregar al Carrito`.
- La validación debe ejecutarse sin demoras perceptibles.
- Los campos obligatorios deben estar claramente diferenciados de los opcionales.

---

## CU-05: Visualizar Vista Previa del Producto

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** El sistema genera una representación visual del producto con las características seleccionadas.

**Precondiciones:**
- El usuario ha seleccionado al menos el modelo y la línea.

**Postcondiciones:**
- El usuario puede revisar el resultado visual antes de confirmar la compra.

**Flujo principal:**
1. Al seleccionar `ModeloAutobus`, la vista previa se actualiza con el modelo.
2. Al seleccionar `Linea`, la vista previa aplica los colores (`ColorPrimario`, `ColorSecundario`).
3. Al ingresar texto (nombre, número, ruta), la vista previa refleja el rotulado.
4. El usuario aprueba visualmente el producto antes de agregar al carrito.

**Requisitos técnicos:**
- La actualización de vista previa debe ser reactiva (sin reload de página).
- El cambio entre cromáticas distintas no debe tomar más de 2 segundos.

---

## CU-06: Agregar Producto al Carrito

**Actor principal:** Cliente  
**Prioridad:** Alta  
**Implementado con:** React → `CarritoController`

**Descripción:** El usuario confirma la configuración y agrega el producto personalizado al carrito.

**Precondiciones:**
- La configuración del producto es válida.
- El usuario tiene acceso activo.

**Postcondiciones:**
- Se crea un registro de `Pedido` con `Estado = "Recibido"` en la base de datos.
- El badge del carrito en la UI se actualiza con el nuevo conteo.

**Flujo principal:**
1. El usuario hace clic en "Agregar al carrito".
2. React ejecuta `validarConfiguracion()` localmente.
3. React envía `POST /Carrito/Agregar` con el payload `CarritoItemDto`.
4. `CarritoController` invoca `PedidoService.AgregarAsync(CarritoItemDto)`.
5. El servicio construye un nuevo `Pedido` y llama a `PedidoRepository.AgregarAsync(pedido)`.
6. El repositorio ejecuta `INSERT INTO "Pedidos" (...)` y retorna el `Id` generado.
7. El servicio retorna `PedidoResultDto { mensaje, pedidoId }`.
8. React llama a `GET /Carrito/Cantidad` y actualiza el badge.
9. Se muestra un Toast: "Producto agregado al carrito".

**Flujos alternativos:**
- **2A. Configuración inválida:** React muestra errores de validación. No se envía el POST.

**Payload `CarritoItemDto`:**
```json
{
  "productoId": 1,
  "modeloAutobusId": 3,
  "lineaId": 7,
  "nombreOperador": "Juan Pérez",
  "numeroEconomico": "105",
  "ruta": "México - Guadalajara",
  "notasEspeciales": "Color especial de llanta",
  "cantidad": 2
}
```

---

## CU-07: Realizar Pedido Personalizado ⭐

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** El usuario confirma la compra de los productos en el carrito y genera una orden de compra.

**Precondiciones:**
- El usuario tiene al menos un producto personalizado en el carrito.

**Postcondiciones:**
- Se genera una `Orden` registrada en el sistema.
- El usuario recibe una notificación de confirmación.

**Flujo principal:**
1. El usuario accede al carrito (`GET /Carrito`).
2. El sistema carga todos los pedidos del carrito con sus datos relacionados (producto, modelo, línea).
3. La vista muestra resumen: Producto · Modelo · Línea · Operador · No. Económico · Ruta · Total.
4. El usuario revisa el resumen y hace clic en "Confirmar pedido".
5. El sistema solicita confirmación de información de envío.
6. Se genera la `Orden` con los ítems del carrito.
7. El sistema notifica al usuario con la confirmación del pedido.

> ⚠️ **Estado de implementación:** El procesamiento de pago y la generación formal de orden (CU-07 / CU-08) está pendiente de implementación completa.

---

## CU-08: Pagar Pedido

**Actor principal:** Cliente  
**Prioridad:** Alta

**Descripción:** El usuario introduce los datos de pago y el sistema procesa la transacción.

**Precondiciones:**
- El usuario tiene una orden generada.

**Postcondiciones:**
- La transacción se procesa correctamente.
- El pedido queda confirmado y pasa a estado de producción.

**Flujo principal:**
1. El usuario introduce los datos de pago.
2. El sistema procesa la transacción con el servicio externo de pago.
3. El sistema valida el resultado del pago.
4. Si exitoso: la orden se confirma y se notifica al usuario.
5. Si fallido: el sistema informa el error y permite reintentar.

> ⚠️ **Estado de implementación:** Pendiente. Requiere integración con pasarela de pago externa.

---

## CU-09: Consultar Estado del Pedido

**Actor principal:** Cliente  
**Prioridad:** Media

**Descripción:** El usuario consulta el avance de su pedido desde la confirmación hasta la entrega.

**Precondiciones:**
- El usuario tiene al menos un pedido confirmado.

**Postcondiciones:**
- El usuario visualiza el estado actual del pedido.

**Flujo principal:**
1. El usuario accede a su historial de pedidos.
2. El sistema muestra la lista de órdenes con su estado actual.
3. El usuario selecciona un pedido para ver el detalle.
4. El sistema muestra el progreso: `Recibido → En producción → Enviado → Entregado`.

---

## CU-10: Administrar Catálogo de Productos

**Actor principal:** Administrador  
**Prioridad:** Alta

**Descripción:** El administrador gestiona el catálogo: crea, modifica y elimina productos y categorías.

**Precondiciones:**
- El usuario tiene rol `"Administrador"`.

**Postcondiciones:**
- Los cambios se reflejan de forma inmediata en el catálogo público.

**Flujo principal:**
1. El administrador accede al módulo de gestión de catálogo.
2. El sistema muestra la lista de productos con opciones CRUD.
3. El administrador agrega, modifica o elimina productos/categorías.
4. El sistema valida la información y actualiza el catálogo.

---

## CU-11: Agregar Nuevas Categorías de Productos

**Actor principal:** Administrador  
**Prioridad:** Media

**Descripción:** El administrador incorpora nuevas categorías que quedan disponibles para los usuarios.

**Postcondiciones:**
- La nueva categoría se registra y se habilita en el catálogo.

---

## CU-12: Configurar Opciones de Personalización

**Actor principal:** Administrador  
**Prioridad:** Alta

**Descripción:** El administrador define los atributos de personalización: modelos de autobús, líneas/cromáticas y opciones visuales. Al agregar una nueva `Linea`, esta queda automáticamente disponible para todos los tipos de productos compatibles.

**Postcondiciones:**
- Las configuraciones se guardan y se aplican a los productos correspondientes.

---

## CU-13: Gestionar Pedidos

**Actor principal:** Administrador  
**Prioridad:** Alta

**Descripción:** El administrador consulta y gestiona todos los pedidos realizados por los usuarios.

**Flujo principal:**
1. El administrador accede a la lista de pedidos.
2. El sistema muestra información detallada de cada pedido: producto, personalización, cliente, estado.
3. El administrador puede filtrar por estado, fecha o cliente.

---

## CU-14: Enviar Pedido a Manufactura

**Actor principal:** Administrador  
**Prioridad:** Alta

**Descripción:** El administrador selecciona un pedido confirmado y lo envía al proceso de producción.

**Postcondiciones:**
- El pedido cambia de estado a `"En producción"`.
- Se genera una **lista de picking** con los detalles de manufactura (modelo, línea, rotulación, colores, cantidad).

**Requisito especial:**
- La lista de picking debe desglosar claramente todos los atributos de personalización para evitar errores humanos en el proceso de sublimación/manufactura.

---

## CU-15: Actualizar Estado del Pedido

**Actor principal:** Administrador  
**Prioridad:** Alta

**Descripción:** El administrador actualiza el estado del pedido conforme avanzan las etapas.

**Estados válidos:**
```
Recibido → En producción → Enviado → Entregado
```

**Postcondiciones:**
- El estado se refleja en el historial del cliente (CU-09).
