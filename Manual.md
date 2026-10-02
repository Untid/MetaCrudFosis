# 📖 Manual de Usuario y Desarrollador: MetaCrudFosis

Este documento proporciona una guía completa para desplegar, configurar y utilizar el sistema MetaCrudFosis, tanto desde la perspectiva del desarrollador como del usuario final.

---

## 1. 🏗️ Arquitectura y Componentes del Sistema

La solución se compone de aplicaciones independientes que se comunican entre sí. Cada una debe ejecutarse en su propio proceso o terminal:

| Proyecto | Descripción | Puerto por defecto |
|----------|-------------|:------------------:|
| `MetaCrudFosis.Api` | Núcleo backend: API REST, SQLite (datos) y LiteDB (logs de auditoría). | `5001` |
| `MetaCrudFosis.Web` | Frontend: Aplicación ASP.NET Core MVC moderna que consume la API. | `5002` |
| `MetaCrudFosis.Generator` | Herramienta de scaffolding: Asistente web con estética Windows XP para definir entidades. | `5003` |
| `MetaCrudFosis.App` | Cliente multiplataforma: Contenedor .NET MAUI (Windows Desktop / Android). | N/A |
| `MetaCrudFosis.Shared.Modern` | Librería de clases Razor (RCL) con estilos CSS y layouts compartidos. | N/A |

---

## 2. 🚀 Inicio Rápido (Entorno de Desarrollo Local)

Para levantar el sistema completo, se recomienda abrir **tres terminales independientes** en la raíz del repositorio y ejecutar los proyectos en el siguiente orden:

### Paso 1: Iniciar la API (Requisito previo)
```bash
dotnet run --project src/MetaCrudFosis.Api
```
*(Esperar a que la consola indique que está escuchando en `http://localhost:5001`)*

### Paso 2: Iniciar la Aplicación Web
```bash
dotnet run --project src/MetaCrudFosis.Web
```
*(Acceder desde el navegador a: `http://localhost:5002`)*

### Paso 3: Iniciar el Generador (Opcional, solo si se van a crear entidades)
```bash
dotnet run --project src/MetaCrudFosis.Generator
```
*(Acceder desde el navegador a: `http://localhost:5003`)*

> ⚠️ **Resolución de conflictos de puertos:**  
> Si aparece un error *"address already in use"* o *"el puerto ya está en uso"*, significa que una ejecución anterior de `dotnet` quedó en segundo plano.  
> **Solución:** Cierra la terminal anterior con `Ctrl+C` o finaliza los procesos `dotnet.exe` huérfanos desde el Administrador de Tareas.

---

## 3. ⚙️ Flujo de Trabajo: Generar una Nueva Entidad

El motor de scaffolding permite añadir funcionalidad al sistema sin escribir código manualmente. Sigue estos pasos:

1. Accede al Generador en `http://localhost:5003`.
2. Introduce el **nombre de la entidad** en singular (ej: `Zapato`, `Cliente`).
3. Define los **campos** (Nombre y Tipo: `string`, `int`, `decimal`, `DateTime`, `bool`).  
   *💡 Tip: Usa el "Pegado inteligente" para introducir múltiples campos a la vez con formato `Nombre:Tipo` (ej: `Talla:int`).*
4. Selecciona un **color corporativo** para la temática de la interfaz generada.
5. Haz clic en **⚡ Generar Aplicación**.

> 🛑 **IMPORTANTE: Sincronización de Base de Datos**  
> Tras generar una nueva entidad, **es obligatorio reiniciar el proyecto `MetaCrudFosis.Api`**.  
> Gracias a la configuración de migraciones incrementales de EF Core, la API detectará el nuevo modelo y creará la tabla correspondiente en SQLite **sin borrar ni afectar los datos existentes**.

---

## 4. 💻 Guía de Uso de la Aplicación Web (CRUD)

Una vez generada la entidad, la interfaz web (`localhost:5002`) ofrece las siguientes capacidades:

- **Listado y Navegación:** Tabla dinámica con paginación y navbar que se actualiza automáticamente con las nuevas entidades.
- **Creación y Edición:** Formularios generados con validaciones visuales y botones de acción claros.
- **Filtrado Avanzado:** 
  - *Texto:* Búsqueda parcial (contiene).
  - *Números:* Coincidencia exacta.
  - *Fechas:* Soporta formatos `AAAA`, `AAAA-MM` o `AAAA-MM-DD`.
  - *Booleanos:* Acepta `sí`/`no` o `true`/`false`.
- **Importación Masiva (Bulk Paste):** Permite pegar datos directamente desde Excel/Google Sheets (separados por tabuladores) o importar JSON exportado previamente.
- **Exportación:** Descarga de los datos filtrados en formato `JSON` o `XML`.
- **Auditoría:** Panel de "Actividad" que registra logs del sistema (altas, bajas, errores y generaciones) mediante LiteDB.

---

## 5. 📱 Despliegue y Uso en Dispositivos Móviles (.NET MAUI)

Para probar el cliente móvil (Android) conectado a tu máquina de desarrollo, es necesario exponer los servicios en la red local:

1. **Obtener la IP local del PC:** Ejecuta `ipconfig` (Windows) y anota la "Dirección IPv4" de tu adaptador WiFi/Ethernet (ej: `192.168.1.XX`).
2. **Configurar la App MAUI:** Edita el archivo `src/MetaCrudFosis.App/Config.cs` y actualiza la IP en el bloque `#if ANDROID`.
3. **Exponer los puertos:** Al iniciar la API y la Web, añade el parámetro `--urls` para escuchar en todas las interfaces de red:
```bash
dotnet run --project src/MetaCrudFosis.Api --urls "http://0.0.0.0:5001"
dotnet run --project src/MetaCrudFosis.Web --urls "http://0.0.0.0:5002"
```
4. **Conectividad:** Asegúrate de que el móvil y el PC estén en la misma red WiFi. Si hay problemas de conexión, verifica que el Firewall de Windows permita el tráfico entrante en los puertos `5001` y `5002` para redes privadas.

> 📌 *Nota técnica:* La funcionalidad de exportación de archivos (JSON/XML) desde el WebView de MAUI tiene limitaciones nativas de descarga. Esta función está optimizada para la versión Web de escritorio. (Ver `IDEAS_FUTURAS.md` para la hoja de ruta de solución).  
> *Para configuraciones avanzadas de red y despliegue en producción, consultar `SISTEMA_DESPLIEGUE.md`.*

---

## 6. 💾 Mantenimiento y Copias de Seguridad

El sistema incluye scripts automatizados en la raíz del repositorio para respaldar las bases de datos (`metacrudfosis.db` y `logs.db`):

- **Windows:** Ejecutar `backup.bat`
- **Linux/macOS:** Ejecutar `chmod +x backup.sh` y luego `./backup.sh`

*Política de retención:* El script genera una copia con marca de tiempo en la carpeta `backups/` y elimina automáticamente los archivos con más de 7 días de antigüedad.


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
