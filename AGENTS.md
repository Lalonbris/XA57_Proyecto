# AGENTS.md

This document provides instructions for AI agents working on this codebase.

## Build, Lint, and Test Commands

### Build

This is a standard .NET 8.0 project. To build the project, use the following command:

```bash
dotnet build
```

The solution file is `XA57_Proyecto.sln`.

### Frontend (React Client)

The project includes a React-based frontend in the `XA57_Proyecto/configurador-client` directory. When making changes to the React components (e.g., `App.js`), you must compile the application and update the static files served by the .NET host.

**Build Workflow:**

1.  **Navigate to the client directory:**
    ```bash
    cd XA57_Proyecto/configurador-client
    ```
2.  **Run the build command:**
    ```bash
    npm run build
    ```
3.  **Copy the build artifacts:** The build generates a new set of static files in the `build/` directory. These must be copied to the .NET project's `wwwroot/react` directory.
    ```bash
    # Example using xcopy on Windows
    xcopy build ..\\wwwroot\\react /E /H /I /Y
    ```
4.  **Update the script reference:** The build command generates a JavaScript file with a unique hash in its name (e.g., `main.xxxxxxx.js`). You must update the `<script>` tag in the relevant `.cshtml` file (e.g., `Views/Productos/Detalle.cshtml`) to point to the new filename.

### Linting

There is no specific linter configured for this project. However, the project uses the default Roslyn analyzers that come with the .NET SDK. Pay attention to warnings and errors reported by the IDE and the build process.

### Testing

There is no test project configured in this solution. If you add new features, please also add a test project and write unit and integration tests.

- Use a well-known testing framework like xUnit or NUnit.
- Name the test project `XA57_Proyecto.Tests`.
- Follow the existing coding style and conventions in your tests.

To run a single test (once you have a test project), you can use the `dotnet test --filter` command. For example:

```bash
dotnet test --filter "FullyQualifiedName=MyNamespace.MyClass.MyTestMethod"
```

## Code Style Guidelines

### Imports

- Implicit usings are enabled for this project. This means you don't need to add common `using` statements at the top of the file.
- Keep `using` statements sorted alphabetically.

### Formatting

- **Indentation:** Use 4 spaces for indentation (no tabs).
- **Braces:** Place braces on a new line for control structures (`if`, `else`, `for`, `while`, etc.) and method/class/namespace declarations.
- **Spacing:** Use a single space after keywords (`if`, `for`, etc.) and commas. Use a single space around binary operators (`+`, `-`, `*`, `/`, `=`, etc.).

### Types

- **Nullable Reference Types:** Nullable reference types are enabled. Use them correctly to avoid null reference exceptions.
- **Type Inference:** Use `var` for local variables when the type is obvious from the right-hand side of the assignment.
- **Data Annotations:** Use data annotations (`[Table]`, `[Key]`, `[Column]`, `[Required]`, etc.) to configure the database mapping in domain entities.

### Naming Conventions

- **Classes, Interfaces, Enums, and Methods:** Use PascalCase (e.g., `ProductosController`, `IProductoService`, `TipoProducto`, `ObtenerTodosAsync`).
- **Properties and Public Fields:** Use PascalCase (e.g., `Id`, `Nombre`).
- **Local Variables:** Use camelCase (e.g., `producto`, `productos`).
- **Private Fields:** Use camelCase with a leading underscore (e.g., `_productoService`).
- **Database Tables and Columns:** Use snake_case (e.g., `productos`, `tipo_producto_id`).

### Error Handling

- For actions in controllers, check for null results from services and return appropriate `IActionResult` (e.g., `NotFound()`).
- Use `try-catch` blocks for operations that can throw exceptions, such as database operations.

### Architecture

- The project follows a clean architecture pattern with `Application`, `Domain`, and `Infrastructure` layers.
- It uses the repository pattern for data access.
- Dependency injection is used throughout the application.

### Asynchronous Programming

- Use `async` and `await` for I/O-bound operations (e.g., database access).
- Suffix asynchronous method names with `Async` (e.g., `ObtenerTodosAsync`).

## Example Code Snippets

### Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using XA57_Proyecto.Application.Interfaces;

namespace XA57_Proyecto.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
    }
}
```

### Entity

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XA57_Proyecto.Domain.Entities
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}
```

## Development Log

### 2026-03-13: Phase 1 - Service Validation

- **Custom Exception**: Created `Application/Exceptions/ValidationException.cs` for consistent error handling.
- **PedidoService Validation**:
  - Injected `IProductoRepository` and `ITipoProductoRepository`.
  - Added logic to `AgregarAsync` to validate:
    - `Cantidad` must be greater than 0.
    - `ProductoId` must exist and be active.
    - `NombreOperador`, `NumeroEconomico`, and `Ruta` do not exceed the `MaxCaracteres` defined in `TipoProducto`.
- **ProductoService Validation**:
  - Added new methods `AgregarAsync` and `ActualizarAsync` to the service and repository layers.
  - Implemented validation for product properties (`Nombre`, `Precio`, `TipoProductoId`).
- **New Repositories**:
  - Created `ITipoProductoRepository` and `TipoProductoRepository` for data access to `tipos_producto` table.
- **Dependency Injection**:
  - Registered `ITipoProductoRepository` in `Program.cs`.
- **Code Refinements**:
  - Fixed a nullable warning in `PedidoResultDto`.
- **Build Verification**: Ensured the project builds successfully after all changes.

### 2026-03-16: Phase 2 - Cart Functionality

- **Cart Customization**:
  - Added `Color` and `ColorHex` properties to the `Pedido` entity and `CarritoItemDto` to store product customization details.
  - Created a database migration to apply the new fields to the `Pedidos` table.
  - Updated `PedidoService` to map the new DTO properties to the entity.
  - Modified the cart view (`Views/Carrito/Index.cshtml`) to display the selected color.
- **Real-Time Cart Icon**:
  - Implemented a global JavaScript function (`actualizarIconoCarrito`) in `_Layout.cshtml` to fetch the cart item count and update the header badge.
  - Called this global function from the React component (`configurador-client/src/App.js`) and the Catalog view (`Views/Home/Catalogo.cshtml`) after adding a product to the cart.
  - Corrected inconsistencies in the element ID for the cart badge.
- **React Build Process**:
  - Documented the frontend build workflow in this file.
### 2026-03-20: Phase 3 - User Registration (CU-01)

- **Scaffolding**: Generated the default ASP.NET Core Identity pages for `Register`, `Login`, and `Account Management`.
- **User Model**: Extended the `ApplicationUser` entity to include `Nombre` and `Apellido` properties to capture more user details.
- **UI/UX Redesign**:
  - Completely redesigned and translated the `Register`, `Login`, and all `Manage Account` pages to match the project's custom look and feel (`xa57-*` design system).
  - Implemented responsive and consistent forms for a professional user experience.
- **Database Migration**: Created and applied a migration to add the new `Nombre` and `Apellido` columns to the `AspNetUsers` table.
- **Integration**:
  - Added the `_LoginPartial` view to the main layout to make authentication links accessible.
  - Corrected several CSS issues to ensure proper alignment and typography across the new pages.
- **Localization**: Translated all user-facing Identity validation messages and labels to Spanish.

