# XA57 — Modelo de Datos

> **Spec Driven Development · Documento 04**  
> Versión: 1.0 | Fecha: 2026-03-27  
> Motor: PostgreSQL 16 | ORM: Entity Framework Core 8 (Code First)

---

## 1. Diagrama ER (Descripción Textual)

```
┌──────────────┐     ┌───────────────────────┐     ┌─────────────────┐
│   Usuarios   │     │       Productos        │     │   Categorias    │
│──────────────│     │───────────────────────│     │─────────────────│
│ Id (PK)      │     │ Id (PK)               │     │ Id (PK)         │
│ Nombre       │     │ Nombre                │◄────│ Nombre          │
│ Apellido     │     │ Descripcion           │     │ Descripcion     │
│ Email UNIQUE │     │ Precio                │     └─────────────────┘
│ PasswordHash │     │ Stock                 │
│ Rol          │     │ ImagenUrl             │     ┌─────────────────────────┐
│ FechaRegistro│     │ EsPersonalizable      │     │  PersonalizacionesProducto│
└──────┬───────┘     │ CategoriaId (FK) ─────┼────►│─────────────────────────│
       │             │ TipoProductoId (FK)   │     │ Id (PK)                 │
       │             │ Activo                │     │ ProductoId (FK)         │
       │             └───────────┬───────────┘     │ TipoOpcion              │
       │                         │                  │ ValorOpcion             │
       │             ┌───────────▼───────────┐     │ PrecioExtra             │
       │             │     TiposProducto     │     └─────────────────────────┘
       │             │───────────────────────│
       │             │ Id (PK)               │
       │             │ Nombre                │
       │             │ MaxCaracteres         │
       │             │ PermiteNombre         │
       │             │ PermiteNumeroEconomico│
       │             │ PermiteRuta           │
       │             └───────────────────────┘
       │
  ┌────▼──────┐     ┌─────────────────────┐
  │ Carritos  │     │    ItemsCarrito      │
  │───────────│     │─────────────────────│
  │ Id (PK)   ├────►│ Id (PK)             │
  │ UsuarioId │     │ CarritoId (FK)      │
  │ FechaCreac│     │ ProductoId (FK)     │
  └───────────┘     │ Cantidad            │
                    │ PrecioUnitario      │
  ┌────────────┐    └─────────────────────┘
  │  Ordenes   │
  │────────────│    ┌─────────────────────┐
  │ Id (PK)    │    │     ItemsOrden       │
  │ UsuarioId  ├───►│─────────────────────│
  │ FechaOrden │    │ Id (PK)             │
  │ Estado     │    │ OrdenId (FK)        │
  │ Total      │    │ ProductoId (FK)     │
  │ DirEnvio   │    │ PersonalizacionId(FK│
  └────────────┘    │ Cantidad            │
                    │ PrecioUnitario      │
                    └─────────────────────┘

  ┌──────────────────┐    ┌──────────────┐    ┌──────────────┐
  │   Pedidos        │    │ModelosAutobus│    │    Lineas    │
  │──────────────────│    │──────────────│    │──────────────│
  │ Id (PK)          │    │ Id (PK)      │    │ Id (PK)      │
  │ ProductoId (FK)  │    │ Nombre       │    │ Nombre       │
  │ ModeloAutobusId  ├───►│ Fabricante   │    │ ColorPrimario│
  │ LineaId (FK)     ├────┤              │    │ ColorSecund. │
  │ NombreOperador   │    │ Activo       │◄───┤ NombreOper.  │
  │ NumeroEconomico  │    └──────────────┘    │ LogoUrl      │
  │ Ruta             │                        │ Activa       │
  │ NotasEspeciales  │                        └──────────────┘
  │ Cantidad         │
  │ Estado           │
  │ FechaCreacion    │
  └──────────────────┘
```

---

## 2. Definición de Tablas

### `Usuarios`

```sql
CREATE TABLE "Usuarios" (
    "Id"              SERIAL          PRIMARY KEY,
    "Nombre"          VARCHAR(100)    NOT NULL,
    "Apellido"        VARCHAR(100)    NOT NULL,
    "Email"           VARCHAR(150)    NOT NULL UNIQUE,
    "PasswordHash"    TEXT            NOT NULL,
    "Rol"             VARCHAR(20)     NOT NULL DEFAULT 'Cliente',
    "FechaRegistro"   TIMESTAMP       NOT NULL DEFAULT NOW()
);

-- Índice para login
CREATE INDEX idx_usuarios_email ON "Usuarios" ("Email");
```

---

### `Categorias`

