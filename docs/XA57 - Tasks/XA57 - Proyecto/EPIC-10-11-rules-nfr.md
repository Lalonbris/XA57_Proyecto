# EPIC-10 — Validaciones y Reglas de Negocio

> → Spec: `07-business-rules.md`, `03-non-functional-requirements.md`  
> → Log: Phase 1 (2026-03-13) — validaciones de servicio implementadas  
> Estado: 🔄 Parcialmente completada  
> Prioridad: 🔴 Alta | Transversal

---

## Completadas en Phase 1

- [x] **EPIC-10-01** — `ValidationException` custom
  - → Log: Phase 1 (2026-03-13)
  - `Application/Exceptions/ValidationException.cs` creada
  - Usada por `PedidoService` y `ProductoService`

- [x] **EPIC-10-02** — Validación RN-02 en `PedidoService` (Cantidad > 0)
  - → Log: Phase 1 (2026-03-13)
  - `Cantidad > 0` validada, lanza `ValidationException`

- [x] **EPIC-10-03** — Validación RN-02 en `PedidoService` (ProductoId activo)
  - → Log: Phase 1 (2026-03-13)
  - `ProductoId` debe existir y `Activo = true`; lanza `ValidationException`

- [x] **EPIC-10-04** — Validación RN-03 en `PedidoService` (límite de caracteres)
  - → Log: Phase 1 (2026-03-13)
  - `NombreOperador`, `NumeroEconomico`, `Ruta` validados contra `TipoProducto.MaxCaracteres`
  - `ITipoProductoRepository` inyectado para leer `MaxCaracteres`

---

## Pendientes

- [ ] **EPIC-10-05** — Validación RN-04 en el backend (caracteres permitidos) 🔴
  - → Spec: RN-04 — sin caracteres especiales no imprimibles
  - Agregar en `PedidoService.AgregarAsync()` para cada campo de texto:
    ```csharp
    private static readonly Regex _regexPermitidos =
        new(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑüÜ\s\-\.\,\/\#]*$");

    private void ValidarCaracteres(string? valor, string campo)
    {
        if (!string.IsNullOrEmpty(valor) && !_regexPermitidos.IsMatch(valor))
            throw new ValidationException($"{campo} contiene caracteres no permitidos.");
    }
    ```
  - Llamar para: `NombreOperador`, `NumeroEconomico`, `Ruta`, `NotasEspeciales`
  - Capturado en `CarritoController.Agregar()` → `400 Bad Request` con mensaje

  **Criterios de aceptación:**
  - POST con caracteres no imprimibles en `NombreOperador` → `400 Bad Request`
  - Mensaje de error descriptivo indica el campo afectado

- [ ] **EPIC-10-06** — Validación RN-05 (integridad de texto en BD) 🔴
  - → Spec: RN-05 — sin truncamiento
  - Verificar que las columnas SQL tienen longitud ≥ `MaxCaracteres` máximo posible:
    - `NombreOperador VARCHAR(100)` ≥ 30 chars máximo ✅ (margen suficiente)
    - `NumeroEconomico VARCHAR(20)` — verificar contra límites del `TipoProducto`
    - `Ruta VARCHAR(150)` — verificar contra límites del `TipoProducto`
  - Crear migración si alguna columna es insuficiente

  **Criterios de aceptación:**
  - Prueba con texto de exactamente `MaxCaracteres` caracteres → se guarda completo, sin truncamiento

- [ ] **EPIC-10-07** — Validación RN-11 en `AdminController` (secuencia de estados) 🟡
  - → Spec: RN-11 — flujo unidireccional `Recibido → En producción → Enviado → Entregado`
  - Implementar `EsTransicionValida(string actual, string nuevo)` en `AdminController` o en un servicio dedicado
  - `ActualizarEstado()` llama al validador antes de persistir
  - Si inválida → `BadRequest` con mensaje "Transición de estado no permitida"

  **Criterios de aceptación:**
  - `Recibido → Entregado` directo → rechazado
  - `Enviado → En producción` (retroceso) → rechazado
  - `Recibido → En producción` → aceptado

- [ ] **EPIC-10-08** — Verificación de Lista de Picking (RN-12) 🟡
  - → Spec: RN-12
  - Con un pedido real de prueba que tenga todos los campos completados:
    - Verificar que `ListaPicking.cshtml` muestra cada campo sin truncamiento
    - Verificar que `Color` y `ColorHex` (Phase 2) aparecen en la lista de picking
  - Checklist de campos mínimos requeridos:
    - [ ] Producto (nombre y tipo)
    - [ ] Modelo de autobús (nombre y fabricante)
    - [ ] Línea / Cromática (nombre + hex)
    - [ ] Color seleccionado (`Pedido.Color`, `Pedido.ColorHex`)
    - [ ] Nombre del operador
    - [ ] Número económico
    - [ ] Ruta / destino
    - [ ] Notas especiales
    - [ ] Cantidad
    - [ ] Fecha del pedido

