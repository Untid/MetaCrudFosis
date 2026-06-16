# Manual de MetaCrudFosis

Motor de generación de CRUD dinámico. Este manual cubre cómo arrancar el sistema completo y cómo usarlo.

---

## 1. Arquitectura del sistema

MetaCrudFosis está formado por cuatro aplicaciones independientes:

| Proyecto | Qué es | Puerto |
|----------|--------|--------|
| `MetaCrudFosis.Api` | API REST + SQLite (datos) + LiteDB (logs) | 5001 |
| `MetaCrudFosis.Web` | Web MVC moderna que consume la API | 5002 |
| `MetaCrudFosis.Generator` | Asistente con estética Windows XP que genera entidades | 5003 |
| `MetaCrudFosis.App` | Contenedor MAUI (escritorio Windows + móvil Android) | — |
| `MetaCrudFosis.Shared.Modern` | Librería de estilos compartidos (RCL) | — |

Los proyectos se ejecutan **por separado**, cada uno en su terminal.

---

## 2. Arrancar el sistema (uso normal en local)

Abrir una terminal por aplicación. Esperar a que cada una diga "listening" antes de seguir.

```bash
# Terminal 1 — API
dotnet run --project src/MetaCrudFosis.Api

# Terminal 2 — Web
dotnet run --project src/MetaCrudFosis.Web
```

Abrir en el navegador: `http://localhost:5002`

Para generar entidades, además:
```bash
# Terminal 3 — Generador
dotnet run --project src/MetaCrudFosis.Generator
```
Abrir: `http://localhost:5003`

> **Nota**: para uso solo en el PC, NO hace falta el parámetro `--urls`.
> Ese parámetro solo es necesario para que el móvil acceda (ver sección 6).
> En el navegador siempre se escribe `localhost` (o la IP real), NUNCA `0.0.0.0`.

---

## 3. Las tres (o cuatro) aplicaciones funcionando a la vez

Es posible tenerlas todas vivas simultáneamente, **cada una en su terminal**.
Como cada proyecto usa un puerto distinto (5001, 5002, 5003), no hay conflicto.

Si al arrancar una aparece un error de tipo *"address already in use"* / *"el puerto ya está en uso"*:
- Hay un proceso usando ese puerto (una ejecución anterior que no se cerró).
- Solución: cerrar la terminal anterior (Ctrl+C) o cerrar el proceso `dotnet` huérfano desde el Administrador de tareas.

Orden recomendado: **API primero**, luego Web, luego Generador. La Web necesita la API viva
para mostrar datos; el Generador es independiente.

---

## 4. Uso de la Web (CRUD)

- **Listar**: la tabla muestra los registros de la entidad.
- **Crear**: botón "+ Crear" → formulario → Guardar.
- **Editar / Eliminar**: botones en cada fila (eliminar pide confirmación).
- **Filtrar**: elegir campo en el desplegable y escribir en la caja de búsqueda.
  - Texto: búsqueda parcial.
  - Números: valor exacto.
  - Fechas: por año (`2026`), año-mes (`2026-05`) o día completo (`2026-05-06`).
  - Booleanos: escribir `sí`/`no` o `true`/`false`.
- **Importar varios**: desplegable "Importar", pegar desde Excel/Sheets (tabuladores o comas)
  o pegar JSON exportado por la propia app.
- **Exportar**: botones "Exportar JSON" / "Exportar XML".
- **Actividad**: registro de logs del sistema (altas, bajas, errores, generaciones).

---

## 5. Generar una entidad nueva (Generador)

1. Arrancar el Generador (`http://localhost:5003`).
2. Indicar el **nombre** de la entidad (singular, ej. `Zapato`).
3. Añadir **campos**: nombre + tipo (string, int, decimal, DateTime, bool).
   - Atajo: "Pegado inteligente", pegar líneas `Nombre:Tipo` (ej. `Talla:int`).
4. Elegir el **color corporativo**.
5. Pulsar **⚡ Generar Aplicación**.

> **IMPORTANTE — tras generar**: para que la tabla de la nueva entidad aparezca en la base
> de datos, hay que **reiniciar la API**. Gracias a la creación incremental de tablas, NO se
> borra la base de datos ni se pierden datos existentes: solo se añade la tabla nueva.

---

## 6. Usar la app en el móvil (Android)

Requiere que el móvil acceda al PC por la red local. Pasos:

1. Averiguar la IP del PC: `ipconfig` → "Dirección IPv4" del adaptador WiFi (no la que acaba en `.1`).
2. Poner esa IP en `src/MetaCrudFosis.App/Config.cs` (bloque `#if ANDROID`).
3. Recompilar e instalar la app en el móvil desde Visual Studio.
4. Arrancar API y Web **expuestas a la red**:
   ```bash
   dotnet run --project src/MetaCrudFosis.Api --urls "http://0.0.0.0:5001"
   dotnet run --project src/MetaCrudFosis.Web --urls "http://0.0.0.0:5002"
   ```
5. PC y móvil en la misma WiFi. Si no conecta, revisar el firewall de Windows (puertos 5001/5002 en red privada).

> Para el detalle del día de la presentación, ver `SISTEMA_DESPLIEGUE.md`.

### Limitaciones conocidas en el móvil
- **Exportar JSON/XML**: no descarga en el contenedor MAUI (el WebView no gestiona descargas de archivos).
  Funciona en la versión web (navegador) y en escritorio. Pendiente de mejora (ver `IDEAS_FUTURAS.md`).

---

## 7. Copia de seguridad

Ejecutar el script de backup desde la raíz del repositorio:
- Windows: `backup.bat`
- Linux/Mac: `backup.sh` (dar permiso una vez: `chmod +x backup.sh`)

Copia `metacrudfosis.db` y `logs.db` a la carpeta `backups/` con marca de tiempo,
y elimina automáticamente las copias de más de 7 días (política de retención).

---

## 8. Plataformas

| Plataforma | Cómo |
|------------|------|
| Web | Navegador, `http://localhost:5002` |
| Escritorio | App MAUI Windows (usa `localhost`, sin configuración extra) |
| Móvil | App MAUI Android (requiere IP del PC, ver sección 6) |