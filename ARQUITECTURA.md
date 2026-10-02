# 🏗️ Arquitectura del Sistema: MetaCrudFosis

MetaCrudFosis está diseñado siguiendo los principios de **Clean Architecture** y **SOLID**, con un enfoque híbrido que combina un **núcleo genérico basado en Reflection** con un **motor de generación de código (Scaffolding)** personalizado. 

El objetivo es lograr una separación estricta de responsabilidades, permitiendo que la lógica de negocio sea independiente de la interfaz de usuario y de los detalles de implementación.

## Diagrama de Flujo de Generación

```text
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS GENERATOR                   │
│  (Capa de Presentación) -> Define Entidad, Campos y Estilos │
└──────────────────────────────┬──────────────────────────────┘
                               │
                               ▼ (Procesa plantillas .tt por sustitución de texto)
┌─────────────────────────────────────────────────────────────┐
│              MOTOR DE PLANTILLAS (Mutación en vivo)         │
│  Reemplaza placeholders: <#= EntityName #>, <#= Properties #>│
└──────────────────────────────┬──────────────────────────────┘
                               │ (Escribe SOLO archivos nuevos: .cs / .cshtml)
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                   PROYECTOS DESTINO                         │
│                                                             │
│  [MetaCrudFosis.Api]            [MetaCrudFosis.Web]         │
│  (Core/Infraestructura)         (Presentación)              │
│  Genera: Modelo de la entidad   Genera: Controlador MVC     │
│                                 y Vistas (.cshtml)          │
│                                                             │
│     NO se genera por entidad (son genéricos y resueltos     │
│   dinámicamente mediante Reflection en tiempo de ejecución):│
│     - GenericController<T>                                  │
│     - GenericRepository<T>                                  │
│     - ApplicationDbContext                                  │
└──────────────────────────────┬──────────────────────────────┘
                               │ (Consumen recursos compartidos)
                               ▼
┌─────────────────────────────────────────────────────────────┐
│                 META CRUD FOSIS SHARED.MODERN               │
│  (Recursos Transversales) -> Razor Class Library (CSS, Layouts)│
└─────────────────────────────────────────────────────────────┘
