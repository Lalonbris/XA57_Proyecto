# EPIC-07 — Módulo React: Configurador de Productos

> → Spec: `02-functional-requirements.md` CU-04, CU-05, CU-06  
> → Spec: `06-api-contracts.md §2, §3`, DS-02, DS-03  
> → Spec: `07-business-rules.md` RN-02, RN-03, RN-04  
> → Log: Phase 2 (2026-03-16)  
> Estado: 🔄 En progreso  
> Prioridad: 🔴 Alta | Depende de: EPIC-05-05 (ConfiguradorApiController) ⏳

---

## Estructura y Build

- [x] **EPIC-07-01** — Estructura del proyecto React (`configurador-client/`)
  - → Log: Phase 2 (2026-03-16)
  - Proyecto React creado en `configurador-client/`
  - Punto de montaje configurado para leer `data-attributes` del DOM inyectados por Razor:
    ```javascript
    const root = document.getElementById('configurador-root');
    if (root) {
      ReactDOM.createRoot(root).render(<App datasetProps={root.dataset} />);
    }
    ```
  - Flujo de build documentado en Development Log

- [x] **EPIC-07-02** — Integración con `_Layout.cshtml` — badge del carrito
  - → Log: Phase 2 (2026-03-16)
  - `App.js` llama a la función global `actualizarIconoCarrito` después de agregar un producto al carrito
  - Consistencia del ID del badge verificada y corregida

---

## Llamadas a la API

- [ ] **EPIC-07-03** — Hook `useConfiguradorData` 🔴
  - → Spec: `06-api-contracts.md §2`, DS-02
  - Depende de: EPIC-05-05 (ConfiguradorApiController) ⏳
  - Ejecutar las 3 llamadas en paralelo al montar el componente:
    ```javascript
    Promise.all([
      fetch('/api/configurador/modelos').then(r => r.json()),
      fetch('/api/configurador/lineas').then(r => r.json()),
      fetch(`/api/configurador/producto/${productoId}`).then(r => r.json())
    ])
    ```
  - Manejar estado `loading` (spinner mientras cargan los datos)
  - Manejar estado `error` (mensaje visible al usuario si falla alguna llamada)

  **Criterios de aceptación:**
  - Las 3 llamadas ocurren en paralelo, no en serie
  - El formulario muestra un indicador de carga mientras se esperan los datos
  - Un fallo de red muestra mensaje de error al usuario (no falla silenciosamente)

---

## Componentes de Selección

- [ ] **EPIC-07-04** — Componente `ModeloSelector` 🔴
  - Props: `modelos`, `selectedId`, `onSelect`
  - `<select>` o grid de tarjetas con modelos disponibles
  - Etiqueta `* Obligatorio` visible (U-02, RN-02)
  - Error visual si no hay modelo seleccionado al intentar enviar

- [ ] **EPIC-07-05** — Componente `LineaSelector` 🔴
  - Props: `lineas`, `selectedLinea`, `onSelect`
  - Lista/grid con nombre de línea y swatches de `colorPrimario` + `colorSecundario`
  - Al seleccionar llama a `onSelect(linea)` con el objeto completo (se necesitan los colores para la vista previa y para el campo `ColorHex` del carrito)
  - Etiqueta `* Obligatorio` visible (U-02, RN-02)
  - La selección actualiza la vista previa en tiempo real (≤ 500 ms, U-06)

  **Nota de implementación:** el objeto `linea` completo debe pasarse al estado del configurador porque `ColorHex` se envía en el `CarritoItemDto` (Phase 2).

---

## Formulario de Personalización

- [ ] **EPIC-07-06** — Componente `PersonalizacionForm` 🔴
  - → Spec: CU-04 pasos 5–8, RN-03, RN-04
  - Props: `tipoProducto`, `valores`, `onChange`
  - Renderizado condicional según flags:
    ```jsx
    {tipoProducto?.permiteNombre && (
      <input label="Nombre del operador (opcional)"
             maxLength={tipoProducto.maxCaracteres} ... />
    )}
    {tipoProducto?.permiteNumeroEconomico && ( ... )}
    {tipoProducto?.permiteRuta && ( ... )}
    <textarea label="Notas especiales (opcional)" />
    ```
  - Contador de caracteres en tiempo real: `{n}/{maxCaracteres}` en rojo si supera el límite (RN-03)
  - Validación de caracteres no permitidos en tiempo real (RN-04)

  **Criterios de aceptación:**
  - Solo se muestran los campos habilitados por las flags del `TipoProducto`
  - El contador bloquea el envío si se supera el límite
  - Los campos opcionales están claramente diferenciados (U-02)

