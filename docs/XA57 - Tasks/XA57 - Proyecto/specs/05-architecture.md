# XA57 — Arquitectura del Software

> **Spec Driven Development · Documento 05**  
> Versión: 1.0 | Fecha: 2026-03-27  
> Fuente: SAD XA57 v1.0 (11/03/2026)

---

## 1. Estilo Arquitectónico

El sistema XA57 implementa una **arquitectura cliente-servidor** basada en el patrón **MVC (Modelo-Vista-Controlador)** sobre ASP.NET Core .NET 8.

El frontend es **híbrido**:
- **Razor Views** para la mayoría de páginas (server-side rendering).
- **React 19** para el módulo de personalización que requiere interactividad en tiempo real.

---

## 2. Diagrama de Capas

```
┌─────────────────────────────────────────────────────────────────┐
│                         CLIENTE (Browser)                        │
│  ┌─────────────────────────┐  ┌──────────────────────────────┐  │
│  │     Razor Views          │  │   React 19 (configurador)    │  │
│  │     (.cshtml)            │  │   configurador-client/       │  │
│  │  Server-side rendered    │  │   Fetch API → JSON           │  │
│  └──────────────┬───────────┘  └───────────────┬──────────────┘  │
└─────────────────┼─────────────────────────────┼─────────────────┘
                  │ HTTP/HTTPS                   │ HTTP/HTTPS (JSON)
┌─────────────────▼─────────────────────────────▼─────────────────┐
│                    SERVIDOR (ASP.NET Core MVC .NET 8)            │
│                                                                   │
│  ┌──────────────────────────────────────────────────────────┐    │
│  │                      CONTROLLERS                          │    │
│  │  HomeController · ProductosController                    │    │
│  │  CarritoController · ConfiguradorApiController           │    │
│  └──────────────┬────────────────────────────────────────────┘    │
│                 │                                                   │
│  ┌──────────────▼────────────────────────────────────────────┐    │
│  │                    APPLICATION / SERVICES                 │    │
│  │  IProductoService     → ProductoService                   │    │
│  │  IPedidoService       → PedidoService                     │    │
│  │  ILineaService        → LineaService                      │    │
│  │  IModeloAutobusService→ ModeloAutobusService              │    │
│  └──────────────┬────────────────────────────────────────────┘    │
│                 │                                                   │
│  ┌──────────────▼────────────────────────────────────────────┐    │
│  │                    DOMAIN / ENTITIES                      │    │
│  │  Producto · TipoProducto · ModeloAutobus · Linea          │    │
│  │  Pedido · Carrito · Orden · Usuario                       │    │
│  │  (Sin dependencias externas)                              │    │
│  └──────────────┬────────────────────────────────────────────┘    │
│                 │                                                   │
│  ┌──────────────▼────────────────────────────────────────────┐    │
│  │                 INFRASTRUCTURE / REPOSITORIES             │    │
│  │  IProductoRepository    → ProductoRepository              │    │
│  │  IPedidoRepository      → PedidoRepository                │    │
│  │  ILineaRepository       → LineaRepository                 │    │
│  │  IModeloAutobusRepository→ ModeloAutobusRepository        │    │
│  │                                                            │    │
│  │  AppDbContext (EF Core 8 + Npgsql)                        │    │
│  └──────────────┬────────────────────────────────────────────┘    │
└─────────────────┼─────────────────────────────────────────────────┘
                  │ TCP/IP
┌─────────────────▼──────────────────────┐
│         BASE DE DATOS (PostgreSQL 16)  │
│  Servidor: IIS / Hosting propio        │
│  Protocolo: HTTPS                      │
└────────────────────────────────────────┘
```

---

## 3. Estructura de Carpetas del Proyecto

```
XA57/
├── Domain/
│   ├── Entities/
│   │   ├── Producto.cs
│   │   ├── TipoProducto.cs
│   │   ├── ModeloAutobus.cs
│   │   ├── Linea.cs
│   │   ├── Pedido.cs
│   │   ├── Carrito.cs
│   │   ├── ItemCarrito.cs
│   │   ├── Orden.cs
│   │   ├── ItemOrden.cs
│   │   └── Usuario.cs
│   └── Interfaces/
│       ├── IProductoRepository.cs
│       ├── IPedidoRepository.cs
│       ├── ILineaRepository.cs
│       └── IModeloAutobusRepository.cs
│
├── Application/
│   ├── DTOs/
│   │   ├── CarritoItemDto.cs
│   │   └── PedidoResultDto.cs
│   ├── Interfaces/
│   │   ├── IProductoService.cs
│   │   ├── IPedidoService.cs
│   │   ├── ILineaService.cs
│   │   └── IModeloAutobusService.cs
│   └── Services/
│       ├── ProductoService.cs
│       ├── PedidoService.cs
│       ├── LineaService.cs
│       └── ModeloAutobusService.cs
│
├── Infrastructure/
│   ├── Data/
│   │   └── AppDbContext.cs
│   └── Repositories/
│       ├── ProductoRepository.cs
│       ├── PedidoRepository.cs
│       ├── LineaRepository.cs
│       └── ModeloAutobusRepository.cs
│
├── Controllers/
│   ├── HomeController.cs
│   ├── ProductosController.cs
│   ├── CarritoController.cs
│   └── ConfiguradorApiController.cs
│
├── ViewModels/
│   └── CarritoViewModel.cs
│
├── Views/
│   ├── Home/
│   │   └── Catalogo.cshtml
│   ├── Productos/
│   │   └── Detalle.cshtml
│   └── Carrito/
│       └── Index.cshtml
│
├── configurador-client/         ← Módulo React 19
│   ├── src/
│   │   ├── components/
│   │   └── App.jsx
│   └── package.json
│
└── Migrations/                  ← EF Core migrations
```

