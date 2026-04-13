# XA57 — Project Overview

> **Spec Driven Development · Documento de Referencia**  
> Versión: 1.0 | Fecha: 2026-03-27 | Metodología: Proceso Unificado (RUP)

---

## 1. Descripción del Sistema

**XA57** es una plataforma de comercio electrónico especializada en la venta de coleccionables de transporte con capacidades avanzadas de personalización. Permite a cualquier usuario explorar un catálogo abierto de productos (busitos de peluche, llaveros, cintas, almohadas temáticas) y configurarlos con atributos específicos: modelo de autobús, línea/cromática, nombre del operador, número económico y ruta.

El sistema opera bajo **manufactura bajo demanda**: cada pedido genera especificaciones detalladas de producción enviadas al equipo de manufactura.

---

## 2. Stakeholders

| Stakeholder | Rol | Necesidades principales |
|---|---|---|
| **Cliente / Usuario Final** | Coleccionistas y entusiastas del transporte | Catálogo completo, configurador visual, seguimiento de pedido |
| **Equipo de Manufactura** | Personal de producción | Especificaciones claras con mockups y atributos estructurados |
| **Administrador** | Gestión del negocio | CRUD de catálogo, órdenes, opciones de personalización |
| **Equipo de Desarrollo** | Implementación técnica | Arquitectura extensible, documentación de interfaces |

---

## 3. Stack Tecnológico

| Capa | Tecnología | Versión |
|---|---|---|
| Backend | ASP.NET Core MVC | .NET 8.0 LTS |
| ORM | Entity Framework Core + Npgsql | 8.x |
| Frontend (vistas servidor) | Razor Views (.cshtml) | ASP.NET Core 8 |
| Frontend (módulo interactivo) | React | 19.x |
| Estilos | Bootstrap + CSS3 personalizado | Bootstrap 5.3 |
| Base de datos | PostgreSQL | 16.x |
| IDE | Visual Studio 2022 | 17.x |
| Control de versiones | Git + GitHub | 2.x |
| Lenguajes | C#, HTML5, CSS3, JavaScript ES6+ | — |

---

## 4. Arquitectura de Alto Nivel

El sistema sigue el patrón **MVC sobre cliente-servidor**:

```
[Navegador / Cliente]
    ├── Razor Views (.cshtml)   → renderizado server-side
    └── React 19               → módulo de personalización (SPA parcial)
         └── Fetch API → JSON

[Servidor ASP.NET MVC .NET 8]
    ├── Controllers            → reciben HTTP, coordinan flujo
    ├── ViewModels             → adaptan dominio a vistas
    ├── Application/Services   → lógica de negocio y casos de uso
    ├── Domain/Entities        → entidades puras sin dependencias
    └── Infrastructure
         ├── Repositories      → implementación concreta de acceso a datos
         └── AppDbContext      → EF Core DbContext

[PostgreSQL 16]               → almacenamiento relacional
```

### Capas del proyecto (solución Visual Studio)

| Carpeta | Responsabilidad |
|---|---|
| `Domain/` | Entidades del negocio e interfaces de repositorios. Sin dependencias externas. |
| `Application/` | Servicios y lógica de negocio. Orquesta casos de uso. |
| `Infrastructure/` | DbContext, repositorios, conexión a PostgreSQL con EF Core. |
| `ViewModels/` | Clases que adaptan datos de dominio para las vistas. |
| `Controllers/` | Recibe peticiones HTTP, llama a Application, retorna View o JSON. |
| `configurador-client/` | Módulo React 19 para la personalización de productos. |
| `Views/` | Archivos `.cshtml` de Razor para renderizado HTML. |
| `Migrations/` | Historial de cambios del esquema generado por EF Core. |

---

## 5. Atributos de Calidad

| Atributo | Prioridad | Estrategia |
|---|---|---|
| **Escalabilidad** | Alta | Arquitectura en capas, PostgreSQL, módulos separados |
| **Flexibilidad** | Alta | Modelos de datos reutilizables (líneas aplican a todos los productos) |
| **Usabilidad** | Alta | Flujo guiado, imágenes de alta calidad, React para interactividad |
| **Rendimiento** | Alta | Optimización de consultas, carga eficiente de imágenes (<2 s) |
| **Integridad** | Alta | PostgreSQL con FK, validación en backend, transacciones EF Core |
| **Confiabilidad de datos** | Alta | Backups periódicos, manejo de transacciones |
| **Disponibilidad** | Media | Servidores confiables, logging, respaldos |
| **Accesibilidad** | Media | Etiquetas adecuadas, navegación sencilla |
| **Vista / Presentación** | Media | React + imágenes HD para decisiones de compra |
| **Multi-idioma** | Baja | Diseño preparado para futura i18n |

---

## 6. Ambiente de Producción

| Componente | Tecnología |
|---|---|
| Servidor Web | Microsoft IIS |
| Runtime | .NET 8 Runtime |
| Base de datos | PostgreSQL 16 |
| Protocolo | HTTPS |

---

## 7. Índice de Documentos SDD

| Archivo | Contenido |
|---|---|
| `00-project-overview.md` | Este documento — contexto general |
| `01-domain-model.md` | Entidades, relaciones conceptuales, glosario |
| `02-functional-requirements.md` | Casos de uso (CU-01 a CU-15) como especificaciones |
| `03-non-functional-requirements.md` | Requisitos FURPS+, reglas de dominio |
| `04-data-model.md` | Esquema ER, definiciones de tablas, restricciones |
| `05-architecture.md` | Capas, componentes, decisiones de diseño |
| `06-api-contracts.md` | Contratos REST derivados de los diagramas de secuencia |
| `07-business-rules.md` | Reglas de negocio RN-01 a RN-0X |