---

## Vista Previa

- [ ] **EPIC-07-07** — Componente `VistaPrevia` 🔴
  - → Spec: CU-05, NFR U-06, P-01
  - Props: `producto`, `modeloSeleccionado`, `lineaSeleccionada`, `personalizacion`
  - Representación visual del producto con:
    - Imagen base del producto
    - Overlay de colores según `lineaSeleccionada.colorPrimario` y `colorSecundario`
    - Texto de `nombreOperador`, `numeroEconomico`, `ruta` superpuestos si tienen valor
  - Actualización reactiva sin delay perceptible al cambiar selecciones
  - Placeholder si no hay modelo o línea seleccionada aún

  **Criterios de aceptación:**
  - Cambio entre cromáticas ≤ 2 segundos (P-01)
  - Si no hay imagen base disponible, muestra un placeholder visual

---

## Controles y Envío

- [ ] **EPIC-07-08** — Componente `CantidadControl` 🟡
  - Botones `+` / `−` y campo numérico editable
  - Valor mínimo: 1 (no permite reducir por debajo)

- [ ] **EPIC-07-09** — Utilidades de validación (`utils/validaciones.js`) 🔴
  - → Spec: RN-02, RN-03, RN-04, `06-api-contracts.md §5`
  ```javascript
  export function validarConfiguracion(config, tipoProducto) {
    const errores = {};
    // RN-02: Modelo y Línea obligatorios
    if (!config.modeloAutobusId) errores.modelo = 'El modelo es obligatorio';
    if (!config.lineaId) errores.linea = 'La línea / cromática es obligatoria';
    // RN-03: Límite de caracteres
    if (config.nombreOperador?.length > tipoProducto.maxCaracteres)
      errores.nombreOperador = `Máximo ${tipoProducto.maxCaracteres} caracteres`;
    // RN-04: Caracteres permitidos
    const regex = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]*$/;
    ['nombreOperador','numeroEconomico','ruta','notasEspeciales'].forEach(campo => {
      if (config[campo] && !regex.test(config[campo]))
        errores[campo] = 'Solo se permiten letras, números y caracteres básicos';
    });
    return errores; // {} = sin errores
  }
  ```
  - Exportar y usar en `AgregarCarritoBtn` antes del POST

- [ ] **EPIC-07-10** — Componente `AgregarCarritoBtn` 🔴
  - → Spec: CU-06, DS-03, `06-api-contracts.md §3`
  - → Log: Phase 2 (2026-03-16) — campos `Color` y `ColorHex` ya incluidos en el DTO
  - Al hacer clic:
    1. Ejecutar `validarConfiguracion(config, tipoProducto)`
    2. Si hay errores → mostrar inline, no enviar POST
    3. Si válido → `POST /Carrito/Agregar` con payload incluyendo `color` y `colorHex`:
       ```javascript
       {
         productoId,
         modeloAutobusId: config.modeloAutobusId,
         lineaId: config.lineaId,
         color: lineaSeleccionada?.nombre,        // Phase 2
         colorHex: lineaSeleccionada?.colorPrimario, // Phase 2
         nombreOperador: config.nombreOperador || null,
         numeroEconomico: config.numeroEconomico || null,
         ruta: config.ruta || null,
         notasEspeciales: config.notasEspeciales || null,
         cantidad: config.cantidad
       }
       ```
    4. En `200 OK` → llamar a `actualizarIconoCarrito()` (función global de `_Layout.cshtml`) y mostrar Toast de confirmación
    5. En error → mostrar mensaje descriptivo al usuario

  **Criterios de aceptación:**
  - El POST no se envía con errores de validación presentes
  - `actualizarIconoCarrito()` se llama tras éxito (badge actualizado)
  - Toast de confirmación visible 3–4 segundos
  - Los campos `color` y `colorHex` se envían correctamente

---

## Build e Integración con Razor

- [ ] **EPIC-07-11** — Build de producción e integración con `Detalle.cshtml` 🔴
  - → Spec: TASK-000-05
  - Configurar script de build para generar archivos en `wwwroot/configurador/`
  - En `Views/Productos/Detalle.cshtml` (`@section Scripts`):
    ```html
    <script src="~/configurador/main.js"></script>
    ```
  - Verificar que el bundle no genera conflictos con Bootstrap 5.3
  - Verificar que el componente se monta solo cuando existe `#configurador-root`

  **Criterios de aceptación:**
  - El configurador carga y funciona en la vista Detalle
  - Sin errores de consola en modo producción
  - Bundle < 500 KB sin minificar