---

## 4. Descripción de Capas

### 4.1 Domain (Entidades)

Contiene las entidades puras del negocio y las interfaces de repositorios. **No tiene dependencias externas** (ni EF Core, ni ASP.NET, ni ningún framework).

- Define el contrato de acceso a datos mediante interfaces (`IProductoRepository`, etc.).
- Las entidades son POCOs (Plain Old C# Objects).

### 4.2 Application (Servicios)

Implementa la lógica de negocio y orquesta los casos de uso. Depende únicamente de `Domain`.

- Los servicios implementan las interfaces de `Application/Interfaces`.
- Consumen repositorios a través de las interfaces de `Domain/Interfaces` (inversión de dependencias).
- Maneja DTOs para transferencia de datos entre capas.

### 4.3 Infrastructure (Repositorios y DbContext)

Implementación concreta del acceso a datos. Depende de `Domain` y de EF Core / Npgsql.

- `AppDbContext` extiende `DbContext` de EF Core.
- Los repositorios implementan las interfaces de `Domain/Interfaces`.
- Las migraciones de EF Core se generan aquí.

### 4.4 Controllers (Presentación)

Recibe peticiones HTTP, coordina el flujo y retorna vistas o JSON. Depende de `Application/Interfaces`.

**Controladores del sistema:**

| Controlador | Rutas | Responsabilidad |
|---|---|---|
| `HomeController` | `GET /`, `GET /Home/Catalogo` | Página principal, grid de catálogo |
| `ProductosController` | `GET /Productos/Detalle/{id}` | Detalle de producto + montar React |
| `CarritoController` | `GET /Carrito`, `POST /Carrito/Agregar`, `POST /Carrito/Eliminar/{id}`, `GET /Carrito/Cantidad` | Gestión del carrito |
| `ConfiguradorApiController` | `GET /api/configurador/modelos`, `GET /api/configurador/lineas`, `GET /api/configurador/producto/{id}` | API JSON para el componente React |

### 4.5 ViewModels

Clases que adaptan los datos del dominio para las vistas Razor. Evitan exponer entidades directamente.

```csharp
public class CarritoViewModel
{
    public List<Pedido> Items { get; set; }
    public decimal Total { get; set; }
}
```

### 4.6 configurador-client (React 19)

Módulo SPA parcial que maneja el flujo de personalización interactiva. Se monta en la vista `Detalle.cshtml` a través de `data-attributes`.

**Responsabilidades:**
- Cargar modelos, líneas y flags de TipoProducto vía Fetch API.
- Renderizar la vista previa reactiva del producto.
- Validar la configuración localmente.
- Enviar `POST /Carrito/Agregar` con el payload serializado.
- Actualizar el badge del carrito tras agregar un ítem.

---

## 5. Diagrama de Componentes

```
┌──────────────────────────────────────────────────────────────┐
│ Frontend                                                      │
│  ┌─────────────────────┐   ┌────────────────────────────┐   │
│  │   React 19          │   │  Razor Views (.cshtml)      │   │
│  │  (configurador)     │   │  + Bootstrap 5.3            │   │
│  └──────────┬──────────┘   └──────────────┬─────────────┘   │
└─────────────┼──────────────────────────────┼─────────────────┘
              │ Fetch API · JSON              │ HTTP
┌─────────────▼──────────────────────────────▼─────────────────┐
│ Backend                                                        │
│  ┌────────────────────────────────────────────────────────┐   │
│  │         Entity Framework Core (ORM · Npgsql)           │   │
│  └─────────────────────────┬──────────────────────────────┘   │
└────────────────────────────┼────────────────────────────────── ┘
                             │
┌────────────────────────────▼────────────────────────────────┐
│ Base de Datos                                                 │
│  <<service>> PostgreSQL 16                                    │
└──────────────────────────────────────────────────────────────┘
```

---

## 6. Decisiones de Arquitectura

| Decisión | Opción elegida | Justificación |
|---|---|---|
| **Patrón de arquitectura** | MVC + Capas (Domain/Application/Infrastructure) | Separación de responsabilidades, mantenibilidad, pruebas |
| **Renderizado frontend** | Híbrido: Razor + React parcial | Eficiencia en páginas simples; interactividad solo donde se necesita |
| **ORM** | Entity Framework Core 8 (Code First) | Tipado fuerte, migraciones automáticas, integración nativa con .NET |
| **Base de datos** | PostgreSQL 16 | Robustez, integridad referencial, soporte a tipos JSON si se necesita |
| **Inversión de dependencias** | Interfaces en Domain, implementaciones en Infrastructure | Facilita pruebas unitarias y futuros cambios de proveedor de datos |
| **Módulo React** | React 19 montado por Razor vía data-attributes | Aislamiento del módulo interactivo sin necesidad de SPA completa |
| **Control de versiones** | Git + GitHub, rama principal | Colaboración del equipo, historial de cambios |

---

## 7. Ambiente de Despliegue

```
[Cliente - Browser]
  ↓ HTTPS
[Servidor Web - Microsoft IIS]
  └── [.NET 8 Runtime]
       └── [ASP.NET Core MVC App - XA57]
            └── [Npgsql Connection]
                 ↓ TCP/IP
            [PostgreSQL 16 Server]
```

| Nodo | Tecnología | Notas |
|---|---|---|
| Servidor Web | Microsoft IIS | Hospeda la aplicación .NET 8 |
| Runtime | .NET 8 Runtime | Requerido en el servidor |
| Base de datos | PostgreSQL 16 | Puede estar en el mismo servidor o separado |
| Protocolo cliente | HTTPS | Obligatorio en producción |
| Node.js (build) | 20.x | Requerido solo en entorno de desarrollo/CI para compilar React |
