# XA57 — Resumen: Orden de Ejecución y Estado

> Última actualización: 2026-03-27

---

## Árbol de Dependencias

```
TASK-000  Setup               ✅
│
├──► EPIC-01  Base de Datos   ✅
│
├──► EPIC-02  Domain Layer    ✅ (+ Color/ColorHex Phase 2, + ApplicationUser Phase 3)
│        └──► EPIC-03  Infrastructure  ✅ (+ TipoProductoRepository Phase 1)
│                  └──► EPIC-04  Application  ✅ (+ ValidationException, Color Phase 1-2)
│                            └──► EPIC-05  Controllers  🔄 (falta ConfiguradorApiController)
│                                      ├──► EPIC-06  Views Razor  🔄 (falta Detalle.cshtml)
│                                      └──► EPIC-07  React        🔄 (falta API + componentes)
│
├──► EPIC-08  Auth (Identity) ✅ (Phase 3)
│        └──► EPIC-09  Admin  ⏳
│
└──► EPIC-10  Business Rules  🔄 (validaciones de servicio hechas, RN-04/05/11 pendientes)
     EPIC-11  NFR             ⏳
```

---

## Phase 4 — En Progreso (Sprint actual)

Tareas pendientes ordenadas por dependencia:

| Orden | ID | Tarea | Prioridad | Bloqueante |
|---|---|---|---|---|
| 1 | EPIC-05-05 | `ConfiguradorApiController` | 🔴 | Desbloquea EPIC-07 completo |
| 2 | EPIC-06-05 | Vista `Productos/Detalle.cshtml` | 🔴 | Necesita EPIC-05-05 para React |
| 3 | EPIC-07-03 | Hook `useConfiguradorData` | 🔴 | Necesita EPIC-05-05 |
| 4 | EPIC-07-04 | Componente `ModeloSelector` | 🔴 | Necesita EPIC-07-03 |
| 5 | EPIC-07-05 | Componente `LineaSelector` | 🔴 | Necesita EPIC-07-03 |
| 6 | EPIC-07-06 | Componente `PersonalizacionForm` | 🔴 | Necesita flags de TipoProducto |
| 7 | EPIC-07-07 | Componente `VistaPrevia` | 🔴 | Necesita 04 y 05 |
| 8 | EPIC-07-08 | Componente `CantidadControl` | 🟡 | Independiente |
| 9 | EPIC-07-09 | `validaciones.js` | 🔴 | Necesita flags de TipoProducto |
| 10 | EPIC-07-10 | Componente `AgregarCarritoBtn` | 🔴 | Necesita 09 |
| 11 | EPIC-07-11 | Build e integración Razor | 🔴 | Necesita todos los componentes |
| 12 | EPIC-10-05 | Validación RN-04 backend (regex) | 🔴 | Independiente |
| 13 | EPIC-10-06 | Verificación RN-05 (integridad BD) | 🔴 | Independiente |

**Total estimado Phase 4: ~20h**

---

## Phase 5 — Panel de Administración (Siguiente Sprint)

| Orden | ID | Tarea | Prioridad | Estimado |
|---|---|---|---|---|
| 1 | EPIC-09-01 | Extender repositorios para admin | 🟡 | 2h |
| 2 | EPIC-09-02 | `AdminController` — Catálogo | 🟡 | 3h |
| 3 | EPIC-09-03 | `AdminController` — Pedidos y Manufactura | 🟡 | 3h |
| 4 | EPIC-09-04 | `Views/Admin/_AdminLayout.cshtml` | 🟡 | 1h |
| 5 | EPIC-09-05 | `Views/Admin/Catalogo.cshtml` | 🟡 | 2h |
| 6 | EPIC-09-06 | `Views/Admin/Pedidos.cshtml` | 🟡 | 2h |
| 7 | EPIC-09-07 | `Views/Admin/DetallePedido.cshtml` | 🟡 | 2h |
| 8 | EPIC-09-08 | `Views/Admin/ListaPicking.cshtml` | 🔴 | 2h |
| 9 | EPIC-10-07 | Validación RN-11 (secuencia estados) | 🟡 | 1h |
| 10 | EPIC-10-08 | Verificación Lista de Picking (RN-12) | 🟡 | 1h |

**Total estimado Phase 5: ~19h**

---

## Phase 6 — Calidad y Despliegue

| Orden | ID | Tarea | Prioridad | Estimado |
|---|---|---|---|---|
| 1 | EPIC-11-05 | Pruebas E2E (flujos 1–4) | 🔴 | 3h |
| 2 | EPIC-11-01 | Logging de errores | 🟡 | 2h |
| 3 | EPIC-11-02 | HTTPS en producción | 🔴 | 1h |
| 4 | EPIC-11-03 | Optimización de imágenes | 🟡 | 1h |
| 5 | EPIC-11-04 | Rendimiento de consultas (EXPLAIN) | 🟡 | 1.5h |

**Total estimado Phase 6: ~8.5h**

---

## Resumen Global

| Phase | Descripción | Estado | Estimado |
|---|---|---|---|
| Phase 1 | Service Validation | ✅ Completada (2026-03-13) | — |
| Phase 2 | Cart Functionality | ✅ Completada (2026-03-16) | — |
| Phase 3 | User Registration | ✅ Completada (2026-03-20) | — |
| Phase 4 | Catálogo y Configurador React | 🔄 En progreso | ~20h |
| Phase 5 | Panel de Administración | ⏳ Pendiente | ~19h |
| Phase 6 | Calidad y Despliegue | ⏳ Pendiente | ~8.5h |
| **Restante** | | | **~47.5h** |

---

## Decisiones Pendientes ⚠️

| Tema | Descripción | Bloqueante |
|---|---|---|
| Pago (CU-07/CU-08) | Pasarela de pago no definida; el flujo de confirmación de orden está pendiente | No — se puede entregar sistema sin pago en primera iteración |
| Vista Previa React | Requiere activos gráficos (imágenes base de los productos) del equipo de diseño | Sí — EPIC-07-07 depende de imágenes |
| Certificado SSL | Requiere definir hosting/dominio de producción | No — solo bloquea EPIC-11-02 |

---

## Definition of Done

Una tarea se considera **completada** cuando:

- [ ] El código compila sin warnings ni errores (`dotnet build` limpio)
- [ ] Los criterios de aceptación definidos en la tarea están verificados manualmente
- [ ] Ningún valor está hardcodeado si debería venir de la base de datos o configuración
- [ ] Los errores posibles están manejados (`try/catch`, `ValidationException`, `NotFound()`)
- [ ] El código está commiteado en GitHub con mensaje descriptivo en el Development Log
- [ ] La funcionalidad no rompe flujos de tareas previamente completadas
