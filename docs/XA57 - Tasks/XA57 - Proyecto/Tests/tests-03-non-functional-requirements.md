# XA57 — Requisitos No Funcionales

> **Spec Driven Development · Documento 03**  
> Versión: 1.0 | Fecha: 2026-03-27  
> Fuente: Documento de Especificación Complementaria XA57 v1.0 (Borrador 29/01/2026)

---

## Marco FURPS+

Este documento organiza los requisitos no funcionales usando el marco **FURPS+** (Funcionalidad transversal, Usabilidad, Fiabilidad, Rendimiento, Soporte, más restricciones).

---

## 1. Funcionalidad Transversal

### 1.1 Configuración y Personalización de Producto

| ID | Requisito | Obligatoriedad |
|---|---|---|
| F-01 | El sistema debe permitir seleccionar primero la "Base del Producto" (modelo de autobús, ej. Irizar i8, Volvo 9800) y la "Línea/Cromática" de un catálogo abierto antes de habilitar campos opcionales. | Obligatorio |
| F-02 | Una vez seleccionados modelo y línea, el sistema debe habilitar campos opcionales: Nombre del Operador / Texto personalizado, Número Económico / Serial de la unidad, Ruta o destino (si aplica al tipo de producto). | Obligatorio |
| F-03 | Los datos de personalización deben persistir vinculados al ítem en el carrito. Si el usuario sale de la página y regresa, su configuración (ej. "Número 105") debe mantenerse. | Obligatorio |

### 1.2 Registro y Gestión de Errores (Logging)

| ID | Requisito | Obligatoriedad |
|---|---|---|
| F-04 | El sistema debe registrar errores de transacción y fallos en el servidor en una base de datos de logs para auditoría técnica. | Obligatorio |

### 1.3 Seguridad

| ID | Requisito | Obligatoriedad |
|---|---|---|
| F-05 | Aunque el catálogo es público (sin autenticación), los datos del cliente (dirección, historial de pedidos personalizados) deben estar protegidos y encriptados. | Obligatorio |
| F-06 | Las contraseñas se almacenan exclusivamente como hash; nunca en texto plano. | Obligatorio |

---

## 2. Usabilidad

### 2.1 Experiencia de Personalización

| ID | Requisito | Métrica |
|---|---|---|
| U-01 | El flujo de compra debe ser: **Seleccionar Producto → Elegir Cromática → Personalizar Datos → Agregar al Carrito**. Sin pasos adicionales obligatorios. | Revisión de UX |
| U-02 | El sistema debe indicar claramente qué campos son obligatorios y cuáles son opcionales. El modelo es siempre obligatorio; el nombre del operador es siempre opcional. | Revisión de UI |
| U-03 | Los errores de validación deben mostrarse de forma inmediata (inline), sin necesidad de enviar el formulario. | Revisión de UI |

### 2.2 Diseño Visual (Catálogo)

| ID | Requisito | Métrica |
|---|---|---|
| U-04 | El catálogo debe priorizar imágenes de alta calidad de las cromáticas y modelos. | Resolución mínima de imágenes definida en etapa de diseño gráfico |
| U-05 | La interfaz debe permitir hacer zoom o ver detalles del diseño del autobús/producto. | Funcionalidad de zoom disponible |
| U-06 | La vista previa del configurador debe actualizarse reactivamente al cambiar modelo, línea o textos (sin recarga de página). | Tiempo de respuesta < 500 ms para actualización local |

---

## 3. Fiabilidad (Reliability)

### 3.1 Exactitud de la Orden

| ID | Requisito | Métrica |
|---|---|---|
| R-01 | El sistema debe garantizar que la información de personalización viaje íntegra desde el carrito hasta la Orden de Compra final. No puede haber truncamiento de texto (si el usuario escribe "Expreso Futura", debe guardarse completo). | 0% de truncamientos en pruebas de integración |
| R-02 | El sistema debe prevenir el uso de caracteres especiales no imprimibles que puedan causar errores en el proceso de manufactura/sublimación. | Validación activa en frontend y backend |
| R-03 | El sistema debe garantizar integridad referencial entre Pedidos, Productos, Modelos y Líneas mediante restricciones de FK en PostgreSQL. | Validado por EF Core migrations |

