
---

### 4. `IDEAS_FUTURAS.md` (Tu red de seguridad contra el "Scope Creep")

```markdown
# Ideas y Líneas Futuras de Desarrollo

Este documento recoge mejoras y funcionalidades identificadas que, por limitación de tiempo del TFC, se han dejado para versiones futuras del sistema.

## Fase 6 (Bonus / Corto Plazo)
- [ ] **Plugin de Base de Datos:** Permitir al usuario elegir entre SQLite (por defecto) y SQL Server desde el generador, modificando dinámicamente el `DbContext` y las cadenas de conexión.
- [ ] **Botón de Limpieza:** Implementar en el generador una función que lea un `manifest.json` y elimine los archivos de la última entidad generada para permitir pruebas iterativas limpias.
- [ ] **Sistema de Login para acceder al Crud** Implementar que se genere ( o mute con el .tt) un sistema de login super básico para poder interactuar con el CRUD correspondiente.
- [ ] **Añadir Https** Implementar que las urls no solo sean por http si no por https y validar y certificar su uso.

## Infraestructura
- [ ] Migrar a HTTPS (API, Web, CORS, HttpClient BaseAddress, fetch JS y WebView de MAUI). Pospuesto: en localhost HTTP es suficiente; revisar en Fase 7.

## Pulido
- [ ] Portada (Home) con bienvenida y enlace a /Producto o con estilo de pantalla de carga.

    Subir Archivo .CSV directamente con input.
    Revisar Estilos.

página índice que liste entidades generadas + navbar dinámico

"Extraer el JS del Index a un crud-generico.js compartido en la RCL, parametrizado por entidad — reduce lo que T4 genera y centraliza la lógica." 

Mirar lo de si en vez de usar xp.css poder recrear la estética xp en una aplicación de escritorio o consola (al igual embebida? Ni idea)

Realizar el .exe ? 

 "Generador como MAUI borderless + WebView2 con drag region (estética XP pura, sin marco moderno). Hacer DESPUÉS de Fase 4/5, cuando MAUI ya esté instalado y el motor validado."

Si en el futuro quisieras que mostrara "Fecha Alta" con espacio, se podría insertar un espacio antes de cada mayúscula al generar el Display, pero es puro pulido y lo dejaría para Fase 7. 

script .bat para arrancar API+Web juntas sin tocar la regla de procesos separados"

"Filtro de campos booleanos con desplegable Sí/No en vez de caja de texto (mejora UX en Index y plantilla ViewIndex.tt)"

"Exportación de archivos desde el contenedor MAUI: el WebView no gestiona descargas. Opciones futuras: handler nativo de descargas en MAUI, o abrir el export en navegador externo."


## Versiones Futuras (Largo Plazo)
- [ ] **Soporte de Imágenes:** Añadir un tipo de campo "Image" que gestione `IFormFile`, guarde la ruta en la BD y muestre un preview en la vista MVC.
- [ ] **Relaciones entre Entidades:** Soportar generación de claves foráneas (1:N, N:M) y dropdowns dinámicos en los formularios.
- [ ] **Validaciones Avanzadas:** Inyección de DataAnnotations (`[Required]`, `[StringLength]`) en las plantillas T4 basadas en la configuración del usuario.
- [ ] **Tests Unitarios:** Generar automáticamente una clase de pruebas `XTests.cs` para la entidad creada.
- [ ] **Despliegue en Docker:** Crear un `docker-compose.yml` que levante la API, la Web y una base de datos SQL Server en contenedores.