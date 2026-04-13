# Guía de Configuración de Base de Datos para XA57_Proyecto

Este documento proporciona instrucciones paso a paso para que los desarrolladores configuren su base de datos PostgreSQL local para trabajar con el proyecto XA57_Proyecto.

## Requisitos Previos

Antes de comenzar, asegúrese de tener instalado:
1. [PostgreSQL](https://www.postgresql.org/download/) (versión 12 o superior recomendada)
2. [.NET SDK](https://dotnet.microsoft.com/download) (versión 8.0 o superior)
3. Un editor de código (como Visual Studio, VS Code, Rider, etc.)

## Paso 1: Verificar la instalación de PostgreSQL

Para verificar que PostgreSQL está instalado y funcionando correctamente:

### En Windows:
1. Abra el símbolo del sistema
2. Ejecute: `psql --version`
3. Debería ver algo como: `psql (PostgreSQL) 14.x`

### En macOS/Linux:
1. Abra la terminal
2. Ejecute: `psql --version`
3. Debería ver algo como: `psql (PostgreSQL) 14.x`

## Paso 2: Iniciar el servicio de PostgreSQL

### En Windows:
1. Abra el Servicios de Windows (services.msc)
2. Busque "PostgreSQL" en la lista
3. Asegúrese de que esté iniciado (clic derecho → Iniciar)

### En macOS (con Homebrew):
```bash
brew services start postgresql
```

### En Linux (Ubuntu/Debian):
```bash
sudo systemctl start postgresql
```

## Paso 3: Conectar a PostgreSQL como superusuario

Para crear la base de datos y usuario, primero necesitamos conectarnos como el usuario postgres (que se crea durante la instalación):

### En Windows:
1. Abra el símbolo del sistema
2. Ejecute: `psql -U postgres`
3. Cuando solicite la contraseña, ingrese la que estableció durante la instalación de PostgreSQL

### En macOS/Linux:
1. Abra la terminal
2. Ejecute: `sudo -u postgres psql`
3. O si prefiere especificar el usuario: `psql -U postgres`

Debería ver el prompt de PostgreSQL: `postgres=#`

## Paso 4: Ejecutar el script de configuración

Hay dos formas de ejecutar el script de configuración:

### Opción A: Usando el archivo SQL directamente (Recomendado)

1. Salga de psql si actualmente está dentro (escriba `\q` y presione Enter)
2. Asegúrese de estar en el directorio raíz del proyecto XA57_Proyecto
3. Ejecute el siguiente comando:
   ```bash
   psql -U postgres -f database-setup.sql
   ```
4. Cuando solicite la contraseña, ingrese la contraseña del usuario postgres

### Opción B: Copiar y pegar en psql

1. Dentro del prompt de psql (`postgres=#`), escriba:
   ```bash
   \i ruta/absoluta/hacia/database-setup.sql
   ```
   Por ejemplo:
   ```bash
   \i C:/Users/laloa/source/repos/XA57_Proyecto/database-setup.sql
   ```
   O en macOS/Linux:
   ```bash
   \i /Users/laloa/source/repos/XA57_Proyecto/database-setup.sql
   ```

## Paso 5: Verificar que la base de datos se creó correctamente

1. Dentro del prompt de psql, conéctese a la base de datos recién creada:
   ```bash
   \c xa57_db
   ```
2. Liste las tablas para verificar que se crearon correctamente:
   ```bash
   \dt
   ```
3. Debería ver una lista de tablas incluyendo:
   - AspNetRoles
   - AspNetUserClaims
   - AspNetUserLogins
   - AspNetUserRoles
   - AspNetUserTokens
   - AspNetUsers
   - lineas
   - modelos_autobus
   - pedidos
   - productos
   - tipos_producto

4. Para ver los datos iniciales insertados:
   ```bash
   SELECT * FROM "AspNetRoles";
   SELECT * FROM "tipos_producto";
   ```

## Paso 6: Configurar el proyecto

1. Abra el proyecto XA57_Proyecto en su IDE preferido
2. Verifique que el archivo `appsettings.json` tenga la configuración correcta:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=xa57_db;Username=postgres;Password=su_contraseña_aquí"
     },
     // ... resto de la configuración
   }
   ```
3. **Importante**: Reemplace `su_contraseña_aquí` con la contraseña real del usuario postgres que estableció durante la instalación

## Paso 7: Ejecutar el proyecto

1. En su IDE, inicie el proyecto (F5 en Visual Studio, o `dotnet run` en la terminal)
2. El aplicación debería iniciar correctamente y conectarse a la base de datos
3. Al iniciar, el código en Program.cs ejecutará automáticamente el seeding de datos:
   - Creará el rol "Administrador" si no existe
   - Creará el rol "Cliente" si no existe
   - Creará un usuario administrador con email `admin@xa57.com` y contraseña `Admin123!`

## Solución de Problemas Comunes

### Error: "connection to server at localhost failed"
- **Causa**: PostgreSQL no está en ejecución o no está aceptando conexiones en localhost
- **Solución**: 
  - Verifique que el servicio de PostgreSQL esté iniciado
  - Asegúrese de que PostgreSQL esté configurado para escuchar en localhost (por defecto lo está)

### Error: password authentication failed for user "postgres"
- **Causa**: Contraseña incorrecta para el usuario postgres
- **Solución**:
  - Si no recuerda la contraseña, puede restablecerla siguiendo las instrucciones específicas de su sistema operativo
  - En desarrollo, también puede usar autenticación por confianza modificando pg_hba.conf (no recomendado para producción)

### Error: database "xa57_db" does not exist
- **Causa**: El script no se ejecutó completamente o hubo un error al crear la base de datos
- **Solución**:
  - Ejecute manualmente: `CREATE DATABASE xa57_db;` en psql como usuario postgres
  - Luego vuelva a ejecutar el script

### Error: permission denied para crear objetos
- **Causa**: El usuario con el que se está ejecutando el script no tiene suficientes permisos
- **Solución**:
  - Asegúrese de ejecutar el script como el usuario postgres o un usuario con permisos de superusuario
  - O otorgue permisos suficientes al usuario que está utilizando

## Verificación Final

Para verificar que todo funciona correctamente:

1. Ejecute el proyecto
2. Navegue a `/swagger` (si está habilitado) o intente acceder a cualquier endpoint API
3. Intente iniciar sesión con:
   - Email: `admin@xa57.com`
   - Contraseña: `Admin123!`
4. Debería poder acceder al sistema como administrador

## Notas Adicionales

- Este script está diseñado para ser idempotente: puede ejecutarlo múltiples veces sin causar errores
- En entornos de desarrollo, se recomienda que cada desarrollador tenga su propia instancia local de PostgreSQL
- Para entornos de producción, considere usar herramientas de migración como Flyway o los comandos EF Core directamente
- Nunca comparta contraseñas reales en repositorios públicos - use variables de conexión mediante variables de entorno o secret managers en producción

## ¿Necesita ayuda adicional?

Si encuentra problemas que no están documentados aquí, por favor:
1. Verifique los logs de PostgreSQL para mensajes de error específicos
2. Consulte la documentación oficial de PostgreSQL: https://www.postgresql.org/docs/
3. Pregunte a un compañero de equipo con más experiencia en bases de datos