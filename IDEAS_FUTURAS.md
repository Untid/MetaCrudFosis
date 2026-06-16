# Ideas y Líneas Futuras de Desarrollo

Este documento recoge mejoras y funcionalidades identificadas que, por limitación de tiempo del TFC, se han dejado para versiones futuras del sistema.

## Corto Plazo (Bonus)
- [ ] **Plugin de Base de Datos:** permitir elegir entre SQLite (por defecto) y SQL Server desde el generador, modificando dinámicamente el `DbContext` y las cadenas de conexión.
- [ ] **Botón de Limpieza:** implementar en el generador una función que lea un `manifest.json` y elimine los archivos de la última entidad generada, para permitir pruebas iterativas limpias.
- [ ] **Sistema de Login para el CRUD:** incorporar autenticación (p. ej. ASP.NET Core Identity) para que el acceso al CRUD requiera usuario y, opcionalmente, roles diferenciados. Se valora generar/mutar también la parte de login mediante plantillas.
- [ ] **Migración a HTTPS:** que las URLs no sean solo HTTP sino HTTPS, validando y certificando su uso (API, Web, CORS, BaseAddress del HttpClient, fetch JS y WebView de MAUI). Pospuesto: en localhost HTTP es suficiente.

## Testing
- [ ] **Tests automatizados:** añadir un proyecto de pruebas (xUnit) con tests unitarios del `GenericRepository` (CRUD y filtrado) y del `TemplateEngineService` (sustitución de placeholders), para complementar la validación manual actual.
- [ ] **Generación de tests por entidad:** que el generador cree automáticamente una clase de pruebas básica para cada entidad nueva.

## Interfaz y UX
- [ ] Portada (Home) con bienvenida y enlace a las entidades, o pantalla de carga con estilo.
- [ ] Importación directa de archivos `.csv` mediante un input de archivo (además del "Bulk Paste" actual).
- [ ] Página índice que liste las entidades generadas (complementando el navbar dinámico).
- [ ] Filtro de campos booleanos con desplegable Sí/No en vez de caja de texto (mejora en Index y en la plantilla ViewIndex.tt).
- [ ] Mostrar "Fecha Alta" con espacio: insertar un espacio antes de cada mayúscula al generar el `Display`. Puro pulido.
- [ ] Refinar el diseño responsive en móvil (tablas y formularios).

## Arquitectura y Generación
- [ ] Extraer el JavaScript del Index a un `crud-generico.js` compartido en la RCL, parametrizado por entidad. Reduce lo que el motor genera y centraliza la lógica.
- [ ] Validaciones avanzadas: inyectar Data Annotations (`[Required]`, `[StringLength]`...) en las plantillas según la configuración del usuario.
- [ ] Soporte de imágenes: tipo de campo "Image" que gestione `IFormFile`, guarde la ruta en la BD y muestre un preview.
- [ ] Relaciones entre entidades: claves foráneas (1:N, N:M) y desplegables dinámicos en los formularios.

## Generador como aplicación de escritorio (estética XP "pura")
- [ ] Explorar recrear la estética XP en una aplicación de escritorio nativa, en lugar de XP.css en web.
- [ ] Generar un ejecutable (`.exe`) del generador.
- [ ] Opción avanzada: Generador como MAUI sin bordes (borderless) + WebView2 con región de arrastre, para una estética XP sin el marco moderno de la ventana. Hacer cuando MAUI ya esté instalado y el motor validado.

## Exportación en móvil
- [ ] Exportación de archivos desde el contenedor MAUI: el WebView no gestiona descargas. Opciones: handler nativo de descargas en MAUI, o abrir el export en un navegador externo.

## Infraestructura y despliegue
- [ ] Despliegue en Docker: un `docker-compose.yml` que levante la API, la Web y, opcionalmente, una base de datos SQL Server en contenedores.