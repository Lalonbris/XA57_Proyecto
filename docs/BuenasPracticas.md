# Guía de Buenas Prácticas y Estándares de Desarrollo

**Stack Tecnológico:** ASP.NET Core MVC, React, C#, Entity Framework Core, PostgreSQL, HTML, CSS, Bootstrap, JavaScript (Vanilla).

## 1. Arquitectura, ASP.NET Core MVC y C#
* **Separación de responsabilidades (Clean Architecture):** Mantener la lógica de negocio y el acceso a datos estrictamente fuera de los Controladores. Utilizar un patrón de Arquitectura en Capas (Servicios y Repositorios).
* **Inyección de Dependencias:** Utilizar el contenedor nativo de .NET (`builder.Services`). No instanciar dependencias con `new`; inyectarlas siempre a través del constructor.
* **Data Transfer Objects (DTOs) y ViewModels:** Nunca exponer entidades de base de datos directamente a las vistas o respuestas de API. Mapear los datos a DTOs o ViewModels específicos para cada caso de uso.
* **Manejo Global de Excepciones:** Implementar un Middleware para atrapar y registrar errores de manera global, devolviendo respuestas estandarizadas y evitando caídas inesperadas.

## 2. Entity Framework Core y PostgreSQL
* **Code-First y Migraciones:** Diseñar el modelo de dominio en C# y utilizar migraciones de EF Core para mantener el esquema de PostgreSQL versionado y sincronizado.
* **Programación Asíncrona:** Emplear siempre métodos asíncronos (`Async`/`Await`, ej. `ToListAsync()`) para operaciones de I/O y acceso a datos.
* **Prevención del Problema N+1:** Utilizar Eager Loading (`.Include()`) de forma estratégica al consultar entidades relacionadas para optimizar el rendimiento.
* **Fluent API:** Configurar las entidades y relaciones utilizando clases que implementen `IEntityTypeConfiguration<T>`, manteniendo los modelos de dominio limpios de Data Annotations.

## 3. Módulo de Personalización en React
* **Integración API:** Consumir y enviar datos exclusivamente a través de Controladores `[ApiController]` que devuelvan JSON, manteniendo una separación clara de los controladores MVC tradicionales que retornan vistas.
* **Componentización:** Diseñar la interfaz dividiéndola en componentes pequeños, de responsabilidad única y altamente reutilizables.
* **Manejo de Estado:** Utilizar hooks nativos (`useState`, `useContext`) para gestionar el estado local y global de la personalización en tiempo real.

## 4. Frontend Clásico (HTML, CSS, Bootstrap, Vanilla JS)
* **Estilos y Diseño (UI):** Mantener un diseño completamente en 2D. No aplicar sombras (`box-shadow`, `text-shadow`) a los elementos visuales, renderizando la interfaz de forma plana.
* **Uso de Bootstrap:** Priorizar las clases de utilidad nativas del framework antes de escribir reglas CSS personalizadas.
* **Modularidad en JS:** Envolver el código JavaScript puro en módulos o IIFE (Immediately Invoked Function Expressions) para aislar el comportamiento y no contaminar el scope global.
* **Separación de Archivos:** Prohibido el uso de código CSS o JS "inline" en los archivos `.cshtml`. Centralizar los scripts y estilos en sus respectivos archivos dentro de la carpeta `wwwroot`.

## 5. Entorno y Configuración (Visual Studio)
* **Control de Versiones:** Realizar commits atómicos y descriptivos en Git por cada unidad lógica de trabajo completada.
* **Estandarización de Código:** Respetar las reglas de formato y estilo definidas en el archivo `.editorconfig` de la solución.
* **Seguridad de Credenciales:** Utilizar "User Secrets" en el entorno de desarrollo local para proteger las cadenas de conexión de PostgreSQL; no exponer credenciales en el archivo `appsettings.json`.