---
---

# EPIC-11 — Requisitos No Funcionales

> → Spec: `03-non-functional-requirements.md`  
> Estado: ⏳ Pendiente  
> Prioridad: 🟡 Media (algunas 🔴)

---

- [ ] **EPIC-11-01** — Logging de errores (F-04) 🟡
  - → Spec: NFR F-04
  - Agregar `try/catch` con `_logger.LogError(ex, ...)` en los métodos de servicio que pueden fallar
  - Opción A (simple): logging a archivo con `appsettings.json`
  - Opción B (recomendada): Serilog + tabla `Logs` en PostgreSQL
  - Los errores de transacción deben quedar registrados con timestamp, mensaje y stack trace
  - Los logs no deben exponer contraseñas ni datos sensibles

  **Criterios de aceptación:**
  - Un error forzado en `PedidoService` queda registrado automáticamente
  - La aplicación no expone el stack trace al usuario final

- [ ] **EPIC-11-02** — HTTPS en producción (F-05) 🔴
  - Agregar `app.UseHttpsRedirection()` en `Program.cs`
  - Certificado SSL configurado en IIS o Let's Encrypt
  - Cookies de sesión de Identity con flag `Secure = true` en producción

  **Criterios de aceptación:**
  - Acceso por HTTP redirige automáticamente a HTTPS
  - Las cookies tienen flag `Secure` en el header `Set-Cookie`

- [ ] **EPIC-11-03** — Optimización de imágenes (P-03) 🟡
  - Atributo `loading="lazy"` en todas las imágenes del catálogo *(verificar que está en `Catalogo.cshtml`)*
  - Definir dimensiones fijas en tarjetas para evitar layout shift (CLS)
  - Formato de imagen recomendado: WebP, máximo 800×600 px para catálogo
  - Verificar carga del grid con 6 productos en ≤ 2 segundos (P-02)

- [ ] **EPIC-11-04** — Verificación de rendimiento de consultas (P-04) 🟡
  - Ejecutar `EXPLAIN ANALYZE` en las consultas principales:
    - `ObtenerTodosAsync()` del catálogo
    - `ObtenerConProductosAsync()` del carrito
    - `ObtenerActivasAsync()` de líneas
  - Verificar que cada consulta usa índice (no `Seq Scan` en tablas con datos)
  - Si hay `Seq Scan` → crear índice y nueva migración EF Core

- [ ] **EPIC-11-05** — Pruebas de flujo completo (End-to-End) 🔴

  **Flujo 1 — Compra exitosa:**
  - [ ] Registrar usuario nuevo
  - [ ] Hacer login
  - [ ] Explorar catálogo, verificar productos activos
  - [ ] Seleccionar producto y abrir vista de detalle
  - [ ] Configurar: modelo + línea + nombre de operador + número económico
  - [ ] Agregar al carrito, verificar toast de confirmación y badge actualizado
  - [ ] Ir al carrito, verificar que aparece el ítem con todos los atributos (incluyendo `Color` y `ColorHex`)
  - [ ] Eliminar el ítem, verificar que el carrito queda vacío

  **Flujo 2 — Validaciones frontend:**
  - [ ] Intentar agregar sin modelo → error visual
  - [ ] Intentar agregar sin línea → error visual
  - [ ] Ingresar texto que supera `MaxCaracteres` → contador en rojo, envío bloqueado
  - [ ] Ingresar caracteres especiales → error visual inmediato

  **Flujo 3 — Validaciones backend:**
  - [ ] POST con `cantidad = 0` → `400 Bad Request`
  - [ ] POST con `productoId` inactivo → `400 Bad Request`
  - [ ] POST con texto que supera `MaxCaracteres` → `400 Bad Request`

  **Flujo 4 — Administración:**
  - [ ] Login como administrador
  - [ ] Ver lista de pedidos
  - [ ] Enviar a manufactura un pedido en estado `Recibido`
  - [ ] Ver Lista de Picking con todos los atributos completos
  - [ ] Actualizar estado a `Enviado`
  - [ ] Verificar que estado `Enviado → Recibido` (retroceso) es rechazado

  **Criterios de aceptación:**
  - Todos los flujos completan sin errores de consola ni excepciones no manejadas
  - Los datos de personalización se muestran íntegros en todos los pasos (RN-05)
