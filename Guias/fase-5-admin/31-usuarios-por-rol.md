# Guía 31 — Usuarios: un bloque por rol

> **Estado:** completada

## Objetivo

Quinto commit del rediseño de Admin. La tabla de usuarios pasa a **dos bloques
lado a lado**, uno por rol, y el alta deja de ser una pantalla propia.

Se suma además algo que faltaba: **restablecer la contraseña de otro usuario**.

## Decisiones de diseño (cerradas con maquetas)

- **Administradores y Cocina lado a lado**, con su conteo en el título. Lo que
  se revisa en esta pantalla es cuánta gente tiene permisos de administración,
  y así se ve de un vistazo.
- Los **desactivados** siguen a la vista, apagados, con "· inactivo" y el botón
  Reactivar. Esconderlos haría imposible recuperarlos.
- El **usuario propio** lleva el badge "vos" y no tiene botón de desactivar: se
  saca el botón en vez de dejarlo fallar al apretarlo.
- Un bloque para las cuentas **sin rol**, que pueden entrar pero no ven ninguna
  pantalla. Antes se mostraban con un guion en la columna Rol.
- **Alta en modal**, con la misma mecánica que el resto de Admin.

## Restablecer contraseña

`AccesoController` ya permitía que cada uno cambie la suya, pidiendo la
anterior. **No había forma de recuperar una cuenta**: el sistema no manda
mails, así que quien olvidaba la contraseña quedaba afuera para siempre.

Ahora el admin puede poner una nueva desde la ficha del usuario. El token de
reseteo se genera y se consume en el mismo request —no viaja a ningún lado— y
la validación de contraseña la sigue haciendo Identity, con sus mensajes.

Es la única vía de recuperación del sistema, así que conviene tenerla presente
al pensar la seguridad: cualquier admin puede tomar la cuenta de cualquiera.
Con dos roles y un equipo chico es razonable; si el equipo crece, se revisa.

## Cuidado: el prefijo del formulario

Los campos del modal se llaman `Nuevo.*` porque la vista es la pantalla entera,
no el formulario. Sin `[Bind(Prefix = "Nuevo")]` el binder no encuentra nada y
el alta falla **en silencio**: vuelve el modal con los campos vacíos como si
faltaran datos. Lo mismo para `NuevaPassword`.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/UsuarioViewModels.cs` | `UsuariosPaginaViewModel`, `RestablecerPasswordViewModel` |
| `Controllers/UsuariosController.cs` | Agrupado por rol, alta con prefijo, `RestablecerPassword` |
| `Views/Usuarios/Index.cshtml` | Los dos bloques con sus dos modales |
| `Views/Usuarios/_Usuario.cshtml` | Un usuario dentro de su bloque |
| `Views/Usuarios/Crear.cshtml` | **Eliminada** |
| `wwwroot/css/site.css` | Lenguaje `usr-*` |

## Verificación

- Los dos bloques muestran su conteo; el usuario propio lleva "vos" y no tiene
  botón de desactivar.
- El alta con una contraseña débil vuelve con los avisos de Identity y el modal
  abierto; con datos válidos, el usuario aparece en el bloque de su rol.
- Restablecer la contraseña de alguien y entrar con la nueva funciona.
- Desactivar apaga la fila y deja Reactivar.
- La ruta vieja (`Crear`) devuelve 404.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): agrupar los usuarios por rol y poder restablecer contraseñas
- Administradores y cocina lado a lado, con las cuentas inactivas a la vista
- Un admin puede poner una contraseña nueva: es la única recuperación posible
- Alta en modal, sin cambiar de pantalla
```
