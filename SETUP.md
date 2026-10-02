# ⚙️ Guía de Configuración y Despliegue Local (SETUP)

Este documento detalla los prerrequisitos, la configuración inicial y los pasos exactos para levantar el entorno de desarrollo de **MetaCrudFosis** en tu máquina local.

---

## 1. 🖥️ Prerrequisitos del Sistema

Antes de comenzar, asegúrate de tener instaladas las siguientes herramientas:

- **.NET 10 SDK**: [Descargar aquí](https://dotnet.microsoft.com/download) (o la versión LTS especificada en el proyecto).
- **IDE**: Visual Studio 2026 (con las cargas de trabajo "Desarrollo de ASP.NET y web" y "Desarrollo multiplataforma con .NET") o Visual Studio Code con la extensión C# Dev Kit.
- **Git**: Para clonar el repositorio y gestionar el control de versiones.
- **(Opcional para móvil)**: Emulador de Android o dispositivo físico con modo desarrollador activado, y las herramientas de Android SDK configuradas.

---

## 2. 📂 Estructura de la Solución

El repositorio está organizado en una arquitectura limpia y modular. Cada componente debe ejecutarse de forma independiente:

| Proyecto | Rol en la Arquitectura | Puerto |
|----------|------------------------|:------:|
| `MetaCrudFosis.Api` | **Core/Infraestructura**: API REST, EF Core, SQLite (datos) y LiteDB (logs). | `5001` |
| `MetaCrudFosis.Web` | **Presentación**: Aplicación ASP.NET Core MVC que consume la API. | `5002` |
| `MetaCrudFosis.Generator` | **Herramienta**: Asistente web (estética XP) para el scaffolding de entidades. | `5003` |
| `MetaCrudFosis.App` | **Cliente**: Contenedor .NET MAUI para escritorio (Windows) y móvil (Android). | N/A |
| `MetaCrudFosis.Shared.Modern` | **Recursos**: Razor Class Library (RCL) con CSS y layouts compartidos. | N/A |

---

## 3. 🚀 Inicio Rápido: Levantar el Entorno

Sigue estos pasos en orden para asegurar que todos los servicios se comuniquen correctamente. Se recomienda usar **tres terminales independientes** abiertas en la raíz del repositorio.

### Paso 1: Iniciar el Núcleo (API)
Es el componente base. Debe estar activo antes que los demás.
```bash
dotnet run --project src/MetaCrudFosis.Api
```
*(Verifica en la consola que indica: "Now listening on: http://localhost:5001")*

### Paso 2: Iniciar la Interfaz Web (CRUD)
```bash
dotnet run --project src/MetaCrudFosis.Web
```
*(Accede en tu navegador a: `http://localhost:5002`)*

### Paso 3: Iniciar el Generador de Entidades (Opcional)
Solo necesario si vas a crear nuevas entidades en esta sesión.
```bash
dotnet run --project src/MetaCrudFosis.Generator
```
*(Accede en tu navegador a: `http://localhost:5003`)*

> ⚠️ **REGLA DE ORO TRAS GENERAR:**  
> Cada vez que uses el Generador para crear una nueva entidad, **debes reiniciar el proceso de `MetaCrudFosis.Api`** (Ctrl+C y volver a ejecutar).  
> *¿Por qué?* EF Core necesita reinicializarse para detectar el nuevo modelo y aplicar la migración incremental de la tabla en SQLite. **Tus datos existentes no se perderán.**

---

## 4. 🛠️ Configuración para .NET MAUI (Escritorio / Móvil)

Si deseas ejecutar el cliente `MetaCrudFosis.App`:

1. **Windows (Escritorio)**: Establece `MetaCrudFosis.App` como proyecto de inicio en Visual Studio y ejecútalo. Se conectará automáticamente a `localhost`.
2. **Android (Dispositivo físico)**:
   - Averigua la IP local de tu PC (ejecuta `ipconfig` en Windows).
   - Edita el archivo `src/MetaCrudFosis.App/Config.cs` y reemplaza `localhost` por tu IP en el bloque `#if ANDROID`.
   - Inicia la API y la Web exponiéndolas a la red local:
     ```bash
     dotnet run --project src/MetaCrudFosis.Api --urls "http://0.0.0.0:5001"
     dotnet run --project src/MetaCrudFosis.Web --urls "http://0.0.0.0:5002"
     ```
   - Asegúrate de que el Firewall de Windows permita el tráfico en los puertos `5001` y `5002`.

---

## 5. 🔧 Solución de Problemas Comunes (Troubleshooting)

| Problema | Causa Probable | Solución |
|----------|----------------|----------|
| *"Address already in use"* / *"El puerto ya está en uso"* | Una ejecución anterior de `dotnet` quedó colgada en segundo plano. | Cierra la terminal con `Ctrl+C` o finaliza el proceso `dotnet.exe` desde el Administrador de Tareas. |
| La Web muestra error 500 o "No se puede conectar a la API" | La API no está ejecutándose o está en un puerto distinto. | Verifica que la Terminal 1 esté activa y escuchando en el puerto `5001`. |
| El generador crea los archivos, pero no aparecen en la Web | La API no se reinició tras la generación. | Detén la API (Ctrl+C) y vuelve a ejecutarla para que EF Core aplique los cambios. |

---

## 📚 Navegación de la Documentación
Para una inmersión completa en el proyecto, consulta los siguientes documentos:
- 🏠 [README: Descripción General](./README.md)
- ⚙️ [SETUP: Guía de Configuración](./SETUP.md)
- 🏗️ [ARQUITECTURA: Sistema y Patrones](./ARQUITECTURA.md)
- 📖 [MANUAL: Usuario y Desarrollador](./Manual.md)
- 🌳 [GitWork: Flujo y Convenciones](./GitWork.md)
- 🚀 [SISTEMA_DESPLIEGUE: Runbook de Demo](./SISTEMA_DESPLIEGUE.md)
- 🔮 [IDEAS_FUTURAS: Roadmap](./IDEAS_FUTURAS.md)
