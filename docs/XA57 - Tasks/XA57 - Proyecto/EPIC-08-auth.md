# EPIC-08 — Autenticación y Gestión de Usuarios

> → Spec: `02-functional-requirements.md` CU-01, `07-business-rules.md` RN-08, RN-09  
> → Log: Phase 3 (2026-03-20)  
> Estado: ✅ Completada  
> **Nota de implementación:** Se utilizó **ASP.NET Core Identity** en lugar de autenticación manual por cookie, lo cual cubre este epic completo con mayor robustez y sin código adicional de hashing.

---

- [x] **EPIC-08-01** — Scaffolding de ASP.NET Core Identity
  - → Log: Phase 3 (2026-03-20)
  - Páginas generadas: `Register`, `Login`, `Account Management`
  - Identity configurado en `Program.cs` con `AddDefaultIdentity<ApplicationUser>()`
  - `AppDbContext` hereda de `IdentityDbContext<ApplicationUser>`

- [x] **EPIC-08-02** — Modelo `ApplicationUser` extendido
  - → Log: Phase 3 (2026-03-20)
  - `ApplicationUser` extiende `IdentityUser` con:
    - `Nombre : string` (requerido)
    - `Apellido : string` (requerido)
  - Migración aplicada: columnas en `AspNetUsers`

- [x] **EPIC-08-03** — Formulario de Registro (CU-01)
  - → Log: Phase 3 (2026-03-20)
  - Campos: Nombre, Apellido, Email, Contraseña, Confirmar contraseña
  - Validaciones de Identity activas (email único, fuerza de contraseña)
  - Rediseñado con sistema de diseño `xa57-*`
  - Mensajes de error traducidos al español
  - Al registrar exitosamente → redirige al login (o al catálogo si hay sesión automática)

- [x] **EPIC-08-04** — Formulario de Login
  - → Log: Phase 3 (2026-03-20)
  - Campos: Email, Contraseña, Recordarme
  - Rediseñado con sistema de diseño `xa57-*`
  - Mensajes de error en español
  - Login exitoso → redirige al catálogo

- [x] **EPIC-08-05** — Páginas de gestión de cuenta (`Manage/`)
  - → Log: Phase 3 (2026-03-20)
  - Todas las páginas de `Areas/Identity/Pages/Account/Manage/` rediseñadas con `xa57-*`
  - Traducciones al español aplicadas
  - Correcciones de CSS en alineación y tipografía

- [x] **EPIC-08-06** — Integración en el Layout principal
  - → Log: Phase 3 (2026-03-20)
  - `_LoginPartial.cshtml` agregado a `_Layout.cshtml`
  - Muestra "Hola, {Nombre}" cuando hay sesión activa
  - Muestra enlaces a "Iniciar sesión" / "Registrarse" cuando no hay sesión

- [x] **EPIC-08-07** — Protección de rutas con `[Authorize]`
  - `CarritoController` protegido con `[Authorize]`
  - Sin sesión → redirige a `/Identity/Account/Login`
  - Panel de administración protegido con `[Authorize(Roles = "Administrador")]`
