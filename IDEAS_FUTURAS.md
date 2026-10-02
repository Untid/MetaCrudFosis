# 🗺️ Roadmap y Líneas Futuras de Desarrollo

Este documento actúa como el backlog técnico de MetaCrudFosis. Recoge las mejoras, refactorizaciones y nuevas funcionalidades identificadas durante el desarrollo del TFC, priorizadas para futuras iteraciones del sistema.

## 🚀 1. Funcionalidades Core (Corto Plazo)
- [ ] **Plugin de Persistencia Dinámica:** Permitir elegir entre SQLite (por defecto) y SQL Server desde el generador, modificando automáticamente el `DbContext` y las cadenas de conexión.
- [ ] **Gestión del Ciclo de Vida (Undo/Cleanup):** Implementar un `manifest.json` en el generador que permita eliminar limpiamente los archivos de la última entidad generada, facilitando pruebas iterativas sin dejar basura en el código.
- [ ] **Sistema de Autenticación y Autorización:** Incorporar ASP.NET Core Identity (o JWT) para proteger el acceso al CRUD, generando también las vistas y controladores de login mediante el motor de plantillas.

## 🧪 2. Calidad y Testing (Prioridad Alta)
- [ ] **Tests Unitarios del Núcleo:** Crear un proyecto de pruebas con **xUnit** (y Moq si aplica) para validar el `GenericRepository<T>` (operaciones CRUD y filtrado) y el `TemplateEngineService` (lógica de sustitución de placeholders).
- [ ] **Generación de Tests por Entidad:** Extender el motor de plantillas para que, al crear una entidad, genere automáticamente una clase de pruebas unitarias básica (`EntityNameTests.cs`).

## 🏗️ 3. Arquitectura y Generación de Código
- [ ] **Principio DRY en Frontend:** Extraer la lógica JavaScript del `Index` a un archivo `crud-generico.js` compartido en la Razor Class Library (RCL), parametrizado por el nombre de la entidad. Esto reduce la huella de código generado y centraliza el mantenimiento.
- [ ] **Validaciones Robustas:** Inyectar Data Annotations (`[Required]`, `[StringLength]`, `[EmailAddress]`) en las plantillas del modelo, basándose en la configuración definida por el usuario en el generador.
- [ ] **Soporte para Relaciones:** Implementar la generación de claves foráneas (1:N, N:M) y desplegables dinámicos en los formularios de creación/edición.
- [ ] **Gestión de Archivos:** Añadir un tipo de campo "Image/File" que gestione `IFormFile`, guarde la ruta en la BD y muestre una previsualización en la vista.

## 🎨 4. Experiencia de Usuario (UX/UI)
- [ ] **Dashboard Inicial:** Crear una pantalla de bienvenida (Home) que liste las entidades disponibles, complementando el navbar dinámico.
- [ ] **Mejora en la Importación:** Añadir un input de archivo para la importación directa de `.csv`, como alternativa al "Bulk Paste" actual.
- [ ] **Pulido de Formularios:** 
  - Reemplazar la caja de texto de campos booleanos por un desplegable o *toggle* (Sí/No).
  - Formatear automáticamente los nombres de propiedades (ej: insertar espacio antes de mayúsculas: `FechaAlta` → `Fecha Alta`).
- [ ] **Diseño Responsive:** Refinar el comportamiento de tablas y formularios en dispositivos móviles.

## 💻 5. Evolución del Generador (Cliente de Escritorio)
- [ ] **Nativo vs Web:** Explorar la migración del generador a una aplicación de escritorio nativa para una experiencia "Windows XP" más auténtica, sin depender de XP.css en un entorno web.
- [ ] **MAUI Borderless:** Investigar el uso de MAUI con ventanas sin bordes (`borderless`) + WebView2 con región de arrastre personalizada, eliminando el marco moderno de la ventana del sistema operativo.

## ☁️ 6. DevOps e Infraestructura
- [ ] **Contenerización con Docker:** Crear un `docker-compose.yml` que levante de forma desatendida la API, la aplicación Web y una instancia de SQL Server, facilitando el despliegue y el onboarding de nuevos desarrolladores.
- [ ] **Hardening de Seguridad:** Migrar las comunicaciones a HTTPS, configurando correctamente los certificados de desarrollo, CORS y la `BaseAddress` del HttpClient en MAUI (actualmente pospuesto, ya que HTTP es suficiente para el entorno de desarrollo local).