---

## 4. Rendimiento (Performance)

| ID | Requisito | Métrica |
|---|---|---|
| P-01 | Navegar entre diferentes cromáticas en el configurador no debe tomar más de **2 segundos**. | Medido con herramientas de performance en navegador |
| P-02 | El catálogo de productos debe cargar el grid inicial en menos de **2 segundos** en condiciones normales de red. | Lighthouse / Web Vitals |
| P-03 | El sistema debe optimizar la carga de imágenes considerando la multiplicidad de variantes (Líneas × Modelos × Productos). | Lazy loading, formatos optimizados (WebP), CDN si aplica |
| P-04 | Las consultas a la base de datos deben estar optimizadas con índices en columnas de FK y columnas frecuentemente filtradas (`activo`, `estado`). | EXPLAIN ANALYZE en PostgreSQL |

---

## 5. Soporte (Supportability)

### 5.1 Escalabilidad de Líneas y Productos

| ID | Requisito | Implicación de diseño |
|---|---|---|
| S-01 | Agregar una nueva "Línea de Autobús" debe hacerla disponible automáticamente para todos los tipos de productos compatibles (Busito, Almohada, Llavero) sin configurar producto por producto. | La relación Línea–Producto es muchos-a-muchos por tipo, no por instancia |
| S-02 | La base de datos debe soportar la adición de nuevos tipos de coleccionables (ej. tazas, gorras) reutilizando las líneas y modelos ya cargados. | Modelo de datos genérico; `TipoProducto` controla las flags |
| S-03 | El sistema debe diseñarse para que agregar nuevas categorías no requiera cambios en la arquitectura core. | Módulos desacoplados, CRUD administrativo |

---

## 6. Restricciones de Implementación

| ID | Restricción | Valor |
|---|---|---|
| C-01 | **Plataforma objetivo** | Aplicación Web (navegador moderno compatible con HTML5, CSS3, JavaScript ES6+) |
| C-02 | **Base de datos** | Relacional (SQL) — PostgreSQL 16 obligatorio para gestionar integridad entre Productos, Líneas y Personalizaciones |
| C-03 | **Framework backend** | ASP.NET Core MVC sobre .NET 8 LTS |
| C-04 | **ORM** | Entity Framework Core 8 con enfoque Code First |
| C-05 | **Frontend interactivo** | React 19 para el módulo de personalización |
| C-06 | **Control de versiones** | Git + GitHub, rama principal |
| C-07 | **Idioma inicial** | Español (interfaz). Diseño preparado para futura internacionalización |

---

## 7. Interfaces Externas

| ID | Interfaz | Descripción |
|---|---|---|
| I-01 | **Salida de Producción (Lista de Picking)** | El sistema debe generar una vista/reporte para el administrador que desglose claramente los detalles de personalización de cada pedido: producto, modelo, línea, colores, rotulación, cantidad. Objetivo: eliminar errores humanos en manufactura/sublimación. |
| I-02 | **Pasarela de Pago** | Integración con servicio externo de procesamiento de pagos (especificación pendiente). |

---

## 8. Restricciones de Calidad de Datos

| ID | Restricción | Implementación |
|---|---|---|
| D-01 | Textos de personalización (Nombre, Ruta, Número Económico) deben almacenarse sin truncamiento | Longitudes de campo definidas por `TipoProducto.MaxCaracteres` |
| D-02 | No se permiten caracteres no imprimibles en campos de personalización | Validación regex en React y en el backend (controller/service) |
| D-03 | El email de usuario debe ser único en el sistema | Restricción `UNIQUE` en tabla `Usuarios` |
| D-04 | Un producto no puede venderse sin modelo y línea asignados | Validación en `PedidoService` y regla de negocio RN-02 |