```sql
CREATE TABLE "Categorias" (
    "Id"          SERIAL          PRIMARY KEY,
    "Nombre"      VARCHAR(100)    NOT NULL,
    "Descripcion" TEXT
);
```

---

### `TiposProducto`

```sql
CREATE TABLE "TiposProducto" (
    "Id"                       SERIAL          PRIMARY KEY,
    "Nombre"                   VARCHAR(100)    NOT NULL,
    "MaxCaracteres"            INT             NOT NULL DEFAULT 30,
    "PermiteNombre"            BOOLEAN         NOT NULL DEFAULT FALSE,
    "PermiteNumeroEconomico"   BOOLEAN         NOT NULL DEFAULT FALSE,
    "PermiteRuta"              BOOLEAN         NOT NULL DEFAULT FALSE
);
```

**Datos de ejemplo:**

| Nombre | MaxCaracteres | PermiteNombre | PermiteNumeroEconomico | PermiteRuta |
|---|---|---|---|---|
| Busito de Peluche | 25 | true | true | true |
| Llavero | 15 | true | false | false |
| Almohada | 30 | true | true | false |
| Cinta | 20 | false | false | true |

---

### `Productos`

```sql
CREATE TABLE "Productos" (
    "Id"               SERIAL           PRIMARY KEY,
    "Nombre"           VARCHAR(150)     NOT NULL,
    "Descripcion"      TEXT,
    "Precio"           DECIMAL(10,2)    NOT NULL,
    "Stock"            INT              NOT NULL DEFAULT 0,
    "ImagenUrl"        TEXT,
    "EsPersonalizable" BOOLEAN          NOT NULL DEFAULT TRUE,
    "CategoriaId"      INT              REFERENCES "Categorias"("Id"),
    "TipoProductoId"   INT              REFERENCES "TiposProducto"("Id"),
    "Activo"           BOOLEAN          NOT NULL DEFAULT TRUE
);

CREATE INDEX idx_productos_activo       ON "Productos" ("Activo");
CREATE INDEX idx_productos_categoria    ON "Productos" ("CategoriaId");
CREATE INDEX idx_productos_tipo         ON "Productos" ("TipoProductoId");
```

---

### `PersonalizacionesProducto`

```sql
CREATE TABLE "PersonalizacionesProducto" (
    "Id"           SERIAL           PRIMARY KEY,
    "ProductoId"   INT              NOT NULL REFERENCES "Productos"("Id"),
    "TipoOpcion"   VARCHAR(100)     NOT NULL,
    "ValorOpcion"  VARCHAR(100)     NOT NULL,
    "PrecioExtra"  DECIMAL(10,2)    NOT NULL DEFAULT 0.00
);
```

---

### `ModelosAutobus`

```sql
CREATE TABLE "ModelosAutobus" (
    "Id"         SERIAL          PRIMARY KEY,
    "Nombre"     VARCHAR(150)    NOT NULL,
    "Fabricante" VARCHAR(100),
    "Activo"     BOOLEAN         NOT NULL DEFAULT TRUE
);

CREATE INDEX idx_modelos_activo ON "ModelosAutobus" ("Activo");
```

**Ejemplos de registros:**

| Nombre | Fabricante |
|---|---|
| Irizar i8 | Irizar |
| Volvo 9800 | Volvo |
| Scania Touring | Scania |
| Mercedes-Benz Travego | Mercedes-Benz |

---

### `Lineas`

```sql
CREATE TABLE "Lineas" (
    "Id"              SERIAL          PRIMARY KEY,
    "Nombre"          VARCHAR(150)    NOT NULL,
    "ColorPrimario"   VARCHAR(7),     -- Hex #RRGGBB
    "ColorSecundario" VARCHAR(7),     -- Hex #RRGGBB
    "NombreOperador"  VARCHAR(100),
    "LogoUrl"         TEXT,
    "Activa"          BOOLEAN         NOT NULL DEFAULT TRUE
);

CREATE INDEX idx_lineas_activa ON "Lineas" ("Activa");
```

---

### `Pedidos`

> Tabla central de personalización. Cada registro es un ítem del carrito con todos sus atributos de personalización.

```sql
CREATE TABLE "Pedidos" (
    "Id"               SERIAL          PRIMARY KEY,
    "ProductoId"       INT             NOT NULL REFERENCES "Productos"("Id"),
    "ModeloAutobusId"  INT             REFERENCES "ModelosAutobus"("Id"),
    "LineaId"          INT             REFERENCES "Lineas"("Id"),
    "NombreOperador"   VARCHAR(100),
    "NumeroEconomico"  VARCHAR(20),
    "Ruta"             VARCHAR(150),
    "NotasEspeciales"  TEXT,
    "Cantidad"         INT             NOT NULL DEFAULT 1,
    "Estado"           VARCHAR(50)     NOT NULL DEFAULT 'Recibido',
    "FechaCreacion"    TIMESTAMP       NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_pedidos_estado ON "Pedidos" ("Estado");
```

