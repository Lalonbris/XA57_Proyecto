# Guía Paso a Paso para Explicar las Llamadas AJAX (Fetch) en el Proyecto

Este documento está diseñado para que puedas explicar cada llamada AJAX personalizada mostrando el código fuente correspondiente, su ubicación y su propósito.

---

## 📁 Estructura del Proyecto (Relevante para AJAX)

```
C:\Users\laloa\source\repos\copia_epic07\XA57_Proyecto\
│
├─ configurador-client\   # Código fuente del frontend React
│   ├─ src\
│   │   ├─ hooks\          # Custom hooks
│   │   │   └─ useConfiguradorData.js   ← Llamadas AJAX al cargar datos
│   │   └─ App.js          # Componente principal, incluye envío al carrito
│   └─ ...
└─ wwwroot\                # Archivos estáticos (bibliotecas como jQuery, no usados para AJAX personalizado)
```

---

## 🔍 Paso 1: Explicar la Carga Inicial de Datos (Hook)

### 📍 Ubicación del Código
**Archivo**: `C:\Users\laloa\source\repos\copia_epic07\XA57_Proyecto\configurador-client\src\hooks\useConfiguradorData.js`  
**Líneas clave**: 15-51 (función `fetchData` dentro del `useEffect`)

### 💬 Qué Decir al Mostrar el Código
> "Esta es la función responsable de obtener los datos iniciales que necesita el configurador: modelos de producto, líneas de ensamblaje y los detalles del producto específico. Se ejecuta cada vez que cambia el `productoId` prop, gracias al `useEffect`."

### 📌 Detalles Técnicos a Resaltar
- **Llamadas en paralelo**: Usa `Promise.all([fetch(...), fetch(...), fetch(...)])` para ejecutar las tres peticiones al mismo tiempo (líneas 21‑25), lo que mejora el rendimiento.
- **Endpoints**:
  1. `GET /api/configurador/modelos` → Obtiene lista de modelos disponibles.
  2. `GET /api/configurador/lineas` → Obtiene líneas de producción.
  3. `GET /api/configurador/producto/${productoId}` → Obtiene detalles del producto seleccionado.
- **Método HTTP**: Todas son `GET` (por defecto de `fetch`).
- **Datos enviados**: Ningos en el cuerpo; el `productoId` va en la URL.
- **Manejo de respuesta**:
  - Verifica `response.ok` (líneas 28‑30) y lanza error si falla.
  - Convierte a JSON con `.json()` (líneas 33‑35).
  - Actualiza el estado local del hook con `setModelos`, `setLineas`, `setProducto` (líneas 38‑40).
- **Estado del Hook** (líneas 8‑12):
  - `loading`: true mientras se hacen las peticiones.
  - `error`: mensaje si alguna falla.
  - `modelos`, `lineas`, `producto`: contienen los datos obtenidos.

### 🖱️ Qué Mostrar en Pantalla
1. Abre el archivo `useConfiguradorData.js`.
2. Resalta el `useEffect` (línea 14) y explica que se ejecuta al cambiar `productoId`.
3. Señala la función `fetchData` (línea 15) y el bloque `try/catch/finally`.
4. Muestra las tres llamadas `fetch` dentro de `Promise.all`.
5. Indica dónde se actualiza el estado (`setModelos`, etc.).
6. Opcional: muestra el retorno del hook (línea 54) para explicar cómo lo usa el componente.

---

## 🛒 Paso 2: Explicar el Envío al Carrito (Componente Principal)

### 📍 Ubicación del Código
**Archivo**: `C:\Users\laloa\source\repos\copia_epic07\XA57_Proyecto\configurador-client\src\App.js`  
**Función**: `handleAgregarCarrito` (líneas 74‑87)

### 💬 Qué Decir al Mostrar el Código
> "Cuando el usuario hace clic en el botón 'Agregar al carrito', se ejecuta esta función asíncrona que envía la configuración actual del producto al endpoint del carrito mediante una petición POST."

### 📌 Detalles Técnicos a Resaltar
- **Endpoint**: `POST /Carrito/Agregar`
- **Método HTTP**: `POST` (explícito en las opciones de `fetch`, línea 77).
- **Datos enviados (JSON)**:
  ```json
  {
    "productoId": "...",          // ID del producto (prop)
    "color": "...",               // Nombre del color seleccionado (state)
    "colorHex": "...",            // Valor hex del color (state)
    "numeroSerie": "...",         // Número de unidad (si aplica, state)
    "notasEspeciales": "...",     // Texto libre de notas (state)
    "cantidad": 1                 // Cantidad a agregar (state, default 1)
  }
  ```
  (Ver línea 76: `const datos = { ... }` y línea 79: `body: JSON.stringify(datos)`)
- **Encabezados**: `"Content-Type": "application/json"` (línea 78) para indicar que el cuerpo es JSON.
- **Manejo de respuesta**:
  - Si `response.ok` (línea 81): muestra toast de éxito y llama a `window.actualizarIconoCarrito()` si existe (líneas 82‑85).
  - No hay manejo explícito de errores en este fragmento (podría añadirse un `catch` si se desea).
- **Estado utilizado**: Todos los valores provienen de los `useState` del componente (`colorHex`, `colorNombre`, `numeroSerie`, `notasEspeciales`, `cantidad`, más el prop `productoId`).

### 🖱️ Qué Mostrar en Pantalla
1. Abre el archivo `App.js`.
2. Busca la función `handleAgregarCarrito` (línea 74).
3. Señala la construcción del objeto `datos` (línea 76).
4. Muestra la llamada a `fetch` con método POST, headers y body (líneas 76‑80).
5. Resalta la condición `if (response.ok)` (línea 81) y las acciones posteriores (toast y actualización del ícono).
6. Opcional: muestra dónde se definen los estados usados (líneas 40‑45) para relacionar los datos del formulario con la petición.

---

## 📝 Notas Adicionales para tu Presentación
- **Tecnología usada**: Ambas llamadas utilizan la API nativa `fetch` de JavaScript (no jQuery ni axios). Esto las hace ligeras y sin dependencias externas.
- **Manejo de errores**: 
  - En `useConfiguradorData.js` hay manejo completo con `try/catch` y estado `error`.
  - En `handleAgregarCarrito` actualmente solo se verifica `response.ok`; podrías sugerir añadir un `catch` para errores de red.
- **Estados de carga**: El hook establece `loading` y `error`; el componente puede usar estos para mostrar spinners o mensajes (aunque no se muestre en el fragmento mostrado, es buena práctica mencionarlo).
- **Endpoint base**: Todas las rutas son relativas al mismo dominio (p.ej., `/api/configurador/modelos` asume que el backend está en el mismo origen).

---

## ✅ Resumen de Pasos para tu Explicación
1. **Muestra la estructura de carpetas** para situar los archivos.
2. **Explica el Hook** (carga inicial):
   - Dónde está.
   - Qué hace (llamadas en paralelo a 3 endpoints).
   - Qué datos envía/recibe.
   - Cómo maneja estado y errores.
3. **Explica el Envío al Carrito**:
   - Dónde está.
   - Qué hace (envío POST con configuración).
   - Qué datos envía en JSON.
   - Qué hace con la respuesta.
4. **Concluye** resaltando que ambas son AJAX personalizadas usando `fetch`, sin librerías extra.

Con esta guía, podrás ir mostrando cada parte del código mientras explicas su función y ubicación. ¡Éxito en tu presentación!