
---

### 4. `IDEAS_FUTURAS.md` (Tu red de seguridad contra el "Scope Creep")

```markdown
# Ideas y Líneas Futuras de Desarrollo

Este documento recoge mejoras y funcionalidades identificadas que, por limitación de tiempo del TFC, se han dejado para versiones futuras del sistema.

## Fase 6 (Bonus / Corto Plazo)
- [ ] **Plugin de Base de Datos:** Permitir al usuario elegir entre SQLite (por defecto) y SQL Server desde el generador, modificando dinámicamente el `DbContext` y las cadenas de conexión.
- [ ] **Botón de Limpieza:** Implementar en el generador una función que lea un `manifest.json` y elimine los archivos de la última entidad generada para permitir pruebas iterativas limpias.

## Versiones Futuras (Largo Plazo)
- [ ] **Soporte de Imágenes:** Añadir un tipo de campo "Image" que gestione `IFormFile`, guarde la ruta en la BD y muestre un preview en la vista MVC.
- [ ] **Relaciones entre Entidades:** Soportar generación de claves foráneas (1:N, N:M) y dropdowns dinámicos en los formularios.
- [ ] **Validaciones Avanzadas:** Inyección de DataAnnotations (`[Required]`, `[StringLength]`) en las plantillas T4 basadas en la configuración del usuario.
- [ ] **Tests Unitarios:** Generar automáticamente una clase de pruebas `XTests.cs` para la entidad creada.
- [ ] **Despliegue en Docker:** Crear un `docker-compose.yml` que levante la API, la Web y una base de datos SQL Server en contenedores.