**Valores válidos de `Estado`:**

```
'Recibido' → 'En producción' → 'Enviado' → 'Entregado'
```

---

### `Carritos`

```sql
CREATE TABLE "Carritos" (
    "Id"            SERIAL      PRIMARY KEY,
    "UsuarioId"     INT         NOT NULL UNIQUE REFERENCES "Usuarios"("Id"),
    "FechaCreacion" TIMESTAMP   NOT NULL DEFAULT NOW()
);
```

---

### `ItemsCarrito`

```sql
CREATE TABLE "ItemsCarrito" (
    "Id"              SERIAL           PRIMARY KEY,
    "CarritoId"       INT              NOT NULL REFERENCES "Carritos"("Id"),
    "ProductoId"      INT              NOT NULL REFERENCES "Productos"("Id"),
    "Cantidad"        INT              NOT NULL DEFAULT 1,
    "PrecioUnitario"  DECIMAL(10,2)    NOT NULL
);
```

---

### `Ordenes`

```sql
CREATE TABLE "Ordenes" (
    "Id"              SERIAL           PRIMARY KEY,
    "UsuarioId"       INT              NOT NULL REFERENCES "Usuarios"("Id"),
    "FechaOrden"      TIMESTAMP        NOT NULL DEFAULT NOW(),
    "Estado"          VARCHAR(50)      NOT NULL DEFAULT 'Recibido',
    "Total"           DECIMAL(10,2)    NOT NULL,
    "DireccionEnvio"  TEXT
);

CREATE INDEX idx_ordenes_usuario ON "Ordenes" ("UsuarioId");
CREATE INDEX idx_ordenes_estado  ON "Ordenes" ("Estado");
```

---

### `ItemsOrden`

```sql
CREATE TABLE "ItemsOrden" (
    "Id"                 SERIAL           PRIMARY KEY,
    "OrdenId"            INT              NOT NULL REFERENCES "Ordenes"("Id"),
    "ProductoId"         INT              NOT NULL REFERENCES "Productos"("Id"),
    "PersonalizacionId"  INT              REFERENCES "PersonalizacionesProducto"("Id"),
    "Cantidad"           INT              NOT NULL DEFAULT 1,
    "PrecioUnitario"     DECIMAL(10,2)    NOT NULL
);
```

---

## 3. Mapa de Entidades EF Core → Tablas

| Entidad C# | Tabla PostgreSQL | DbSet en AppDbContext |
|---|---|---|
| `Producto` | `Productos` | `Productos` |
| `TipoProducto` | `TiposProducto` | `TiposProducto` |
| `Categoria` | `Categorias` | `Categorias` |
| `PersonalizacionProducto` | `PersonalizacionesProducto` | `PersonalizacionesProducto` |
| `ModeloAutobus` | `ModelosAutobus` | `ModelosAutobus` |
| `Linea` | `Lineas` | `Lineas` |
| `Pedido` | `Pedidos` | `Pedidos` |
| `Carrito` | `Carritos` | `Carritos` |
| `ItemCarrito` | `ItemsCarrito` | `ItemsCarrito` |
| `Orden` | `Ordenes` | `Ordenes` |
| `ItemOrden` | `ItemsOrden` | `ItemsOrden` |
| `Usuario` | `Usuarios` | `Usuarios` |

---

## 4. AppDbContext — Estructura

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Usuario>                    Usuarios                    { get; set; }
    public DbSet<Categoria>                  Categorias                  { get; set; }
    public DbSet<TipoProducto>               TiposProducto               { get; set; }
    public DbSet<Producto>                   Productos                   { get; set; }
    public DbSet<PersonalizacionProducto>    PersonalizacionesProducto   { get; set; }
    public DbSet<ModeloAutobus>              ModelosAutobus              { get; set; }
    public DbSet<Linea>                      Lineas                      { get; set; }
    public DbSet<Pedido>                     Pedidos                     { get; set; }
    public DbSet<Carrito>                    Carritos                    { get; set; }
    public DbSet<ItemCarrito>                ItemsCarrito                { get; set; }
    public DbSet<Orden>                      Ordenes                     { get; set; }
    public DbSet<ItemOrden>                  ItemsOrden                  { get; set; }
}
```
