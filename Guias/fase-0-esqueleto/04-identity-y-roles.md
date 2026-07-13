# Guía 04 — Identity y roles

> **Estado:** completada

## Objetivo

Autenticación completa con ASP.NET Core Identity: login, logout, cambio de contraseña y gestión de usuarios por el Admin (sin registro público, decisión de la guía 00). Al terminar, la app entera exige login, el admin puede crear usuarios con rol `Admin` o `Cocina`, y desactivarlos sin borrarlos.

## Decisiones

- **Login por email** (el `UserName` de Identity ES el email, como sembró el seeder de la guía 03).
- **Sin registro público**: no existe página de registro; el alta es un ABM que solo ve el rol `Admin`.
- **Desactivar en lugar de borrar**: el flag `Activo` de `ApplicationUser` bloquea el login pero conserva las FKs históricas (comandas, costos, movimientos referencian usuarios). El Admin no puede desactivarse a sí mismo.
- **Lockout**: 5 intentos fallidos bloquean la cuenta temporalmente (configurado en la guía 03).
- **Mensajes en español**: `IdentityErrorDescriber` propio para los errores comunes de Identity (contraseña corta, email duplicado, etc.).
- La pantalla de login usa la paleta Southex con estilos propios (`Layout = null`); el layout general con sidebar llega en la guía 05, por eso las vistas de usuarios usan el layout del template por ahora.

## Piezas

| Archivo | Contenido |
|---|---|
| `Controllers/AccesoController.cs` | `Login` (GET/POST), `Logout` (POST), `CambiarPassword` (GET/POST), `Denegado` |
| `Controllers/UsuariosController.cs` | `[Authorize(Roles = "Admin")]`: listado, alta con rol, activar/desactivar |
| `Controllers/HomeController.cs` | Se agrega `[Authorize]`: nada es público salvo el login |
| `ViewModels/LoginViewModel.cs` | Email, password, recordarme |
| `ViewModels/CambiarPasswordViewModel.cs` | Actual, nueva, confirmación |
| `ViewModels/UsuarioViewModels.cs` | `CrearUsuarioViewModel` (datos + rol) y `UsuarioListaViewModel` |
| `Helpers/IdentityErrorDescriberEspanol.cs` | Traducción de errores de Identity |
| `Views/Acceso/Login.cshtml` | Card centrada con paleta Southex, `Layout = null` |
| `Views/Acceso/CambiarPassword.cshtml`, `Denegado.cshtml` | — |
| `Views/Usuarios/Index.cshtml`, `Crear.cshtml` | Listado con badges de rol/estado y formulario de alta |

Reglas de negocio del alta/estado:
- El alta pide nombre, apellido, email, contraseña inicial y rol (`Admin`/`Cocina`).
- Activar/desactivar es un POST con antiforgery; si el usuario objetivo es el mismo que está logueado, se rechaza.
- El login rechaza usuarios con `Activo = false` con el mismo mensaje genérico que credenciales inválidas (no revelar cuáles emails existen).

## Verificación

- `dotnet run` → `GET /` redirige a `/Acceso/Login?ReturnUrl=%2F` (302).
- Login con `admin@recetario.local` / `Admin123!` → entra a Home.
- `/Usuarios` como Admin → lista al admin. Crear un usuario `Cocina` → aparece en la lista.
- Login con el usuario Cocina → `/Usuarios` devuelve acceso denegado.
- Desactivar el usuario Cocina → no puede loguearse más.
- Logout → vuelve al login y `GET /` vuelve a redirigir.

## Próximo paso

[Guía 05 — Layout y navegación](05-layout-y-navegacion.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar autenticación y gestión de usuarios
- Login, logout y cambio de contraseña con Identity
- ABM de usuarios solo Admin con roles y activar/desactivar
- Errores de Identity traducidos al español
```
